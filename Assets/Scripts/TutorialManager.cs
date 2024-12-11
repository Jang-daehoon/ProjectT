using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool isFirstTutirial;    //Tutorial 최초 시작

    public DialogSystem firstTutorialDialog;
    public DialogSystem secondTutorialDialog;
    public DialogSystem thirdTutorialDialog;
    public DialogSystem fourthTutorialDialog;

    // 튜토리얼 적 소환 관련
    public GameObject[] InstantiateEnemyPrefabs; // 소환할 적 프리팹 배열
    public Transform[] SpawnPoints;             // 적 소환 위치 배열
    public List<GameObject> ActiveEnemies = new List<GameObject>(); // 활성화된 적 리스트
    public ParticleSystem enemySpawnParticle;
    // 보상 아이템 관련
    public GameObject rewardItemPrefab;         // 보상 아이템 프리팹
    public Transform rewardSpawnPoint;         // 보상 아이템 생성 위치

    //포탈 관련 변수
    public GameObject potalPrefab;  //포탈 프리팹
    public Transform potalSpawnPoint;   //포탈 소환 위치

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

            //튜토리얼 대사 시작
            yield return new WaitUntil(() => firstTutorialDialog.UpdateDialog());
            //몬스터 생성
            Debug.Log("몬스터 생성 메서드 시작");

            secondTutorialDialog.gameObject.SetActive(true);
            yield return null;
            //튜토리얼 대사 시작
            yield return new WaitUntil(() => secondTutorialDialog.UpdateDialog());
            SpawnEnemies();
            Debug.Log("적을 소환했습니다.");
            GameManager.Instance.player.canMove = true;

            //생성된 모든 몬스터 처치
            yield return new WaitUntil(() => AreAllEnemiesDefeated());
            Debug.Log("모든 적을 처치했습니다!");
            //처치 완료 시 보상 아이템 생성 ->튜토리얼이라 확정 생성
            GameObject rewardBox = Instantiate(rewardItemPrefab, rewardSpawnPoint.position, Quaternion.identity);

            Debug.Log("보상 아이템이 생성되었습니다.");
            thirdTutorialDialog.gameObject.SetActive(true);
            yield return null;
            GameManager.Instance.player.canMove = false;
            //튜토리얼 대사 시작
            yield return new WaitUntil(() => thirdTutorialDialog.UpdateDialog());
            GameManager.Instance.player.canMove = true;
            //생성한 오브젝트의 ChestReward의 getReward가 true가 될때까지 대기
            yield return new WaitUntil(() => rewardBox.GetComponent<ChestReward>().getReward == true);
            
            //보상 아이템 획득 시 포탈 생성
           Instantiate(potalPrefab, potalSpawnPoint.position, Quaternion.identity);

            fourthTutorialDialog.gameObject.SetActive(true);
            yield return null;
            GameManager.Instance.player.canMove = false;
            //튜토리얼 대사 시작
            yield return new WaitUntil(() => fourthTutorialDialog.UpdateDialog());
            GameManager.Instance.player.canMove = true;
            //포탈 상호작용 시 Map UI 활성화
            yield return new WaitUntil(() => UiManager.Instance.isMapUIActive == true);
            //튜토리얼 대사 시작
        }
        else
            yield break;
    }
    // 적 소환 메서드
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

    // 모든 적 처치 여부 확인
    private bool AreAllEnemiesDefeated()
    {
        // ActiveEnemies 리스트에서 null(처치된 적) 제거
        ActiveEnemies.RemoveAll(enemy => enemy == null);
        return ActiveEnemies.Count == 0;
    }
}
