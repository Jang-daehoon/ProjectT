using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Unknown Data", menuName = "Scriptable Object/UnknownData")]
public class UnknownData : ScriptableObject
{
    public enum UnknownInfo { Battle, choice, Trap, compensation }  //����, ������, ����, ����
    public UnknownInfo unknownInfo;

    //Unknown �̺�Ʈ�� �ռ� ���� Room �̸��� Room Imgae�� ������ �� �÷��̾� ��ġ ������ 
    //��ȯ�� NPC�� ��ȣ�ۿ��� �ؼ� Unknown�濡�� ���� �� �ִ�.
    [Header("EventNPC")]
    public GameObject NPC;  //��ȯ�Ǵ� NPC

    [Header("Event")]
    public string roomName;
    public Sprite roomImage;
    [Multiline(10)]
    public string roomContents;
    public string[] optionText;
    public List<UnityEvent> optionEvent;

    [Space(3)]

    [Header("�̺�Ʈ ����")]
    [Multiline(10)]
    public string roomContentsAfter;
    public string[] afterOptionText;
    public List<UnityEvent> afterOptionEvent;

}
