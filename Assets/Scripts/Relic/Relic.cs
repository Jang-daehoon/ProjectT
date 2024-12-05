using System.Collections;
using System.Collections.Generic;
using HoonsCodes;
using System;
using UnityEngine;

public class Relic : MonoBehaviour
{
    public RelicData relicData;
    

    public void GetRelic()
    {
        switch(relicData.statName)
        {
            case RelicData.StatsName.maxHp :
                if (relicData.isLevelPlus == true)
                {
                    float inf = GameManager.Instance.player.Level * relicData.relicStatsPoint;
                    GameManager.Instance.player.HpPlus(inf);
                }
                else
                {
                    GameManager.Instance.player.HpPlus(relicData.relicStatsPoint);
                }
                break;
            case RelicData.StatsName.moveSpeed :
                if (relicData.isLevelPlus == true)
                {
                    float inf = GameManager.Instance.player.Level * relicData.relicStatsPoint;
                    GameManager.Instance.player.moveSpeed += inf;
                }
                else
                {
                    GameManager.Instance.player.moveSpeed += relicData.relicStatsPoint;
                }
                break;
            case RelicData.StatsName.dmgValue :
                if (relicData.isLevelPlus == true)
                {
                    float inf = GameManager.Instance.player.Level * relicData.relicStatsPoint;
                    GameManager.Instance.player.dmgValue += inf;
                }
                else
                {
                    GameManager.Instance.player.dmgValue += relicData.relicStatsPoint;
                }
                break;
            case RelicData.StatsName.atkSpeed :
                if (relicData.isLevelPlus == true)
                {
                    float inf = GameManager.Instance.player.Level * relicData.relicStatsPoint;
                    GameManager.Instance.player.atkSpeed += inf;
                }
                else
                {
                    GameManager.Instance.player.atkSpeed += relicData.relicStatsPoint;
                }
                break;
        }
    }
}
