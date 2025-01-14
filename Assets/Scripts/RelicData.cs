using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RelicData;

[CreateAssetMenu(fileName = "Relic Data", menuName = "Scriptable Object/RelicData")]
public class RelicData : ScriptableObject
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic
    }
    [Tooltip("유물 등급")]
    public Rarity rarity;
    [Tooltip("유물 등급 색")]
    public Color color;
    [Tooltip("유물 이름")]
    public string relicName;
    [Tooltip("유물 ID")]
    public int relicID;
    [Tooltip("유물 이미지")]
    public Sprite relicSprite;
    [Tooltip("유물 설명"), TextArea(3,3)]
    public string relicDesc;
    [Tooltip("레벨 관여 여부")]
    public bool isLevelPlus;
    [Tooltip("유물이 올리는 스텟")]
    public string statName;
    [Tooltip("오르는 수치")]
    public float relicStatsPoint;
    private void OnEnable()
    {
        //유물 등급에 따라서 유물등급색 적용
        switch (this.rarity)
        {
            case Rarity.Common:
                color = Color.white;
                break;
            case Rarity.Uncommon:
                color = Color.cyan;
                break;
            case Rarity.Rare:
                color = Color.yellow;
                break;
            default:
                color = Color.red;
                break;
        }
    }
}
