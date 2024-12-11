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
    //FadeObj
    public UIFadeInOut FadeObj;
    //UnknownUI Obj
    public GameObject UnknownUIObj;
    public GameObject enterUnknownUi;
    public GameObject mainUnknownUi;

    public GameObject UnknownUIObj2;
    public GameObject enterUnknownUi2;
    public GameObject mainUnknownUi2;

    public GameObject UnknownUIObj3;
    public GameObject enterUnknownUi3;
    public GameObject mainUnknownUi3;

    [Header("PlayerSkillImage")]
    public Image qSkill;
    public Image qCoolImg;  //��Ÿ�� ȸ�� �̹���
    public TextMeshProUGUI qCoolTimeText;   //��Ÿ�� �ؽ�Ʈ
    public Image wSkill;
    public Image wCoolImg;
    public TextMeshProUGUI wCoolTimeText;
    public Image eSkill;
    public Image eCoolImg;
    public TextMeshProUGUI eCoolTimeText;

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
    public Image unknownMainImage;
    public TextMeshProUGUI unknownRoomContext;
    public Button unknownEventBtn1;
    public Button unknownEventBtn2;
    public Button unknownEventBtn3;

    [Header("UnknownUI2")]
    public TextMeshProUGUI unknownRoomName2;
    public Image unknownRoomImage2;
    public Image unknownMainImage2;
    public TextMeshProUGUI unknownRoomContext2;
    public Button unknownEventBtn2_1;
    public Button unknownEventBtn2_2;
    public Button unknownEventBtn2_3;

    [Header("UnknownUI3")]
    public TextMeshProUGUI unknownRoomName3;
    public Image unknownRoomImage3;
    public Image unknownMainImage3;
    public TextMeshProUGUI unknownRoomContext3;
    public Button unknownEventBtn3_1;
    public Button unknownEventBtn3_2;
    public Button unknownEventBtn3_3;


    //Ȱ��ȭ ���� Ȯ�� ����
    public bool isDialogUiActive;
    public bool isArcanaUIActive;
    public bool isMapUIActive;
    public bool isInteractiveUiActive;
    public bool isUnknownUiActive;
    public bool isUnknownUiActive2;
    public bool isUnknownUiActive3;

    private void Awake()
    {
        isInteractiveUiActive = false;
        isDialogUiActive = false;
        isArcanaUIActive = false;
        isMapUIActive = false;

        isUnknownUiActive = false;
        isUnknownUiActive2 = false;
        isUnknownUiActive3 = false;
    }
    private void Start()
    {
        unknownEventBtn1.onClick.AddListener(() => UnknownManager.Instance.GetRandomRelic());
        unknownEventBtn2.onClick.AddListener(() => UnknownManager.Instance.someoneIsWatchingMe());
        unknownEventBtn3.onClick.AddListener(() => UnknownManager.Instance.RunAway());

        unknownEventBtn2_1.onClick.AddListener(() => UnknownManager.Instance.GetRandomRelic());
        unknownEventBtn2_2.onClick.AddListener(() => UnknownManager.Instance.someoneIsWatchingMe());
        unknownEventBtn2_3.onClick.AddListener(() => UnknownManager.Instance.RunAway());

        unknownEventBtn3_1.onClick.AddListener(() => UnknownManager.Instance.GetRandomRelic());
        unknownEventBtn3_2.onClick.AddListener(() => UnknownManager.Instance.someoneIsWatchingMe());
        unknownEventBtn3_3.onClick.AddListener(() => UnknownManager.Instance.RunAway());
    }
    private void Update()
    {
        if (isDialogUiActive == true || isMapUIActive == true || isArcanaUIActive == true)
            PlayerStatusUiObj.SetActive(false);
        else
            PlayerStatusUiObj.SetActive(true);
    }
    //��Ÿ�� 
    public void UseQSkill()
    {
        float qCoolTime = SkillManager.Instance.qCoolTime; // Q ��ų�� ��Ÿ�� (�� ����)
        StartCoolTime(qCoolImg, qCoolTimeText, qCoolTime);
        // Q ��ų ���� �߰�
    }
    public void UseWSkill()
    {
        float wCoolTime = SkillManager.Instance.wCoolTime; // w ��ų�� ��Ÿ�� (�� ����)
        StartCoolTime(wCoolImg, wCoolTimeText, wCoolTime);
    }
    public void UseESkill()
    {
        float eCoolTime = SkillManager.Instance.eCoolTime;  // e ��ų�� ��Ÿ�� (�� ����)
        StartCoolTime(eCoolImg, eCoolTimeText, eCoolTime);
    }
    public void StartCoolTime(Image coolImage, TextMeshProUGUI coolTimeText, float coolTime)
    {
        StartCoroutine(CoolTimeCheck(coolImage, coolTimeText, coolTime));
    }

    public IEnumerator CoolTimeCheck(Image coolImage, TextMeshProUGUI coolTimeText, float coolTime)
    {
        float remainingTime = coolTime;
        coolImage.fillAmount = 1f;
        coolTimeText.gameObject.SetActive(true);

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            coolImage.fillAmount = remainingTime / coolTime;
            coolTimeText.text = Mathf.Ceil(remainingTime).ToString();
            yield return null;
        }

        coolImage.fillAmount = 0f;
        coolTimeText.gameObject.SetActive(false);
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
        firstArcanaName.text = arcanaData[0].ArcanaName;
        firstArcanaDesc.text = arcanaData[0].ArcanaDesc;

        // �� ��° �Ƹ�ī��
        secondArcanaImg.sprite = arcanaData[1].ArcanaImage;
        secondArcanaName.text = arcanaData[1].ArcanaName;
        secondArcanaDesc.text = arcanaData[1].ArcanaDesc;

        // �� ��° �Ƹ�ī��
        thirdArcanaImg.sprite = arcanaData[2].ArcanaImage;
        thirdArcanaName.text = arcanaData[2].ArcanaName;
        thirdArcanaDesc.text = arcanaData[2].ArcanaDesc;

        // ��ư Ŭ�� �̺�Ʈ ���
        firstArcana.onClick.AddListener(() => ArcanaSelect(arcanaData[0]));
        secondArcana.onClick.AddListener(() => ArcanaSelect(arcanaData[1]));
        thirdArcana.onClick.AddListener(() => ArcanaSelect(arcanaData[2]));
    }

    //UI���
    public void ToggleUIElement(GameObject uiElement, ref bool isActive)
    {
        isActive = !isActive;
        uiElement.SetActive(isActive);
    }

    /// <summary>
    /// ���õ� �Ƹ�ī�� ó��
    /// </summary>
    /// <param name="selectedArcana">���õ� �Ƹ�ī�� ������</param>
    public void ArcanaSelect(ArcanaData selectedArcana)
    {
        if (selectedArcana == null)
        {
            Debug.LogError("�߸��� �Ƹ�ī���� ���õǾ����ϴ�!");
            return;
        }

        Debug.Log($"���õ� �Ƹ�ī��: {selectedArcana.name}");

        // �Ƹ�ī�� ������ ���� ó��
        if (selectedArcana.arcanaInfo == ArcanaData.ArcanaInfo.AttackEnhancement)
        {
            Debug.Log($"���� ��ȭ �Ƹ�ī�� ����: {selectedArcana.name}");
            switch (selectedArcana.ArcanaId)
            {
                case 0: // �Ϲ� ���� ��ȭ
                    Debug.Log("�Ϲ� ���� ��ȭ ���� ����.");
                    if (ArcanaManager.Instance.canEnhanceMeleeAttack == false)
                    {
                        ArcanaManager.Instance.canEnhanceMeleeAttack = true;
                        Debug.Log("���� ���� ��ȭ Ȱ��ȭ��");
                    }
                    else
                    {
                        // �ٽ� ���� �� ���� ���� �� ������ �ջ�
                        ArcanaManager.Instance.curEnhanceLevel++;
                        ArcanaManager.Instance.enhanceAtkDamage += selectedArcana.enhanceDamage[ArcanaManager.Instance.curEnhanceLevel];
                        Debug.Log($"���� ���� ��ȭ ���� ��: {ArcanaManager.Instance.curEnhanceLevel}, ��� ������: {ArcanaManager.Instance.enhanceAtkDamage}");
                    }
                    break;
                case 1: // ���� ����ü ��ȭ
                    Debug.Log("���� ����ü ��ȭ ���� ����.");
                    if (ArcanaManager.Instance.canRandomBulletInit == false)
                    {
                        ArcanaManager.Instance.canRandomBulletInit = true;
                        Debug.Log("���� ����ü �ʱ�ȭ Ȱ��ȭ��");
                    }
                    else
                    {
                        ArcanaManager.Instance.randomAtkLevel++;
                        ArcanaManager.Instance.randomAtkDamage += selectedArcana.enhanceDamage[ArcanaManager.Instance.randomAtkLevel];
                        Debug.Log($"���� ����ü ���� ��: {ArcanaManager.Instance.randomAtkLevel}, ��� ������: {ArcanaManager.Instance.randomAtkDamage}");
                    }
                    break;
                case 2: // �Ϲ� ���� ����ü�� ����
                    if (ArcanaManager.Instance.canCatalyst == false)
                    {
                        ArcanaManager.Instance.ChanageCatalyst();
                        Debug.Log("����ü ���� Ȱ��ȭ��");
                    }
                    break;
                default:
                    Debug.LogWarning($"ó������ ���� �Ƹ�ī�� ID: {selectedArcana.ArcanaId}");
                    break;
            }
        }
        else if (selectedArcana.arcanaInfo == ArcanaData.ArcanaInfo.SkillEnhancement)
        {
            Debug.Log($"��ų ��ȭ �Ƹ�ī�� ����: {selectedArcana.name}");
            switch (selectedArcana.ArcanaId)
            {
                case 0:
                    ArcanaManager.Instance.qSkillLevel++;
                    // ��Ÿ�� ����, ����ü ���� ����, ������ ���� ����
                    SkillManager.Instance.qCoolTime -= selectedArcana.cooldownReduction[ArcanaManager.Instance.qSkillLevel];
                    SkillManager.Instance.arrowCount += selectedArcana.enhanceCount[ArcanaManager.Instance.qSkillLevel];
                    SkillManager.Instance.qDamageMultiplier = selectedArcana.enhanceDamageMultiplier[ArcanaManager.Instance.qSkillLevel];

                    Debug.Log($"Q ��ų ���� ��: {ArcanaManager.Instance.qSkillLevel}, ��Ÿ��: {SkillManager.Instance.qCoolTime}, ȭ�� ����: {SkillManager.Instance.arrowCount}, ������ ����: {SkillManager.Instance.qDamageMultiplier}");
                    break;
                case 1:
                    ArcanaManager.Instance.wSkillLevel++;
                    // ��Ÿ�� ����, ��ƼŬ ������ ����
                    SkillManager.Instance.wCoolTime -= selectedArcana.cooldownReduction[ArcanaManager.Instance.wSkillLevel];
                    ArcanaManager.Instance.skillSize = selectedArcana.enhanceSkillScale[ArcanaManager.Instance.wSkillLevel];

                    Debug.Log($"W ��ų ���� ��: {ArcanaManager.Instance.wSkillLevel}, ��Ÿ��: {SkillManager.Instance.wCoolTime}, ��ų ũ��: {ArcanaManager.Instance.skillSize}");
                    break;
                case 2:
                    ArcanaManager.Instance.eSkillLevel++;
                    // ��Ÿ�� ����, ������ ���� ����
                    SkillManager.Instance.eCoolTime -= selectedArcana.cooldownReduction[ArcanaManager.Instance.eSkillLevel];
                    SkillManager.Instance.eDamageMultiplier = selectedArcana.enhanceDamageMultiplier[ArcanaManager.Instance.eSkillLevel];

                    Debug.Log($"E ��ų ���� ��: {ArcanaManager.Instance.eSkillLevel}, ��Ÿ��: {SkillManager.Instance.eCoolTime}, ������ ����: {SkillManager.Instance.eDamageMultiplier}");
                    break;
            }
        }

        // ChestReward ó��
        ChestReward chestReward = FindObjectOfType<ChestReward>();
        if (chestReward != null)
        {
            chestReward.getReward = true;
            Debug.Log("������ ȹ��Ǿ��� getReward�� true�� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogWarning("������ ChestReward�� ã�� �� �����ϴ�.");
        }

        // UI �ݱ�
        ToggleUIElement(ArcanaUIObj, ref isArcanaUIActive);
    }

}
