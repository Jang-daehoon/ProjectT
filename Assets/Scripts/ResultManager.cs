using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : Singleton<ResultManager>
{
    public float RewardDropRate = 0.6f; // ���� ȹ�� Ȯ��
    public bool getReward;
    // �Ƹ�ī�� ����
    public GameObject rewardBoxPrefab;
    public ArcanaData[] arcanaDatas;  // ArcanaData �迭�� ���⿡ �Ҵ�

    public void RandomArcana()
    {
        // arcanaDatas �迭�� ����Ͽ� �Ƹ�ī�� ����
        ArcanaData[] arcanaArray = arcanaDatas;

        // �ߺ� ������ ���� ���õ� �Ƹ�ī���� ������ ����Ʈ
        List<ArcanaData> selectedArcana = new List<ArcanaData>();

        if (arcanaArray == null || arcanaArray.Length == 0)
        {
            Debug.LogWarning("ArcanaData is empty or not initialized!");
            return;
        }

        // �������� 3���� �Ƹ�ī�� ����
        int attempts = 0; // �ִ� �õ� Ƚ��
        while (selectedArcana.Count < 3 && attempts < 10)  // �ִ� 10���� �õ� ����
        {
            int randomIndex = Random.Range(0, arcanaArray.Length);

            // �̹� ���õ� �Ƹ�ī������ Ȯ��
            if (!selectedArcana.Contains(arcanaArray[randomIndex]))
            {
                selectedArcana.Add(arcanaArray[randomIndex]);
            }
            attempts++;
        }

        // ���� 3���� �Ƹ�ī���� ���õ��� �ʾҴٸ�, ���� �޽��� ���
        if (selectedArcana.Count < 3)
        {
            Debug.LogWarning("Not enough unique Arcana to select.");
        }
        else
        {
            // ���õ� �Ƹ�ī�� ����� ���
            Debug.Log("Selected Arcana:");
            foreach (ArcanaData arcana in selectedArcana)
            {
                Debug.Log($"Arcana Name: {arcana.name}, Description: {arcana.ArcanaDesc}");
            }

            // ���õ� �Ƹ�ī���� ���� ȭ�鿡 ǥ��
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

        // UiManager�� ���� UI ������Ʈ
        UiManager.Instance.UpdateArcanaUI(selectedArcana);  // UI ������Ʈ ȣ��

        // UI Ȱ��ȭ
        UiManager.Instance.AracanaUiActive();
    }
}
