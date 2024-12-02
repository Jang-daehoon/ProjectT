using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private Speaker[] speakers; //Speaker의 Dialog배열
    [SerializeField]
    private Dialog[] dialogs; //현재 분기의 대사 목록 배열

    [SerializeField]
    private bool isAutoStart = true;    //자동시작 여부
    private bool isFirst = true;    //최초 1회만 호출하기 위한 변수
    private int currentDialogIndex = -1;    //현재 대사 순번
    private int currentDialogIndexNum = 0;  //현재 설명을 하는 dialogIndex의 배열 순번
    [SerializeField] private float typingSpeed = 0.1f;   //텍스트 타이핑 효과의 재생속도
    private bool isTypingEffect = false;    //텍스트 타이핑 효과를 재생중인지 확인하는 변수

    private void Awake()
    {
        Setup();
    }
    private void Setup()
    {
        //모든 대화 관련 게임오브젝트 비활성화
        for (int i = 0; i < speakers.Length; ++i)
        {
            SetActiveObjects(speakers[i], false);
            //캐릭터 이름 설정
            speakers[i].textName.text = dialogs[currentDialogIndexNum].Name;
            //캐릭터 이미지는 보이도록 설정
            speakers[i].characterImage.gameObject.SetActive(true);
        }
    }

    public bool UpdateDialog()
    {
        //대사 분기가 시작될 때 1회만 호출
        if (isFirst == true)
        {
            //초기화, 설명 이미지는 활성화 하고, 대사 관련 UI는 모두 비활성화
            Setup();

            //자동재생(isAutoStart = true_으로 설정되어 있으면 첫번째 대사 진행
            if (isAutoStart == true)
            {
                SetNextDialog();
                isFirst = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //이름 변경
            speakers[currentDialogIndex].textName.text = dialogs[currentDialogIndexNum].Name;
            //텍스트 타이핑 효과를 재생중일 때 마우스 왼쪽 클릭하면 타이핑 효과 종료
            if (isTypingEffect == true)
            {
                isTypingEffect = false;

                //타이핑 효과를 중지하고, 현재 대사 전체를 출력한다.
                StopCoroutine("OnTypingText");
                speakers[currentDialogIndex].textDialogue.text = dialogs[currentDialogIndexNum].dialogue;
                //대사가 완료되었을 때 출력되는 커서 활성화
                speakers[currentDialogIndex].objectArrow.SetActive(true);

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
                for (int i = 0; i < speakers.Length; ++i)
                {
                    SetActiveObjects(speakers[i], false);
                    //SetActiveObjects()에 캐릭터 이미지를 보이지 않게 하는 부분이 없기에 별도로 호출
                    speakers[i].characterImage.gameObject.SetActive(false);
                }
                return true;
            }
        }
        return false;
    }

    private void SetNextDialog()
    {
        //이전 설명의 대화 관련 오브젝트 비활성화
        SetActiveObjects(speakers[currentDialogIndexNum], false);

        //다음 대사를 진행하도록
        currentDialogIndex++;

        // 설명 시 출력할 이미지 설정
        speakers[currentDialogIndex].characterImage.sprite = dialogs[currentDialogIndex].charPortrait;

        //현재 설명 순번 설정
        currentDialogIndexNum = dialogs[currentDialogIndex].dialogIndex;

        //현재 설명관련 오브젝트 활성화
        SetActiveObjects(speakers[currentDialogIndexNum], true);
        //현재 설명 대사 설정
        StartCoroutine("OnTypingText");
    }

    private void SetActiveObjects(Speaker speakers, bool visible)
    {
        speakers.characterImage.gameObject.SetActive(visible); //캐릭터 이미지 활성화
        speakers.charNameImage.gameObject.SetActive(visible);   //캐릭터 이름 이미지 활성화
        speakers.textDialogue.gameObject.SetActive(visible);    //대화창 이미지 활성화
        speakers.textName.gameObject.SetActive(visible);    //캐릭터 이름 활성화
        speakers.textDialogue.gameObject.SetActive(visible);   //대화창 텍스트 이미지 활성화

        //화살표는 대사가 종료되었을 때만 활성화 하기 때문에 항상 false
        speakers.objectArrow.SetActive(false);
    }

    private IEnumerator OnTypingText()
    {
        int index = 0;
        isTypingEffect = true;

        //텍스트를 한글자씩 타이핑 치듯 재생
        while (index < dialogs[currentDialogIndexNum].dialogue.Length)
        {
            speakers[currentDialogIndex].textDialogue.text = dialogs[currentDialogIndexNum].dialogue.Substring(0, index);

            index++;

            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        isTypingEffect = false;

        //설명이 종료되었을 때 출력되는 커서 활성화
        speakers[currentDialogIndex].objectArrow.SetActive(true);
    }
}

[System.Serializable]
public struct Speaker
{
    public Image characterImage;    //캐릭터 초상화 오브젝트
    public Image charNameImage; //캐릭터 이름 이미지 오브젝트
    public Image dialogImage;   //대화창 이미지 오브젝트
    
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDialogue;    //대사 오브젝트
    public GameObject objectArrow;  //커서 오브젝트
}
[System.Serializable]
public struct Dialog
{
    public Sprite charPortrait;  //캐릭터 초상화
    public int dialogIndex; //대사를 출력할 Speaker의 배열 순번
    public string Name;
    [TextArea(5, 5)]
    public string dialogue;
}