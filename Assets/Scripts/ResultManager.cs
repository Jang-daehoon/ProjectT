using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : Singleton<ResultManager>
{
    public float RewardDropRate = 0.6f; // 보상 획득 확률
    public bool getReward;
    // 아르카나 보상
    public GameObject rewardBoxPrefab;
    public ArcanaData[] arcanaDatas;  // ArcanaData 배열을 여기에 할당

    public void RandomArcana()
    {
        // arcanaDatas 배열을 사용하여 아르카나 선택
        ArcanaData[] arcanaArray = arcanaDatas;

        // 중복 방지를 위해 선택된 아르카나를 저장할 리스트
        List<ArcanaData> selectedArcana = new List<ArcanaData>();

        if (arcanaArray == null || arcanaArray.Length == 0)
        {
            Debug.LogWarning("ArcanaData is empty or not initialized!");
            return;
        }

        // 랜덤으로 3개의 아르카나 선택
        int attempts = 0; // 최대 시도 횟수
        while (selectedArcana.Count < 3 && attempts < 10)  // 최대 10번의 시도 제한
        {
            int randomIndex = Random.Range(0, arcanaArray.Length);

            // 이미 선택된 아르카나인지 확인
            if (!selectedArcana.Contains(arcanaArray[randomIndex]))
            {
                selectedArcana.Add(arcanaArray[randomIndex]);
            }
            attempts++;
        }

        // 만약 3개의 아르카나가 선택되지 않았다면, 오류 메시지 출력
        if (selectedArcana.Count < 3)
        {
            Debug.LogWarning("Not enough unique Arcana to select.");
        }
        else
        {
            // 선택된 아르카나 디버그 출력
            Debug.Log("Selected Arcana:");
            foreach (ArcanaData arcana in selectedArcana)
            {
                Debug.Log($"Arcana Name: {arcana.name}, Description: {arcana.ArcanaDesc}");
            }

            // 선택된 아르카나를 보상 화면에 표시
            DisplayArcana(selectedArcana);
        }
    }

    private void DisplayArcana(List<ArcanaData> selectedArcana)
    {
        if (selectedArcana == null || selectedArcana.Count < 3)
        {
            Debug.LogError("Insufficient Arcana data for UI display!");
            return;
        }

        // UiManager를 통해 UI 업데이트
        UiManager.Instance.UpdateArcanaUI(selectedArcana);  // UI 업데이트 호출

        // UI 활성화
        UiManager.Instance.AracanaUiActive();
    }
}
