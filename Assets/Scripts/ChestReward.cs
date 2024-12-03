using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ChestReward : MonoBehaviour
{
    public Animator animator;
    public bool getReward;  //보상 획득 여부
    private void Awake() => animator = GetComponentInChildren<Animator>();
    [Header("아르카나 데이터")]
    public bool isOpen;   //상자 열렸는지 확인
    
    //아르카나 보상 
    public IEnumerator ArcanaResult()
    {
        animator.SetTrigger("Open");
        isOpen = true;
        // 애니메이션이 끝날 때까지 대기
        yield return new WaitUntil(() => IsAnimationFinished("Open"));
        //초기화 및 UI출력
        ResultManager.Instance.RandomArcana();
        //랜덤한 아르카나 카드 3개중에 하나 선택
        Debug.Log("아르나카 보상을 획득하였습니다.");
    }
    public void SelectArcana()
    {
        getReward = true;
        ResultManager.Instance.getReward = getReward;
    }
    // 애니메이션 상태를 체크하는 함수
    private bool IsAnimationFinished(string animationName)
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1f;
    }
}
