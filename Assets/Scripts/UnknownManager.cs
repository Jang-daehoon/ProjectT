using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnknownManager : Singleton<UnknownManager>
{
   
    //Unknown���� ����� �޼����

    //���� ���� ȹ��
    public void GetRandomRelic()
    {
        //���� ����
        //stageManager�� ã�Ƽ� spawnPos�� ã�Ƽ� �ش� ��ġ�� ��ȯ
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
