using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public Image progressBar;
    public Image backgroundImage; // ��� �̹����� ������ UI �̹���
    public Sprite eliteSceneBackground; // EliteScene ��� ��������Ʈ
    public Sprite bossSceneBackground;  // BossScene ��� ��������Ʈ
    public Sprite restartBackground;  // Restart ��� ��������Ʈ
    static string nextScene;

    private void Start()
    {
        SetBackgroundImage();
        StartCoroutine(LoadingSceneProgress());
    }
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    private void SetBackgroundImage()
    {
        if (backgroundImage != null)
        {
            if (nextScene == "EliteScene")
            {
                backgroundImage.sprite = eliteSceneBackground;
            }
            else if (nextScene == "BossScene")
            {
                backgroundImage.sprite = bossSceneBackground;
            }
            else if (nextScene == "Scenes/TitleScene")
            {
                backgroundImage.sprite = restartBackground;
            }
        }
    }
    public IEnumerator LoadingSceneProgress()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        //���ҽ� �ε��� ������ ���� Scene Loading�� ������ �ε����� ���� ������Ʈ ������ ���� ����
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }

        }
    }
}
