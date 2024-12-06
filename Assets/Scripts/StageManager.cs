using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public float RewardDropRate = 0.6f; // ���� ȹ�� Ȯ��
    public int spawnCount;  // ��ȯ�� ī��Ʈ
    public GameObject[] InstantiateEnemyPrefabs; // ��ȯ�� �� ������ �迭
    public Transform[] SpawnPoints;             // �� ��ȯ ��ġ �迭
    public List<GameObject> ActiveEnemies = new List<GameObject>(); // Ȱ��ȭ�� �� ����Ʈ

    // ���� ������ ����
    public GameObject rewardItemPrefab;         // ���� ������ ������
    public Transform rewardSpawnPoint;          // ���� ������ ���� ��ġ

    // ��Ż ���� ����
    public GameObject potalPrefab;  // ��Ż ������
    public Transform potalSpawnPoint;   // ��Ż ��ȯ ��ġ

    private List<int> usedSpawnIndexes = new List<int>();  // ���� ��ȯ ��ġ �ε����� �����ϴ� ����Ʈ

    private IEnumerator Start()
    {
        //Fadein Fadeout or Shader�� ���� �� �̵� ������ ������ ���Ͱ� ��ȯ�ǰ� ���� �߰� ����
        //FadeOut
        UiManager.Instance.FadeObj.gameObject.SetActive(true);
        UiManager.Instance.FadeObj.isFadeOut = true;
        yield return new WaitForSeconds(UiManager.Instance.FadeObj.duration);
        UiManager.Instance.FadeObj.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        SpawnEnemies();

        // ������ ��� ���� óġ ���� üũ
        yield return new WaitUntil(() => AreAllEnemiesDefeated());
        Debug.Log("��� ���� óġ�߽��ϴ�!");

        // óġ �Ϸ� �� RewardDropRate Ȯ���� ���� ���� ������ ����
        if (Random.Range(0f, 1f) <= RewardDropRate)  // RewardDropRate Ȯ���� ���� ������ ����
        {
            Instantiate(rewardItemPrefab, rewardSpawnPoint.position, Quaternion.identity);
            Debug.Log("���� �������� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.Log("������ �������� �ʾҽ��ϴ�.");
        }

        RoomManager.Instance.ClearRoom();

        Instantiate(potalPrefab, potalSpawnPoint.position, Quaternion.identity);

    }

    // �� ��ȯ �޼���
    private void SpawnEnemies()
    {
        int enemiesToSpawn = Mathf.Min(spawnCount, SpawnPoints.Length);  // ��ȯ�� ���� ���� spawnCount�� SpawnPoints�� ���� ����

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // ������ ���� ��ȯ ��ġ�� ã�� ���� �����ϰ� ����
            int spawnIndex = GetRandomUnusedSpawnPoint();
            if (spawnIndex == -1)
                break;  // �� �̻� ����� �� �ִ� ��ġ�� ������ ����

            int randomIndex = Random.Range(0, InstantiateEnemyPrefabs.Length);  // �������� �� ������ ����
            GameObject enemy = Instantiate(InstantiateEnemyPrefabs[randomIndex], SpawnPoints[spawnIndex].position, Quaternion.identity);
            ActiveEnemies.Add(enemy);  // Ȱ��ȭ�� �� ����Ʈ�� �߰�
            usedSpawnIndexes.Add(spawnIndex);  // ���� ��ġ �ε��� ���
        }
    }

    // ������ ���� ��ȯ ��ġ�� �����ϰ� �����ϴ� �޼���
    private int GetRandomUnusedSpawnPoint()
    {
        List<int> availableIndexes = new List<int>();

        // ������ ���� ��ȯ ��ġ �ε����� ã�´�
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            if (!usedSpawnIndexes.Contains(i))
            {
                availableIndexes.Add(i);
            }
        }

        // ������ ���� ��ġ�� ������ �������� �����ϰ�, ������ -1 ��ȯ
        if (availableIndexes.Count > 0)
        {
            int randomIndex = Random.Range(0, availableIndexes.Count);
            return availableIndexes[randomIndex];
        }
        else
        {
            return -1;  // ����� �� �ִ� ��ġ�� ������ -1 ��ȯ
        }
    }

    // ��� �� óġ ���� Ȯ��
    private bool AreAllEnemiesDefeated()
    {
        // ActiveEnemies ����Ʈ���� null(óġ�� ��) ����
        ActiveEnemies.RemoveAll(enemy => enemy == null);
        return ActiveEnemies.Count == 0;
    }
}
