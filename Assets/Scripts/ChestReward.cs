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

    public ParticleSystem chestParticle;
    public ParticleSystem chestOpenParticle;

    private void Awake() => animator = GetComponentInChildren<Animator>();

    private void OnEnable()
    {
        arcanaDatas = ArcanaManager.Instance.ArcanaData;
        getReward = false;
        isOpen = false;
    }


    //�Ƹ�ī�� ���� 
    public IEnumerator ArcanaResult()
    {
        isOpen = true;
        chestParticle.Stop();
        chestOpenParticle.Play();
        UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
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
}
