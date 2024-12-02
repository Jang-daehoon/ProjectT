using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    [Header("Desc Dialogs UI")]
    [SerializeField] private DescDialogSystem firstDescWorld;
    [Header("TitleButton")]
    [SerializeField] private Button gameStartBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button exitBtn;
    [Header("Title UI Set")]
    [SerializeField] private GameObject Title;  

    private void Start()
    {
        // 버튼 클릭 이벤트 리스너 추가
        gameStartBtn.onClick.AddListener(() => OnGameStartClicked());
        optionBtn.onClick.AddListener(() => OnOptionClicked());
        exitBtn.onClick.AddListener(() => OnExitClicked());
    }

    private void OnGameStartClicked()
    {
        Title.SetActive(false);
        StartCoroutine(StartFirstDescDialog());
        Debug.Log("Game Start Button Clicked");
    }
    private IEnumerator StartFirstDescDialog()
    {
        //대화 로그 실행
        yield return new WaitUntil(() => firstDescWorld.UpdateDialog());

        //원하는 행동 추가 가능

        //Scene이동
        Debug.Log("Scene Movement");
        // 게임 시작 로직 ->Stage1으로 이동
        ScenesManager.Instance.ChanageScene("Stage1");
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
