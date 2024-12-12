using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class RelicBox : MonoBehaviour
{
    public ParticleSystem particle;
    public Animator animator;
    public bool getReward;  //보상 획득 여부

    public bool isOpen;   //상자 열렸는지 확인

    public Transform relicSpwanPos;
    public GameObject relicObj;

    private float relicSpwanChance = 1f;
    private float common = 0.7f;
    private float unCommon = 0.2f;
    private float rare = 0.1f;
    // Start is called before the first frame update
    private void Awake() => animator = GetComponentInChildren<Animator>();

    private void OnEnable()
    {
        getReward = false;
        isOpen = false;
        particle.gameObject.SetActive(true);
        particle.Play();
    }


    //아르카나 보상 
    public IEnumerator RelicResult()
    {
        isOpen = true;
        animator.SetTrigger("Open");
        // 애니메이션이 끝날 때까지 대기
        yield return new WaitUntil(() => IsAnimationFinished("Open"));
        particle.Stop();
        // 보상 획득 상태가 아니면 아르카나 보상 호출
        if (getReward == false)
        {
            RelicSpwan();
            Debug.Log("유물 보상을 획득하였습니다.");
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

    //유물을 소환할때 이 함수 호출
    public void RelicSpwan()
    {
        //60%확률로 유물 스폰
        float relicSpwan = Random.value;
        if (relicSpwan <= relicSpwanChance)
        {
            //랜덤 유물 선택
            RelicData relicData = RandomRelic();
            if (relicData == null)
            {
                Debug.Log("유물 다획득함");
                return;
            }
            //유물 랜덤으로 고르고 그 유물 스폰 및 데이터넣으면 유물오브젝트가 일정시간후 삭제되고 자동으로 플레이어스텟에 반영
            GameObject relic = Instantiate(relicObj, relicSpwanPos.position, Quaternion.identity);
            relic.transform.GetComponentInChildren<RelicObject>().relicData = relicData;
            relic.transform.GetComponentInChildren<RelicObject>().Spwan();
        }
        getReward = true;
    }

    public RelicData RandomRelic()
    {
        while (true)
        {
            List<RelicData> randomRelic = RandomRelicList();
            if (randomRelic == null)
            {
                return null;
            }
            //고른등급중에 한번더 랜덤유물 선택
            int index = Random.Range(0, randomRelic.Count);
            RelicData selectRelic = randomRelic[index];
            //유물매니저에 얻은유물 추가
            GameManager.Instance.relicList.Add(selectRelic);
            //얻은유물 리스트에서 제거
            if (randomRelic == GameManager.Instance.commonRelic)
            {
                GameManager.Instance.commonRelic.RemoveAt(index);
            }
            else if (randomRelic == GameManager.Instance.unCommonRelic)
            {
                GameManager.Instance.unCommonRelic.RemoveAt(index);
            }
            else if (randomRelic == GameManager.Instance.rareRelic)
            {
                GameManager.Instance.rareRelic.RemoveAt(index);
            }
            return selectRelic;
        }
    }
    private List<RelicData> RandomRelicList()
    {
        bool isnull = false;
        //고를 렐릭 일단정의
        List<RelicData> randomRelic = null;
        while (isnull == false)
        {
            float randomValue = Random.value;
            if (GameManager.Instance.commonRelic.Count == 0 && GameManager.Instance.unCommonRelic.Count == 0 && GameManager.Instance.rareRelic.Count == 0)
            {
                isnull = true;
                return null;
            }
            //커먼등급 확률 60%
            if (randomValue <= common && isnull != true)
            {
                if (GameManager.Instance.commonRelic.Count != 0)
                {
                    randomRelic = GameManager.Instance.commonRelic;
                }
                else
                {
                    randomRelic = null;
                }
            }
            //언커먼등급 확률 30%
            else if (randomValue <= common + unCommon && isnull != true)
            {
                if (GameManager.Instance.unCommonRelic.Count != 0)
                {
                    randomRelic = GameManager.Instance.unCommonRelic;
                }
                else
                {
                    randomRelic = null;
                }
            }
            //레어등급 확률 10%
            else
            {
                if (GameManager.Instance.rareRelic.Count != 0)
                {
                    randomRelic = GameManager.Instance.rareRelic;
                }
                else
                {
                    randomRelic = null;
                }
            }
            if (randomRelic != null)
            {
                return randomRelic;
            }
            else
            {
                randomRelic = null;
            }
        }
        return randomRelic;
    }
}
