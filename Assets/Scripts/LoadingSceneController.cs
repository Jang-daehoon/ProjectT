using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public Image progressBar;
    public Image backgroundImage; // 배경 이미지를 변경할 UI 이미지
    public Sprite eliteSceneBackground; // EliteScene 배경 스프라이트
    public Sprite bossSceneBackground;  // BossScene 배경 스프라이트
    public Sprite restartBackground;  // Restart 배경 스프라이트
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
        //리소스 로딩이 끝나기 전에 Scene Loading이 끝나면 로딩되지 않은 오브젝트 깨지는 현상 방지
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
