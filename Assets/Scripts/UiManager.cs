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

    [Header("�÷��̾� ��ȣ�ۿ� ���� UI")]
    public GameObject interactiveObjUi;
    public TextMeshProUGUI interactiveText;


    public GameObject PlayerStatusUiObj;
    //�Ƹ�ī�� UI
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
    /// Arcana UI ������Ʈ
    /// </summary>
    /// <param name="arcanaData">���õ� 3���� �Ƹ�ī�� ������</param>
    public void UpdateArcanaUI(List<ArcanaData> arcanaData)
    {
        if (arcanaData.Count < 3)
        {
            Debug.LogError("Insufficient Arcana data provided to UpdateArcanaUI!");
            return;
        }

        // ���� ������ ����
        firstArcana.onClick.RemoveAllListeners();
        secondArcana.onClick.RemoveAllListeners();
        thirdArcana.onClick.RemoveAllListeners();

        // ù ��° �Ƹ�ī��
        firstArcanaImg.sprite = arcanaData[0].ArcanaImage;
        firstArcanaName.text = arcanaData[0].name;
        firstArcanaDesc.text = arcanaData[0].ArcanaDesc;

        // �� ��° �Ƹ�ī��
        secondArcanaImg.sprite = arcanaData[1].ArcanaImage;
        secondArcanaName.text = arcanaData[1].name;
        secondArcanaDesc.text = arcanaData[1].ArcanaDesc;

        // �� ��° �Ƹ�ī��
        thirdArcanaImg.sprite = arcanaData[2].ArcanaImage;
        thirdArcanaName.text = arcanaData[2].name;
        thirdArcanaDesc.text = arcanaData[2].ArcanaDesc;

        // ��ư Ŭ�� �̺�Ʈ ���
        firstArcana.onClick.AddListener(() => ArcanaSelect(0));
        secondArcana.onClick.AddListener(() => ArcanaSelect(1));
        thirdArcana.onClick.AddListener(() => ArcanaSelect(2));
    }

    //UI���
    public void ToggleUIElement(GameObject uiElement, ref bool isActive)
    {
        isActive = !isActive;
        uiElement.SetActive(isActive);
    }

    // �Ƹ�ī�� ���� �� ���� ó��
    public void ArcanaSelect(int index)
    {
        // ���õ� �Ƹ�ī���� �ε����� �޾Ƽ� ó��
        if (ArcanaManager.Instance.ArcanaData.Length > index)
        {
            ArcanaData selectedArcana = ArcanaManager.Instance.ArcanaData[index];
            Debug.Log($"Selected Arcana: {selectedArcana.name}");

            // ���õ� �Ƹ�ī���� ���� ������ ���⿡ �߰�
            if (selectedArcana.ArcanaId == 0)
                ArcanaManager.Instance.canEnhanceMeleeAttack = true;
            if (selectedArcana.ArcanaId == 1)
                ArcanaManager.Instance.canRandomBulletInit = true;
        }
        //ChestReward�� ���� ������Ʈ�� ã�� 
        //ChestReward������Ʈ ���� ������ getReward�� ���� true�� ���� 
        // ChestReward�� ���� ������Ʈ�� ã�Ƽ� getReward�� true�� ����

        ChestReward chestReward = FindObjectOfType<ChestReward>(); // ���� ���� �ִ� ChestReward ������Ʈ�� ã��
        if (chestReward != null)
        {
            chestReward.getReward = true;  // getReward ���� true�� ����
            Debug.Log("Reward acquired and getReward is set to true.");
        }
        else
        {
            Debug.LogWarning("No ChestReward found in the scene.");
        }
        ArcanaUIObj.SetActive(false);
    }
}
