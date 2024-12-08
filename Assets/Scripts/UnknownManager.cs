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
        //렐릭 생성
        //stageManager를 찾아서 spawnPos를 찾아서 해당 위치에 소환
    }
    public void someoneIsWatchingMe()
    {
        GameManager.Instance.player.curHp -= 10;
    }
    public void RunAway()
    {
        RoomManager.Instance.ClearRoom();
    }
}
