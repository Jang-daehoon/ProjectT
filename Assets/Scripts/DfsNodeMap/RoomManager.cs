using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour, IRegisterable
{
    private int battle1Index = 0;
    private int battle2Index = 0;
    private int unknownIndex = 0;

    public void Init()
    {
        battle1Index = 0;
        battle2Index = 0;
        unknownIndex = 0;

    }

    public void EnterRoom(RoomType roomType)
    {

    }
    //�Ϲ� �� �濡 �� ��
    private void OnEnterEnemyRoom()
    {

    }
    //����Ʈ �濡 �� ��
    private void OnEnterEliteRoom()
    {

    }
    // ���� �濡 �� ��
    private void OnEnterMerchantRoom()
    {

    }
    // �޽� �濡 �� ��
    private void OnEnterRestRoom()
    {

    }
    // ���� �濡 �� ��
    private void OnEnterTreasureRoom()
    {

    }
    // ���� �濡 �� ��
    public void OnEnterBossRoom()
    {

    }
    // ���� �濡 �� ��
    private void OnEnterUnknownRoom()
    {

    }
    public void NextUnknown()
    {

    }

    public void AfterUnknown()
    {

    }

    public void AfterUnknown2()
    {

    }


    public void ClearRoom()
    {

    }
}
