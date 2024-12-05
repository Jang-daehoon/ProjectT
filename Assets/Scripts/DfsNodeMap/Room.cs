using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

//StagePrefab�� ������ �־���ϴ� ��ũ��Ʈ.
public class Room : MonoBehaviour
{
    public delegate void OnSelectRoom(); // �ش� �� ��ġ�� ����� ��������Ʈ
    public delegate void OnTouchOther(); // �ٸ� �� ��ġ�� ����� ��������Ʈ

    public OnSelectRoom onSelectRoom;
    public OnTouchOther onTouchOther;

    private float _posX;
    private float _posY;
    private bool _isGenerate;
    [SerializeField]private bool _isGoable;
    private bool _isBigger;
    private bool _isHighlight;

    private bool _isEnable = false;

    private StageData _stageData;

    public List<Room> connectedRooms;

    // [SerializeField] private Animator _anim;
    [SerializeField]
    private Image _outLineImage;
    [SerializeField]
    private Image _childImage;
    [SerializeField]
    private Color _unSelectedColor;
    [SerializeField]
    private Color _selectedColor;
    [SerializeField]
    private GameObject _clearCheck;

    //ĸ��ȭ
    //Property ���� (���� ǥ������ ����� �Ӽ��� ���� ������ ��ȯ)
    public float PosX => _posX; //�б����� �Ӽ� _posX�� ���� �о�´�.
    public float PosY => _posY; //�б����� �Ӽ� _posY�� ���� �о�´�.
    public bool IsGenerate { get { return _isGenerate; } set { _isGenerate = value; } } //���ο� ���� value�� ����
    //���� �ڵ� ���� �Ӽ����� ��ȯ ������ �Ӽ��� �߰� ������ �����Ƿ� �ڵ� ���� �Ӽ����� �ܼ�ȭ�� �� �ֽ��ϴ�:
    //public bool IsGenerate { get; set; }   
    
    public bool IsGoable
    {
        get { return _isGoable; }
        set //���ο� ���� value�� ���޵Ǹ�, �ʵ� _isGoable�� ����
        {
            _isGoable = value;

            if (_isEnable)
            {
                IsBigger = false;
                IsHighlight = false;
                //_anim.SetBool("isGoable", _isGoable);
            }
        }
    }

    public bool IsBigger    //���� ���� ������ �������� Ŀ���� �۾����� �ϵ��� �ϴ� �ִϸ��̼� ����
    {
        get { return _isBigger; }
        set
        {
            _isBigger = value;

            if (_isEnable)
            {
                //_anim.SetBool("isBigger", _isBigger);
            }
        }
    }

    public bool IsHighlight //�⺻�� -> ���̶���Ʈ�� �ݺ������� ���� ����Ǵ� �ִϸ��̼� ����
    {
        get { return _isHighlight; }
        set
        {
            _isHighlight = value;

            if (_isEnable)
            {
                if (IsGoable)
                    return;

                IsBigger = false;
                //_anim.SetBool("isHighlight", _isHighlight);
            }
        }
    }

    public ERoomType RoomType { get; set; }

    private void OnEnable()
    {
        _isEnable = true;

        //_anim.SetBool("isGoable", _isGoable);
        //_anim.SetBool("isBigger", false);
        //_anim.SetBool("isHighlight", false);
    }

    private void OnDisable()
    {
        _isEnable = false;
    }
    //���� ��ġ�� ���¸� �ʱ�ȭ�ϰ�, ��ư Ŭ�� �̺�Ʈ�� ����
    public void InitRoom(float posX, float posY)
    {
        _posX = posX;
        _posY = posY;

        _isGenerate = false;
        _isGoable = false;

        connectedRooms = new List<Room>();

        Button button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError($"Button component is missing on {gameObject.name}!");
            return;
        }

        button.onClick.AddListener(() =>
        {
            if (onSelectRoom != null)
            {
                onSelectRoom.Invoke();
            }
            else
            {
                Debug.LogError("onSelectRoom delegate is null!");
            }
        });

        onSelectRoom += OnClickButton;
    }


    public void Positioning()
    {
        //�θ� ������Ʈ �������� ������Ʈ�� ���� ��ǥ ����
        transform.localPosition = new Vector3(_posX, _posY, 0);
        transform.SetAsLastSibling();   //���� ������Ʈ�� �ڽ��� �θ� ������Ʈ ������ ������ ������ �̵�
    }

    //���������� Ÿ���� �����ϴ� �޼���
    public void SetStageType(StageData stageData, ERoomType roomType)
    {
        _stageData = stageData;

        // �̹���, �ƿ����� �ٲٱ�
        _outLineImage.sprite = stageData.spriteOutline;
        _childImage.sprite = stageData.sprite;

        RoomType = roomType;

        _isGenerate = true;
    }

    public void OnClickButton()
    {
        // ���� 90 -> 40
        // ũ�� 1.5��

        if (_isGoable)
        {
            // �ش� �� ����
            IsGoable = false;

            GameManager.Game.CurrentRoom = this;
            RoomManager.Instance.EnterRoom(RoomType);
            _clearCheck.SetActive(true);
        }
        else
        {
            GameManager.Game.SelectedRoom = this;
        }
    }

    // �������� Ŭ���� �� 
    public void ClearRoom()
    {
        foreach (Room connectedRoom in connectedRooms)
        {
            connectedRoom.IsGoable = true;
        }
    }
}
