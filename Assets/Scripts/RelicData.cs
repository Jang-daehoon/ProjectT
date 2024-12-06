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
    [Tooltip("���� ���")]
    public Rarity rarity;
    [Tooltip("���� �̸�")]
    public string relicName;
    [Tooltip("���� ID")]
    public int relicID;
    [Tooltip("���� �̹���")]
    public Sprite relicSprite;
    [Tooltip("���� ����"), TextArea(3,3)]
    public string relicDesc;
    [Tooltip("���� ���� ����")]
    public bool isLevelPlus;
    [Tooltip("������ �ø��� ����")]
    public string statName;
    [Tooltip("������ ��ġ")]
    public float relicStatsPoint;
}
