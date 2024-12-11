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

    public ParticleSystem chestParticle;
    public ParticleSystem chestOpenParticle;

    private void Awake() => animator = GetComponentInChildren<Animator>();

    private void OnEnable()
    {
        arcanaDatas = ArcanaManager.Instance.ArcanaData;
        getReward = false;
        isOpen = false;
    }


    //아르카나 보상 
    public IEnumerator ArcanaResult()
    {
        isOpen = true;
        chestParticle.Stop();
        chestOpenParticle.Play();
        UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
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
}
