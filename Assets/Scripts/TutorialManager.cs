using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool isFirstTutirial;    //Tutorial ���� ����

    public DialogSystem firstTutorialDialog;
    public DialogSystem secondTutorialDialog;
    public DialogSystem thirdTutorialDialog;
    public DialogSystem fourthTutorialDialog;

    // Ʃ�丮�� �� ��ȯ ����
    public GameObject[] InstantiateEnemyPrefabs; // ��ȯ�� �� ������ �迭
    public Transform[] SpawnPoints;             // �� ��ȯ ��ġ �迭
    public List<GameObject> ActiveEnemies = new List<GameObject>(); // Ȱ��ȭ�� �� ����Ʈ
    public ParticleSystem enemySpawnParticle;
    // ���� ������ ����
    public GameObject rewardItemPrefab;         // ���� ������ ������
    public Transform rewardSpawnPoint;         // ���� ������ ���� ��ġ

    //��Ż ���� ����
    public GameObject potalPrefab;  //��Ż ������
    public Transform potalSpawnPoint;   //��Ż ��ȯ ��ġ

    private void Awake()
    {
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        if (isFirstTutirial == false)
        {
            GameManager.Instance.player.canMove = false;
            //FadeOut
            UiManager.Instance.FadeObj.gameObject.SetActive(true);
            isFirstTutirial = true;
            UiManager.Instance.FadeObj.isFadeOut = true;
            yield return new WaitForSeconds(UiManager.Instance.FadeObj.duration);
            UiManager.Instance.FadeObj.gameObject.SetActive(false);

            //Ʃ�丮�� ��� ����
            yield return new WaitUntil(() => firstTutorialDialog.UpdateDialog());
            //���� ����
            Debug.Log("���� ���� �޼��� ����");

            secondTutorialDialog.gameObject.SetActive(true);
            yield return null;
            //Ʃ�丮�� ��� ����
            yield return new WaitUntil(() => secondTutorialDialog.UpdateDialog());
            SpawnEnemies();
            Debug.Log("���� ��ȯ�߽��ϴ�.");
            GameManager.Instance.player.canMove = true;

            //������ ��� ���� óġ
            yield return new WaitUntil(() => AreAllEnemiesDefeated());
            Debug.Log("��� ���� óġ�߽��ϴ�!");
            //óġ �Ϸ� �� ���� ������ ���� ->Ʃ�丮���̶� Ȯ�� ����
            GameObject rewardBox = Instantiate(rewardItemPrefab, rewardSpawnPoint.position, Quaternion.identity);

            Debug.Log("���� �������� �����Ǿ����ϴ�.");
            thirdTutorialDialog.gameObject.SetActive(true);
            yield return null;
            GameManager.Instance.player.canMove = false;
            //Ʃ�丮�� ��� ����
            yield return new WaitUntil(() => thirdTutorialDialog.UpdateDialog());
            GameManager.Instance.player.canMove = true;
            //������ ������Ʈ�� ChestReward�� getReward�� true�� �ɶ����� ���
            yield return new WaitUntil(() => rewardBox.GetComponent<ChestReward>().getReward == true);
            
            //���� ������ ȹ�� �� ��Ż ����
           Instantiate(potalPrefab, potalSpawnPoint.position, Quaternion.identity);

            fourthTutorialDialog.gameObject.SetActive(true);
            yield return null;
            GameManager.Instance.player.canMove = false;
            //Ʃ�丮�� ��� ����
            yield return new WaitUntil(() => fourthTutorialDialog.UpdateDialog());
            GameManager.Instance.player.canMove = true;
            //��Ż ��ȣ�ۿ� �� Map UI Ȱ��ȭ
            yield return new WaitUntil(() => UiManager.Instance.isMapUIActive == true);
            //Ʃ�丮�� ��� ����
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
            ParticleSystem spawnParticle = Instantiate(enemySpawnParticle, enemy.transform.position, Quaternion.identity);
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
