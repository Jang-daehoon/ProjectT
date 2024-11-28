using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class NodeTest : MonoBehaviour
{
    public delegate void SelectRoom();   //map node Ŭ�� �� ����
    public delegate void selectOtherRooom();    //�ٸ� map node Ŭ�� �� ����

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
                //�ִϸ��̼� ����
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
                //�ִϸ��̼� ����
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
                //�ִϸ��̼� ����
            }
        }
    }

    public RoomType RoomType { get; set; }
    private RoomManager roomManager => ServiceLocator.Instance.GetSecvice<RoomManager>();

    private void OnEnable()
    {
        isEnable = true;

        //�ִϸ��̼� ���� Goable, Bigger, Highlight
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

        //�̹���, �ƿ����� �ٲٱ�
        outLineImage.sprite = stageData.spriteOutline;
        childImage.sprite = stageData.sprite;

        RoomType = roomType;
        IsGenerate = true;
    }

    public void OnClickButton()
    {
        //�� 90->40
        //ũ�� 1.5��

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
    //�������� Ŭ���� �� 
    public void ClearRoom()
    {
        foreach (NodeTest connectedNode in connectedNodes)
        {
            connectedNode.isGoable = true;
        }
    }
}
