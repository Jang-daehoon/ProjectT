using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour, IRegisterable
{
    //[SerializeField] private MapUI mapUI;

    //[Header("Battle")]
    //[SerializeField] private List<BattleData> firstAct1BattleData;
    //[SerializeField] private List<BattleData> secondAct1BattleData;
    //[SerializeField] private List<BattleData> eliteAct1BattleData;
    //[SerializeField] private List<BattleData> bossAct1BattleData;
    //[Space(3)]

    private bool _isEarly = true;
    private bool isBossGoable = false;
    private int height = 1;

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
    //일반 적 방에 들어갈 때
    private void OnEnterEnemyRoom()
    {

    }
    //엘리트 방에 들어갈 때
    private void OnEnterEliteRoom()
    {

    }
    // 상인 방에 들어갈 때
    private void OnEnterMerchantRoom()
    {

    }
    // 휴식 방에 들어갈 때
    private void OnEnterRestRoom()
    {

    }
    // 보물 방에 들어갈 때
    private void OnEnterTreasureRoom()
    {

    }
    // 보스 방에 들어갈 때
    public void OnEnterBossRoom()
    {

    }
    // 랜덤 방에 들어갈 때
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
