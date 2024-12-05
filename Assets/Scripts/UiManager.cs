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
        firstArcana.onClick.AddListener(() => ArcanaSelect(0));
        secondArcana.onClick.AddListener(() => ArcanaSelect(1));
        thirdArcana.onClick.AddListener(() => ArcanaSelect(2));
    }

    //UI토글
    public void ToggleUIElement(GameObject uiElement, ref bool isActive)
    {
        isActive = !isActive;
        uiElement.SetActive(isActive);
    }

    // 아르카나 선택 시 동작 처리
    public void ArcanaSelect(int index)
    {
        // 선택된 아르카나의 인덱스를 받아서 처리
        if (ArcanaManager.Instance.ArcanaData.Length > index)
        {
            ArcanaData selectedArcana = ArcanaManager.Instance.ArcanaData[index];
            Debug.Log($"Selected Arcana: {selectedArcana.name}");

            // 선택된 아르카나에 대한 로직을 여기에 추가
            if (selectedArcana.ArcanaId == 0)
                ArcanaManager.Instance.canEnhanceMeleeAttack = true;
            if (selectedArcana.ArcanaId == 1)
                ArcanaManager.Instance.canRandomBulletInit = true;
        }
        //ChestReward를 가진 오브젝트를 찾아 
        //ChestReward컴포넌트 내부 변수의 getReward의 값을 true로 변경 
        // ChestReward를 가진 오브젝트를 찾아서 getReward를 true로 설정

        ChestReward chestReward = FindObjectOfType<ChestReward>(); // 게임 내에 있는 ChestReward 오브젝트를 찾음
        if (chestReward != null)
        {
            chestReward.getReward = true;  // getReward 값을 true로 설정
            Debug.Log("Reward acquired and getReward is set to true.");
        }
        else
        {
            Debug.LogWarning("No ChestReward found in the scene.");
        }
        ArcanaUIObj.SetActive(false);
    }
}
