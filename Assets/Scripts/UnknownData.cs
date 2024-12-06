using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Unknown Data", menuName = "Scriptable Object/UnknownData")]
public class UnknownData : ScriptableObject
{
    public enum UnknownInfo { Battle, choice, Trap, compensation }  //전투, 선택지, 함정, 보상
    public UnknownInfo unknownInfo;

    //Unknown 이벤트에 앞서 먼저 Room 이름과 Room Imgae를 보여준 후 플레이어 위치 보여줌 
    //소환된 NPC와 상호작용을 해서 Unknown방에서 나갈 수 있다.
    [Header("EventNPC")]
    public GameObject NPC;  //소환되는 NPC

    [Header("Event")]
    public string roomName;
    public Sprite roomImage;
    [Multiline(10)]
    public string roomContents;
    public string[] optionText;
    public List<UnityEvent> optionEvent;

    [Space(3)]

    [Header("이벤트 종료")]
    [Multiline(10)]
    public string roomContentsAfter;
    public string[] afterOptionText;
    public List<UnityEvent> afterOptionEvent;

}
