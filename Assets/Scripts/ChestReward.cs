using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ChestReward : MonoBehaviour
{
    // 아르카나 보상
    public ArcanaData[] arcanaDatas;  // ArcanaData 배열을 여기에 할당

    public Animator animator;
    public bool getReward;  //보상 획득 여부

    [Header("아르카나 데이터")]
    public bool isOpen;   //상자 열렸는지 확인

    [Header("유물 데이터")]
    public List<RelicData> commonRelic;
    public List<RelicData> unCommonRelic;
    public List<RelicData> rareRelic;

    public Transform relicSpwanPos;
    public GameObject relicObj;

    private float relicSpwanChance = 0.6f;
    private float common = 0.7f;
    private float unCommon = 0.2f;
    private float rare = 0.1f;

    private void Awake() => animator = GetComponentInChildren<Animator>();

    private void OnEnable()
    {
        getReward = false;
        isOpen = false;
    }


    //아르카나 보상 
    public IEnumerator ArcanaResult()
    {
        isOpen = true;
        animator.SetTrigger("Open");
        // 애니메이션이 끝날 때까지 대기
        yield return new WaitUntil(() => IsAnimationFinished("Open"));

        // 보상 획득 상태가 아니면 아르카나 보상 호출
        if (getReward == false)
        {
            RandomArcana();
            Debug.Log("아르카나 보상을 획득하였습니다.");
        }
        else
        {
            Debug.Log("이미 보상을 받았습니다. 나중에 다시 시도하세요.");
        }
    }

    // 애니메이션 상태를 체크하는 함수
    private bool IsAnimationFinished(string animationName)
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1f;
    }


    public void RandomArcana()
    {
        // 아르카나 랜덤 생성 로직
        ArcanaData[] arcanaArray = arcanaDatas;
        List<ArcanaData> selectedArcana = new List<ArcanaData>();

        if (arcanaArray.Length < 3)
        {
            Debug.LogWarning("Not enough Arcana data to select.");
            return;
        }

        // 중복되지 않게 아르카나 3개 선택
        while (selectedArcana.Count < 3)
        {
            int randomIndex = Random.Range(0, arcanaArray.Length);
            if (!selectedArcana.Contains(arcanaArray[randomIndex]))
            {
                selectedArcana.Add(arcanaArray[randomIndex]);
            }
        }

        // 선택된 아르카나 UI 업데이트
        DisplayArcana(selectedArcana);
    }

    private void DisplayArcana(List<ArcanaData> selectedArcana)
    {
        if (selectedArcana.Count < 3)
        {
            Debug.LogError("Insufficient Arcana data for UI display!");
            return;
        }

        UiManager.Instance.UpdateArcanaUI(selectedArcana);
        UiManager.Instance.ToggleUIElement(UiManager.Instance.ArcanaUIObj, ref UiManager.Instance.isArcanaUIActive);
    }


    //유물을 소환할때 이 함수 호출
    public void RelicSpwan()
    {
        //60%확률로 유물 스폰
        float relicSpwan = Random.value;
        if (relicSpwan <= relicSpwanChance)
        {
            //랜덤 유물 선택
            RelicData relicData = RandomRelic();
            //유물 랜덤으로 고르고 그 유물 스폰 및 데이터넣으면 유물오브젝트가 일정시간후 삭제되고 자동으로 플레이어스텟에 반영
            GameObject relic = Instantiate(relicObj);
            relic.transform.GetComponentInChildren<RelicObject>().relicData = relicData;
            relic.transform.GetComponentInChildren<RelicObject>().Spwan();
            relic.transform.position = relicSpwanPos.position;
            //유물매니저에 획득한 유물 추가
            RelicManager.Instance.relicList.Add(relicData);
        }
    }

    public RelicData RandomRelic()
    {
        while (true)
        {
            float randomValue = Random.value;
            //고를 렐릭 일단정의
            List<RelicData> randomRelic;
            //커먼등급 확률 60%
            if (randomValue <= common)
            {
                randomRelic = commonRelic;
            }
            //언커먼등급 확률 30%
            else if (randomValue <= common + unCommon)
            {
                randomRelic = unCommonRelic;
            }
            //레어등급 확률 10%
            else
            {
                randomRelic = rareRelic;
            }
            //고른등급중에 한번더 랜덤유물 선택
            int index = Random.Range(0, randomRelic.Count);
            RelicData selectRelic = randomRelic[index];
            //고른 유물을 선택한적있는지 체크
            bool exists = RelicManager.Instance.relicList.Contains(selectRelic);
            if(exists == false)
            {
                //고른적 없으면 반환
                return selectRelic;
            }
            //고른적 있으면 다시
        }
    }
}
