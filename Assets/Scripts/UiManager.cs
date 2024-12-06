using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    [SerializeField] private Canvas inGameCanvas;

    [Header("플레이어 상호작용 감지 UI")]
    public GameObject interactiveObjUi;
    public TextMeshProUGUI interactiveText;


    public GameObject PlayerStatusUiObj;
    //아르카나 UI
    public GameObject ArcanaUIObj;
    //Map UI
    public GameObject MapUIObj;
    //FadeObj
    public UIFadeInOut FadeObj;
    //UnknownUI Obj
    public GameObject UnknownUIObj;

    [Header("ArcanaImage")]
    public Image firstArcanaImg;
    public Image secondArcanaImg;
    public Image thirdArcanaImg;
    [Header("ArcanaButton")]
    public Button firstArcana;
    public Button secondArcana;
    public Button thirdArcana;
    [Header("ArcanaName")]
    public TextMeshProUGUI firstArcanaName;
    public TextMeshProUGUI secondArcanaName;
    public TextMeshProUGUI thirdArcanaName;
    [Header("ArcanaDesc")]
    public TextMeshProUGUI firstArcanaDesc;
    public TextMeshProUGUI secondArcanaDesc;
    public TextMeshProUGUI thirdArcanaDesc;

    [Header("UnknownUI")]
    public TextMeshProUGUI unknownRoomName;
    public Image unknownRoomImage;
    public TextMeshProUGUI unknownRoomContext;
    public Button[] unknownEventButton;

    
    //활성화 유무 확인 변수
    public bool isDialogUiActive;
    public bool isArcanaUIActive;
    public bool isMapUIActive;
    public bool isInteractiveUiActive;
    private void Awake()
    {
        isInteractiveUiActive = false;
        isDialogUiActive = false;
        isArcanaUIActive = false;
        isMapUIActive = false;
    }
    private void Update()
    {
        if(isDialogUiActive == true || isMapUIActive == true || isArcanaUIActive == true)
            PlayerStatusUiObj.SetActive(false);
        else
            PlayerStatusUiObj.SetActive(true);
    }

    /// <summary>
    /// Arcana UI 업데이트
    /// </summary>
    /// <param name="arcanaData">선택된 3개의 아르카나 데이터</param>
    public void UpdateArcanaUI(List<ArcanaData> arcanaData)
    {
        if (arcanaData.Count < 3)
        {
            Debug.LogError("Insufficient Arcana data provided to UpdateArcanaUI!");
            return;
        }

        // 기존 리스너 제거
        firstArcana.onClick.RemoveAllListeners();
        secondArcana.onClick.RemoveAllListeners();
        thirdArcana.onClick.RemoveAllListeners();

        // 첫 번째 아르카나
        firstArcanaImg.sprite = arcanaData[0].ArcanaImage;
        firstArcanaName.text = arcanaData[0].name;
        firstArcanaDesc.text = arcanaData[0].ArcanaDesc;

        // 두 번째 아르카나
        secondArcanaImg.sprite = arcanaData[1].ArcanaImage;
        secondArcanaName.text = arcanaData[1].name;
        secondArcanaDesc.text = arcanaData[1].ArcanaDesc;

        // 세 번째 아르카나
        thirdArcanaImg.sprite = arcanaData[2].ArcanaImage;
        thirdArcanaName.text = arcanaData[2].name;
        thirdArcanaDesc.text = arcanaData[2].ArcanaDesc;

        // 버튼 클릭 이벤트 등록
        firstArcana.onClick.AddListener(() => ArcanaSelect(arcanaData[0]));
        secondArcana.onClick.AddListener(() => ArcanaSelect(arcanaData[1]));
        thirdArcana.onClick.AddListener(() => ArcanaSelect(arcanaData[2]));
    }

    //UI토글
    public void ToggleUIElement(GameObject uiElement, ref bool isActive)
    {
        isActive = !isActive;
        uiElement.SetActive(isActive);
    }

    /// <summary>
    /// 선택된 아르카나 처리
    /// </summary>
    /// <param name="selectedArcana">선택된 아르카나 데이터</param>
    public void ArcanaSelect(ArcanaData selectedArcana)
    {
        if (selectedArcana == null)
        {
            Debug.LogError("Invalid Arcana selected!");
            return;
        }

        Debug.Log($"Selected Arcana: {selectedArcana.name}");

        // 선택된 아르카나에 대한 로직
        switch (selectedArcana.ArcanaId)
        {
            case 0:
                ArcanaManager.Instance.canEnhanceMeleeAttack = true;
                break;
            case 1:
                ArcanaManager.Instance.canRandomBulletInit = true;
                break;
            case 2:
                ArcanaManager.Instance.canCatalyst = true;
                break;
            default:
                Debug.LogWarning($"Unhandled ArcanaId: {selectedArcana.ArcanaId}");
                break;
        }

        // ChestReward 처리
        ChestReward chestReward = FindObjectOfType<ChestReward>();
        if (chestReward != null)
        {
            chestReward.getReward = true;
            Debug.Log("Reward acquired and getReward is set to true.");
        }
        else
        {
            Debug.LogWarning("No ChestReward found in the scene.");
        }

        // UI 닫기
        ToggleUIElement(ArcanaUIObj, ref isArcanaUIActive);
    }
}
