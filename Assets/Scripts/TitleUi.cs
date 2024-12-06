using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUi : MonoBehaviour
{
    [Header("Desc Dialogs UI")]
    [SerializeField] private DescDialogSystem firstDescWorld;
    [Header("TitleButton")]
    [SerializeField] private Button gameStartBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button exitBtn;
    [Header("Title UI Set")]
    [SerializeField] private GameObject Title;
    [Header("Fade InOut Obj")]
    [SerializeField] private UIFadeInOut FadeObj;

    private void Start()
    {
        // 버튼 클릭 이벤트 리스너 추가
        gameStartBtn.onClick.AddListener(() => OnGameStartClicked());
        optionBtn.onClick.AddListener(() => OnOptionClicked());
        exitBtn.onClick.AddListener(() => OnExitClicked());
    }

    private void OnGameStartClicked()
    {
        StartCoroutine(StartFirstDescDialog());
        Debug.Log("Game Start Button Clicked");
    }
    private IEnumerator StartFirstDescDialog()
    {
        FadeObj.gameObject.SetActive(true);
        //FadeIn 시작
        FadeObj.isFadeIn = true;
        yield return new WaitForSeconds(FadeObj.duration);

        firstDescWorld.gameObject.SetActive(true);  
        //대화 로그 실행
        yield return new WaitUntil(() => firstDescWorld.UpdateDialog());

        //Scene이동
        Debug.Log("Scene Movement");

        LoadingSceneController.LoadScene("TutorialScene");
        //ScenesManager.Instance.ChanageScene("LoadingScene");
    }
    private void OnOptionClicked()
    {
        Debug.Log("Option Button Clicked");
        // 옵션 설정 화면 띄우기
    }

    private void OnExitClicked()
    {
        Debug.Log("Exit Button Clicked");
        // 게임 종료
    }
}
