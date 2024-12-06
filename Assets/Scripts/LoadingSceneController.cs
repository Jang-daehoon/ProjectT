using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public Image progressBar;
    static string nextScene;

    private void Start()
    {
        StartCoroutine(LoadingSceneProgress());
    }
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
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
