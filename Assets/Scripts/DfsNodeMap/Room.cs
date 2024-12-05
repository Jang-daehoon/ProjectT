using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

//StagePrefab이 가지고 있어야하는 스크립트.
public class Room : MonoBehaviour
{
    public delegate void OnSelectRoom(); // 해당 방 터치시 실행될 델리게이트
    public delegate void OnTouchOther(); // 다른 곳 터치시 실행될 델리게이트

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

    //캡슐화
    //Property 정의 (람다 표현식을 사용해 속성의 값을 간단히 반환)
    public float PosX => _posX; //읽기전용 속성 _posX의 값을 읽어온다.
    public float PosY => _posY; //읽기전용 속성 _posY의 값을 읽어온다.
    public bool IsGenerate { get { return _isGenerate; } set { _isGenerate = value; } } //새로운 값을 value로 접근
    //완전 자동 구현 속성으로 변환 가능이 속성은 추가 로직이 없으므로 자동 구현 속성으로 단순화할 수 있습니다:
    //public bool IsGenerate { get; set; }   
    
    public bool IsGoable
    {
        get { return _isGoable; }
        set //새로운 값이 value로 전달되며, 필드 _isGoable에 저장
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

    public bool IsBigger    //맵의 선택 가능한 아이콘이 커졌다 작아졌다 하도록 하는 애니메이션 실행
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

    public bool IsHighlight //기본색 -> 하이라이트색 반복적으로 색이 변경되는 애니메이션 실행
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
    //방의 위치와 상태를 초기화하고, 버튼 클릭 이벤트를 설정
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
        //부모 오브젝트 기준으로 오브젝트의 로컬 좌표 설정
        transform.localPosition = new Vector3(_posX, _posY, 0);
        transform.SetAsLastSibling();   //현재 오브젝트를 자신의 부모 오브젝트 내에서 마지막 순서로 이동
    }

    //스테이지의 타입을 설정하는 메서드
    public void SetStageType(StageData stageData, ERoomType roomType)
    {
        _stageData = stageData;

        // 이미지, 아웃라인 바꾸기
        _outLineImage.sprite = stageData.spriteOutline;
        _childImage.sprite = stageData.sprite;

        RoomType = roomType;

        _isGenerate = true;
    }

    public void OnClickButton()
    {
        // 색깔 90 -> 40
        // 크기 1.5배

        if (_isGoable)
        {
            // 해당 방 진입
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

    // 스테이지 클리어 시 
    public void ClearRoom()
    {
        foreach (Room connectedRoom in connectedRooms)
        {
            connectedRoom.IsGoable = true;
        }
    }
}
