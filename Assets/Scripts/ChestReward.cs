using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ChestReward : MonoBehaviour
{
    // �Ƹ�ī�� ����
    public ArcanaData[] arcanaDatas;  // ArcanaData �迭�� ���⿡ �Ҵ�

    public Animator animator;
    public bool getReward;  //���� ȹ�� ����

    [Header("�Ƹ�ī�� ������")]
    public bool isOpen;   //���� ���ȴ��� Ȯ��

    [Header("���� ������")]
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


    //�Ƹ�ī�� ���� 
    public IEnumerator ArcanaResult()
    {
        isOpen = true;
        animator.SetTrigger("Open");
        // �ִϸ��̼��� ���� ������ ���
        yield return new WaitUntil(() => IsAnimationFinished("Open"));

        // ���� ȹ�� ���°� �ƴϸ� �Ƹ�ī�� ���� ȣ��
        if (getReward == false)
        {
            RandomArcana();
            Debug.Log("�Ƹ�ī�� ������ ȹ���Ͽ����ϴ�.");
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


    public void RandomArcana()
    {
        // �Ƹ�ī�� ���� ���� ����
        ArcanaData[] arcanaArray = arcanaDatas;
        List<ArcanaData> selectedArcana = new List<ArcanaData>();

        if (arcanaArray.Length < 3)
        {
            Debug.LogWarning("Not enough Arcana data to select.");
            return;
        }

        // �ߺ����� �ʰ� �Ƹ�ī�� 3�� ����
        while (selectedArcana.Count < 3)
        {
            int randomIndex = Random.Range(0, arcanaArray.Length);
            if (!selectedArcana.Contains(arcanaArray[randomIndex]))
            {
                selectedArcana.Add(arcanaArray[randomIndex]);
            }
        }

        // ���õ� �Ƹ�ī�� UI ������Ʈ
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


    //������ ��ȯ�Ҷ� �� �Լ� ȣ��
    public void RelicSpwan()
    {
        //60%Ȯ���� ���� ����
        float relicSpwan = Random.value;
        if (relicSpwan <= relicSpwanChance)
        {
            //���� ���� ����
            RelicData relicData = RandomRelic();
            //���� �������� ���� �� ���� ���� �� �����ͳ����� ����������Ʈ�� �����ð��� �����ǰ� �ڵ����� �÷��̾�ݿ� �ݿ�
            GameObject relic = Instantiate(relicObj);
            relic.transform.GetComponentInChildren<RelicObject>().relicData = relicData;
            relic.transform.GetComponentInChildren<RelicObject>().Spwan();
            relic.transform.position = relicSpwanPos.position;
            //�����Ŵ����� ȹ���� ���� �߰�
            RelicManager.Instance.relicList.Add(relicData);
        }
    }

    public RelicData RandomRelic()
    {
        while (true)
        {
            float randomValue = Random.value;
            //�� ���� �ϴ�����
            List<RelicData> randomRelic;
            //Ŀ�յ�� Ȯ�� 60%
            if (randomValue <= common)
            {
                randomRelic = commonRelic;
            }
            //��Ŀ�յ�� Ȯ�� 30%
            else if (randomValue <= common + unCommon)
            {
                randomRelic = unCommonRelic;
            }
            //������ Ȯ�� 10%
            else
            {
                randomRelic = rareRelic;
            }
            //������߿� �ѹ��� �������� ����
            int index = Random.Range(0, randomRelic.Count);
            RelicData selectRelic = randomRelic[index];
            //�� ������ ���������ִ��� üũ
            bool exists = RelicManager.Instance.relicList.Contains(selectRelic);
            if(exists == false)
            {
                //���� ������ ��ȯ
                return selectRelic;
            }
            //���� ������ �ٽ�
        }
    }
}
