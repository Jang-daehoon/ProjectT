using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool isFirstTutirial;    //Tutorial ���� ����
    public DialogSystem firstTutorialDialog;
    public DialogSystem secondTutorialDialog;
    public DialogSystem thirdTutorialDialog;

    // Ʃ�丮�� �� ��ȯ ����
    public GameObject[] InstantiateEnemyPrefabs; // ��ȯ�� �� ������ �迭
    public Transform[] SpawnPoints;             // �� ��ȯ ��ġ �迭
    public List<GameObject> ActiveEnemies = new List<GameObject>(); // Ȱ��ȭ�� �� ����Ʈ

    // ���� ������ ����
    public GameObject rewardItemPrefab;         // ���� ������ ������
    public Transform rewardSpawnPoint;         // ���� ������ ���� ��ġ

    //��Ż ���� ����
    public GameObject potalPrefab;  //��Ż ������
    public Transform potalSpawnPoint;   //��Ż ��ȯ ��ġ

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        if (isFirstTutirial == false)
        {
            isFirstTutirial = true;
            Time.timeScale = 0;
            //Ʃ�丮�� ��� ����
            yield return new WaitUntil(() => firstTutorialDialog.UpdateDialog());

            //���� ����
            Debug.Log("���� ���� �޼��� ����");

            secondTutorialDialog.gameObject.SetActive(true);
            yield return null;

            //Ʃ�丮�� ��� ����
            yield return new WaitUntil(() => secondTutorialDialog.UpdateDialog());
            Time.timeScale = 1;

            SpawnEnemies();
            Debug.Log("���� ��ȯ�߽��ϴ�.");

            //������ ��� ���� óġ
            yield return new WaitUntil(() => AreAllEnemiesDefeated());
            Debug.Log("��� ���� óġ�߽��ϴ�!");
            //óġ �Ϸ� �� ���� ������ ���� ->Ʃ�丮���̶� Ȯ�� ����
            Instantiate(rewardItemPrefab, rewardSpawnPoint.position, Quaternion.identity);
            Debug.Log("���� �������� �����Ǿ����ϴ�.");

            thirdTutorialDialog.gameObject.SetActive(true);
            yield return null;
            Time.timeScale = 0;
            //Ʃ�丮�� ��� ����
            yield return new WaitUntil(() => thirdTutorialDialog.UpdateDialog());
            Time.timeScale = 1;

            if(ResultManager.Instance.getReward == true)
            {
                //���� ������ ȹ�� �� ��Ż ����
                Instantiate(potalPrefab, potalSpawnPoint.position, Quaternion.identity);
                //Ʃ�丮�� ��� ����
                //��Ż ��ȣ�ۿ� �� Map UI Ȱ��ȭ
                //Ʃ�丮�� ��� ����

                //�� UI Ŭ�� �� �ش� ��ġ�� �̵� 


                //->Ʃ�丮�� ����

            }
        }
        else
            yield break;
    }
    // �� ��ȯ �޼���
    private void SpawnEnemies()
    {
        foreach (Transform spawnPoint in SpawnPoints)
        {
            int randomIndex = Random.Range(0, InstantiateEnemyPrefabs.Length);
            GameObject enemy = Instantiate(InstantiateEnemyPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
            ActiveEnemies.Add(enemy);
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
