using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ChestReward : MonoBehaviour
{
    public Animator animator;
    public bool getReward;  //���� ȹ�� ����
    private void Awake() => animator = GetComponentInChildren<Animator>();
    [Header("�Ƹ�ī�� ������")]
    public bool isOpen;   //���� ���ȴ��� Ȯ��
    
    //�Ƹ�ī�� ���� 
    public IEnumerator ArcanaResult()
    {
        animator.SetTrigger("Open");
        isOpen = true;
        // �ִϸ��̼��� ���� ������ ���
        yield return new WaitUntil(() => IsAnimationFinished("Open"));
        //�ʱ�ȭ �� UI���
        ResultManager.Instance.RandomArcana();
        //������ �Ƹ�ī�� ī�� 3���߿� �ϳ� ����
        Debug.Log("�Ƹ���ī ������ ȹ���Ͽ����ϴ�.");
    }
    public void SelectArcana()
    {
        getReward = true;
        ResultManager.Instance.getReward = getReward;
    }
    // �ִϸ��̼� ���¸� üũ�ϴ� �Լ�
    private bool IsAnimationFinished(string animationName)
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1f;
    }
}
