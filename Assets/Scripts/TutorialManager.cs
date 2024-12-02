using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool isFirstTutirial;    //Tutorial 최초 시작
    public DialogSystem firstTutorialDialog;
    public DialogSystem secondTutorialDialog;


    // Start is called before the first frame update
    private IEnumerator Start()
    {
        if (isFirstTutirial == false)
        {
            isFirstTutirial = true;
            //튜토리얼 대사 시작
            yield return new WaitUntil(() => firstTutorialDialog.UpdateDialog());

            //몬스터 생성
            Debug.Log("몬스터 생성 메서드 시작");
            Time.timeScale = 0;

            secondTutorialDialog.gameObject.SetActive(true);
            yield return null;

            //튜토리얼 대사 시작
            yield return new WaitUntil(() => secondTutorialDialog.UpdateDialog());
            Time.timeScale = 1;

            //생성된 모든 몬스터 처치

            //처치 완료 시 보상 아이템 생성

            //튜토리얼 대사 시작

            //보상 아이템 획득 시 포탈 생성

            //튜토리얼 대사 시작

            //포탈 상호작용 시 Map UI 활성화
            //튜토리얼 대사 시작

            //맵 UI 클릭 시 해당 위치로 이동 

            //->튜토리얼 종료
        }
        else
            yield break;
    }
    
}
