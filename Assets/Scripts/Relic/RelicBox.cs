using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class RelicBox : MonoBehaviour
{
    public ParticleSystem particle;
    public Animator animator;
    public bool getReward;  //���� ȹ�� ����

    public bool isOpen;   //���� ���ȴ��� Ȯ��

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


    //�Ƹ�ī�� ���� 
    public IEnumerator RelicResult()
    {
        isOpen = true;
        animator.SetTrigger("Open");
        // �ִϸ��̼��� ���� ������ ���
        yield return new WaitUntil(() => IsAnimationFinished("Open"));
        particle.Stop();
        // ���� ȹ�� ���°� �ƴϸ� �Ƹ�ī�� ���� ȣ��
        if (getReward == false)
        {
            RelicSpwan();
            Debug.Log("���� ������ ȹ���Ͽ����ϴ�.");
        }
        else
        {
            Debug.Log("�̹� ������ �޾ҽ��ϴ�. ���߿� �ٽ� �õ��ϼ���.");
        }
    }

    // �ִϸ��̼� ���¸� üũ�ϴ� �Լ�
    private bool IsAnimationFinished(string animationName)
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1f;
    }

    //������ ��ȯ�Ҷ� �� �Լ� ȣ��
    public void RelicSpwan()
    {
        //60%Ȯ���� ���� ����
        float relicSpwan = Random.value;
        if (relicSpwan <= relicSpwanChance)
        {
            //���� ���� ����
            RelicData relicData = RandomRelic();
            if (relicData == null)
            {
                Debug.Log("���� ��ȹ����");
                return;
            }
            //���� �������� ���� �� ���� ���� �� �����ͳ����� ����������Ʈ�� �����ð��� �����ǰ� �ڵ����� �÷��̾�ݿ� �ݿ�
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
            if (GameManager.Instance.commonRelic.Count == 0 && GameManager.Instance.unCommonRelic.Count == 0 && GameManager.Instance.rareRelic.Count == 0)
            {
                return null;
            }
            float randomValue = Random.value;
            //�� ���� �ϴ�����
            List<RelicData> randomRelic;
            //Ŀ�յ�� Ȯ�� 60%
            if (randomValue <= common)
            {
                if (GameManager.Instance.commonRelic.Count == 0)
                {
                    Debug.Log("Ŀ�յ�� ���� �ٸ���");
                    return null;
                }
                randomRelic = GameManager.Instance.commonRelic;
            }
            //��Ŀ�յ�� Ȯ�� 30%
            else if (randomValue <= common + unCommon)
            {
                if (GameManager.Instance.commonRelic.Count == 0)
                {
                    Debug.Log("��Ŀ�յ�� ���� �ٸ���");
                    return null;
                }
                randomRelic = GameManager.Instance.unCommonRelic;
            }
            //������ Ȯ�� 10%
            else
            {
                if (GameManager.Instance.rareRelic.Count == 0)
                {
                    Debug.Log("������ ���� �ٸ���");
                    return null;
                }
                randomRelic = GameManager.Instance.rareRelic;
            }
            //������߿� �ѹ��� �������� ����
            int index = Random.Range(0, randomRelic.Count);
            RelicData selectRelic = randomRelic[index];
            //�����Ŵ����� �������� �߰�
            GameManager.Instance.relicList.Add(selectRelic);
            //�������� ����Ʈ���� ����
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
}
