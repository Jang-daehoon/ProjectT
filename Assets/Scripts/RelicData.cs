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
    [Tooltip("���� ���")]
    public Rarity rarity;
    [Tooltip("���� ��� ��")]
    public Color color;
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
    private void OnEnable()
    {
        //���� ��޿� ���� ������޻� ����
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
