using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class NodeTest : MonoBehaviour
{
    public delegate void SelectRoom();   //map node 클릭 시 실행
    public delegate void selectOtherRooom();    //다른 map node 클릭 시 실행

    public SelectRoom selectRoom;
    public selectOtherRooom otherRoom;

    private float posX;
    private float posY;
    private bool isGenerate;
    private bool isGoable;
    private bool isBigger;
    private bool isHighlight;

    private bool isEnable = false;

    private StageData _stageData;

    public List<NodeTest> connectedNodes;

    //[SerializeField] private Animator animator;
    [SerializeField] private Image outLineImage;
    [SerializeField] private Image childImage;
    [SerializeField] private Color unSelectedColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private GameObject clearCheck;

    public float PosX => posX;
    public float PosY => posY;
    public bool IsGenerate { get { return isGenerate; } set { isGenerate = value; } }
    public bool IsGoable
    {
        get { return isGoable; }
        set
        {
            isGoable = value;
            if (isEnable)
            {
                isBigger = false;
                isHighlight = false;
                //애니메이션 세팅
            }
        }
    }
    public bool IsBigger
    {
        get { return isBigger; }
        set
        {
            isBigger = value;

            if (isEnable)
            {
                //애니메이션 세팅
            }
        }
    }

    public bool IsHighlight
    {
        get { return IsHighlight; }
        set
        {
            IsHighlight = value;
            if (isEnable)
            {
                if (isGoable)
                    return;
                isBigger = false;
                //애니메이션 세팅
            }
        }
    }

    public RoomType RoomType { get; set; }
    private RoomManager roomManager => ServiceLocator.Instance.GetSecvice<RoomManager>();

    private void OnEnable()
    {
        isEnable = true;

        //애니메이션 세팅 Goable, Bigger, Highlight
    }

    private void OnDisable()
    {
        isEnable = false;
    }
    public void InitRoom(float PosX, float PosY)
    {
        posX = PosX;
        posY = PosY;

        isGenerate = false;
        isGoable = false;

        connectedNodes = new List<NodeTest>();

        GetComponent<Button>().onClick.AddListener(() => selectRoom());

        selectRoom += OnClickButton;
    }
    public void Positioning()
    {
        transform.localPosition = new Vector3(PosX, posY, 0);
        transform.SetAsLastSibling();
    }
    public void SetStageType(StageData stageData, RoomType roomType)
    {
        _stageData = stageData;

        //이미지, 아웃라인 바꾸기
        outLineImage.sprite = stageData.spriteOutline;
        childImage.sprite = stageData.sprite;

        RoomType = roomType;
        IsGenerate = true;
    }

    public void OnClickButton()
    {
        //색 90->40
        //크기 1.5배

        if (isGoable)
        {
            isGoable = false;

            GameManager.Game.CurrentRoom = this;

            roomManager.EnterRoom(RoomType);

            clearCheck.SetActive(true);
        }
        else
        {
            GameManager.Game.SelectedRoom = this;
        }
    }
    //스테이지 클리어 시 
    public void ClearRoom()
    {
        foreach (NodeTest connectedNode in connectedNodes)
        {
            connectedNode.isGoable = true;
        }
    }
}
