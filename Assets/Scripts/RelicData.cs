using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
