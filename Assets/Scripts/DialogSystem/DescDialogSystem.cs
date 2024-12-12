using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//세계관 및 상황 설명할 때 사용하는 스크립트입니다.
public class DescDialogSystem : MonoBehaviour
{
    [SerializeField]
    private DialogIndex[] dialogIndex;  //설명을 진행할 때 사용되는 Dialog의 배열
    [SerializeField]
    private DialogData[] dialogs;   //현재 분기의 대사 목록 배열
    [SerializeField]
    private bool isAutoStart = true;    //자동시작 여부
    private bool isFirst = true;    //최초 1회만 호출하기 위한 변수
    private int currentDialogIndex = -1;    //현재 대사 순번
    private int currentDialogIndexNum = 0;  //현재 설명을 하는 dialogIndex의 배열 순번
    [SerializeField]private float typingSpeed = 0.1f;   //텍스트 타이핑 효과의 재생속도
    private bool isTypingEffect = false;    //텍스트 타이핑 효과를 재생중인지 확인하는 변수

    private void Awake()
    {
        Setup();
    }
    private void Setup()
    {
        //모든 대화 관련 게임오브젝트 비활성화
        for(int i = 0; i< dialogIndex.Length; ++i)
        {
            SetActiveObjects(dialogIndex[i], false);
            //설명 이미지는 보이도록 설정
            dialogIndex[i].imageDescObj.gameObject.SetActive(true);
        }
    }

    public bool UpdateDialog()
    {
        //대사 분기가 시작될 때 1회만 호출
        if(isFirst == true)
        {
            //초기화, 설명 이미지는 활성화 하고, 대사 관련 UI는 모두 비활성화
            Setup();

            //자동재생(isAutoStart = true_으로 설정되어 있으면 첫번째 대사 진행
            if(isAutoStart == true)
            {
                SetNextDialog();
                isFirst = false;    
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            //텍스트 타이핑 효과를 재생중일 때 마우스 왼쪽 클릭하면 타이핑 효과 종료
            if (isTypingEffect == true)
            {
                isTypingEffect = false;

                //타이핑 효과를 중지하고, 현재 대사 전체를 출력한다.
                StopCoroutine("OnTypingText");
                dialogIndex[currentDialogIndex].textDialogue.text = dialogs[currentDialogIndexNum].dialogue;
                //대사가 완료되었을 때 출력되는 커서 활성화
                dialogIndex[currentDialogIndex].objectArrow.SetActive(true);

                return false;
            }
            //대사가 남아있을 경우 다음 대사 진행
            if (dialogs.Length > currentDialogIndexNum + 1)
            {
                SetNextDialog();
            }
            //대사가 더 이상 없을 경우 모든 오브젝트를 비활성화 하고 true 반환
            else
            {
                //현재 대화에 참여했던 모든 캐릭터, 대화 관련 UI를 보이지 않게 비활성화
                for(int i = 0; i<dialogIndex.Length; ++i)
                {
                    SetActiveObjects(dialogIndex[i], false );
                    //SetActiveObjects()에 캐릭터 이미지를 보이지 않게 하는 부분이 없기에 별도로 호출
                    dialogIndex[i].imageDescObj.gameObject.SetActive(false);
                }
                return true;
            }
        }
        return false;
    }

    private void SetNextDialog()
    {
        //이전 설명의 대화 관련 오브젝트 비활성화
        SetActiveObjects(dialogIndex[currentDialogIndexNum], false);

        //다음 대사를 진행하도록
        currentDialogIndex++;

        // 설명 시 출력할 이미지 설정
        dialogIndex[currentDialogIndex].imageDescObj.sprite = dialogs[currentDialogIndex].descImage;

        //현재 설명 순번 설정
        currentDialogIndexNum = dialogs[currentDialogIndex].DescIndex;

        //현재 설명관련 오브젝트 활성화
        SetActiveObjects(dialogIndex[currentDialogIndexNum], true);
        //현재 설명 대사 설정
        StartCoroutine("OnTypingText");
    }

    private void SetActiveObjects(DialogIndex DescIndex, bool visible)
    {
        DescIndex.imageDialog.gameObject.SetActive (visible);
        DescIndex.textDialogue.gameObject.SetActive (visible);

        //화살표는 대사가 종료되었을 때만 활성화 하기 때문에 항상 false
        DescIndex.objectArrow.SetActive(false);     
    }

    private IEnumerator OnTypingText()
    {
        int index = 0;
        isTypingEffect = true;

        //텍스트를 한글자씩 타이핑 치듯 재생
        while(index < dialogs[currentDialogIndexNum].dialogue.Length)
        {
            dialogIndex[currentDialogIndex].textDialogue.text = dialogs[currentDialogIndexNum].dialogue.Substring(0, index);

            index++;

            yield return new WaitForSeconds(typingSpeed);
        }
        isTypingEffect =false;

        //설명이 종료되었을 때 출력되는 커서 활성화
        dialogIndex[currentDialogIndex].objectArrow.SetActive(true);
    }
}
[System.Serializable]
public struct DialogIndex
{
    public Image imageDescObj;   //설명 이미지 
    public Image imageDialog;   //대화창 이미지
    public TextMeshProUGUI textDialogue;    //현재 대사 출력
    public GameObject objectArrow;  //대사 완료 시 출력되는 커서 오브젝트
}

[System.Serializable]
public struct DialogData
{
    public Sprite descImage; //설명에 출력할 이미지
    public int DescIndex;   //대사를 출력할 DialogIndex의 배열 순번
    [TextArea(7, 7)]
    public string dialogue; //설명할 대사
}