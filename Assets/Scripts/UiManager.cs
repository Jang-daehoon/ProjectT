using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private GameObject PlayerStatusUiObj;
    //아르카나 UI
    [SerializeField] private GameObject ArcanaUIObj;
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
    public TextMeshProUGUI thirdAracanaDesc;


    public bool isDialogUiActive;
    public bool isArcanaUIActive;

    private void Awake()
    {
        isDialogUiActive = false;
        isArcanaUIActive = false;

    }
    private void Update()
    {
        if(isDialogUiActive == true)
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
        thirdAracanaDesc.text = arcanaData[2].ArcanaDesc;

        // 버튼 클릭 이벤트 등록
        firstArcana.onClick.AddListener(() => ArcanaSelect(0));
        secondArcana.onClick.AddListener(() => ArcanaSelect(1));
        thirdArcana.onClick.AddListener(() => ArcanaSelect(2));
    }

    public void AracanaUiActive()
    {
        isArcanaUIActive = true;
        ArcanaUIObj.SetActive(true);    
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
            if(selectedArcana.EnhanceAttackCnt == 3)
                ArcanaManager.Instance.canEnhanceMeleeAttack = true;    
        }
        ArcanaUIObj.SetActive(false);
    }
}
