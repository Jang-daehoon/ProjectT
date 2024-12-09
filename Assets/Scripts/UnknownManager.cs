using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnknownManager : Singleton<UnknownManager>
{
   
    //Unknown에서 사용할 메서드들

    //랜덤 렐릭 획득
    public void GetRandomRelic()
    {
        // StageManager 컴포넌트 검색
        StageManager stageManager = FindObjectOfType<StageManager>();

        // StageManager가 존재하는지 확인
        if (stageManager != null)
        {
            //렐릭 생성
            //stageManager를 찾아서 spawnPos를 찾아서 해당 위치에 소환
            Instantiate(stageManager.rewardItemPrefab, stageManager.rewardSpawnPoint.position, Quaternion.identity);
            Debug.Log("보상 아이템이 생성되었습니다.");

            // unknownClear 속성을 true로 설정
            stageManager.unknownClear = true;
            Debug.Log("StageManager의 unknownClear가 true로 변경되었습니다.");
        }
        else
        {
            Debug.LogError("StageManager를 찾을 수 없습니다.");
        }

        if (UiManager.Instance.isUnknownUiActive == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi, ref UiManager.Instance.isUnknownUiActive);
        }
        else if (UiManager.Instance.isUnknownUiActive2 == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi2, ref UiManager.Instance.isUnknownUiActive);
        }
        else if (UiManager.Instance.isUnknownUiActive3 == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi3, ref UiManager.Instance.isUnknownUiActive);
        }

        RoomManager.Instance.ClearRoom();
    }

    public void someoneIsWatchingMe()
    {
        int ranDamage =  Random.Range(10, 30);
        GameManager.Instance.player.curHp -= ranDamage;

        // StageManager 컴포넌트 검색
        StageManager stageManager = FindObjectOfType<StageManager>();

        // StageManager가 존재하는지 확인
        if (stageManager != null)
        {
            // unknownClear 속성을 true로 설정
            stageManager.unknownClear = true;
            Debug.Log("StageManager의 unknownClear가 true로 변경되었습니다.");
        }
        else
        {
            Debug.LogError("StageManager를 찾을 수 없습니다.");
        }

        if (UiManager.Instance.isUnknownUiActive == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi, ref UiManager.Instance.isUnknownUiActive);
        }
        else if (UiManager.Instance.isUnknownUiActive2 == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi2, ref UiManager.Instance.isUnknownUiActive);
        }
        else if (UiManager.Instance.isUnknownUiActive3 == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi3, ref UiManager.Instance.isUnknownUiActive);
        }

        RoomManager.Instance.ClearRoom();
    }

    public void RunAway()
    {
        // StageManager 컴포넌트 검색
        StageManager stageManager = FindObjectOfType<StageManager>();

        // StageManager가 존재하는지 확인
        if (stageManager != null)
        {
            // unknownClear 속성을 true로 설정
            stageManager.unknownClear = true;
            Debug.Log("StageManager의 unknownClear가 true로 변경되었습니다.");
        }
        else
        {
            Debug.LogError("StageManager를 찾을 수 없습니다.");
        }

        if (UiManager.Instance.isUnknownUiActive == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi, ref UiManager.Instance.isUnknownUiActive);
        }
        else if (UiManager.Instance.isUnknownUiActive2 == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi2, ref UiManager.Instance.isUnknownUiActive);
        }
        else if (UiManager.Instance.isUnknownUiActive3 == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi3, ref UiManager.Instance.isUnknownUiActive);
        }

        RoomManager.Instance.ClearRoom();
    }
    
}
