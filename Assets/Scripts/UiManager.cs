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
    public Image qCoolImg;  //쿨타임 회전 이미지
    public TextMeshProUGUI qCoolTimeText;   //쿨타임 텍스트
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


    //활성화 유무 확인 변수
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
    //쿨타임 
    public void UseQSkill()
    {
        float qCoolTime = SkillManager.Instance.qCoolTime; // Q 스킬의 쿨타임 (초 단위)
        StartCoolTime(qCoolImg, qCoolTimeText, qCoolTime);
        // Q 스킬 로직 추가
    }
    public void UseWSkill()
    {
        float wCoolTime = SkillManager.Instance.wCoolTime; // w 스킬의 쿨타임 (초 단위)
        StartCoolTime(wCoolImg, wCoolTimeText, wCoolTime);
    }
    public void UseESkill()
    {
        float eCoolTime = SkillManager.Instance.eCoolTime;  // e 스킬의 쿨타임 (초 단위)
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
            case 0: //일반공격 강화
                if(ArcanaManager.Instance.canEnhanceMeleeAttack == false)
                {
                    ArcanaManager.Instance.canEnhanceMeleeAttack = true;
                    Debug.Log("Melee Enhance Activated");
                }
                else
                {
                    //다시 선택 시 레벨 증가 및 데미지 합산
                    ArcanaManager.Instance.curEnhanceLevel++;
                    ArcanaManager.Instance.enhanceAtkDamage += selectedArcana.enhanceDamage[ArcanaManager.Instance.curEnhanceLevel];
                    Debug.Log($"Melee Enhance Level Up: {ArcanaManager.Instance.curEnhanceLevel}, ResultDamage: {ArcanaManager.Instance.enhanceAtkDamage}");
                }
                break;
            case 1: //랜덤 투사체 강화
                if (ArcanaManager.Instance.canRandomBulletInit == false)
                {
                    ArcanaManager.Instance.canRandomBulletInit = true;
                    Debug.Log("Random Bullet Init Activated");
                }
                else
                {
                    ArcanaManager.Instance.randomAtkLevel++;
                    ArcanaManager.Instance.randomAtkDamage += selectedArcana.enhanceDamage[ArcanaManager.Instance.randomAtkLevel];
                    Debug.Log($"Random Bullet Level Up: {ArcanaManager.Instance.randomAtkLevel}, ResultDamage: {ArcanaManager.Instance.randomAtkDamage}");
                }
                break;
            case 2: //일반공격 유도체로 변경
                if(ArcanaManager.Instance.canCatalyst == false)
                    ArcanaManager.Instance.ChanageCatalyst();
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
