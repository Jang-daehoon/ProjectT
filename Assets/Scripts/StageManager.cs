using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public enum RoomCheck { NOMAL, ELITE, BOSS, UNKNOWN, REST, REWARD, SHOP}
    public RoomCheck roomCheck;
    [Header("현재 스테이지 정보")]
    public float RewardDropRate = 0.6f; // 보상 획득 확률
    public int spawnCount;  // 소환할 카운트
    public GameObject[] InstantiateEnemyPrefabs; // 소환할 적 프리팹 배열
    public Transform[] SpawnPoints;             // 적 소환 위치 배열
    public List<GameObject> ActiveEnemies = new List<GameObject>(); // 활성화된 적 리스트
    public ParticleSystem enemySpawnParticle;

    //랜덤 방 관련 
    [Header("랜덤방에 사용될 정보")]
    public bool unknownClear;

    //휴식 방 관련
    [Header("휴식방에 사용될 정보")]
    public bool restRoomClear;

    //EliteRoom 관련
    [Header("EliteRoom에 사용될 정보")]
    public bool eliteRoomClear;
    //Elite MonsterPrefab
    public GameObject eliteMonsterObj;
    public Transform eliteMonsterSpawnPos;
    //Elite Monster의 Minions
    public GameObject eliteMinionObj;
    public ParticleSystem eliteSpawnParticleStone;
    public ParticleSystem eliteSpawnParticleMud;

    //BossRoom 관련
    [Header("BossRoom 사용될 정보")]
    public GameObject BossObj;
    public GameObject BossSpawnPos;
    [Header("보스방 입장")]
    public DialogSystem BossRoomEnter;
    public BossRoomEnterTrigger bossEnterTrigger;
    [Header("보스 조우")]
    public DialogSystem meetBossDialog;
    [Header("보스 처치")]
    public DialogSystem excuteBossDialog;
    public ParticleSystem BossSpawnParticle;
    public bool isBossClear;

    //보상 방 관련
    public Transform[] rewardsPos;

    // 보상 아이템 관련
    public GameObject rewardItemPrefab;         // 보상 아이템 프리팹
    public Transform rewardSpawnPoint;          // 보상 아이템 생성 위치

    // 포탈 관련 변수
    public GameObject potalPrefab;  // 포탈 프리팹
    public Transform potalSpawnPoint;   // 포탈 소환 위치

    private List<int> usedSpawnIndexes = new List<int>();  // 사용된 소환 위치 인덱스를 저장하는 리스트

    private IEnumerator Start()
    {
        switch (roomCheck)
        {
            case RoomCheck.NOMAL:
                //Fadein Fadeout or Shader를 통한 맵 이동 연출을 실행후 몬스터가 소환되게 로직 추가 예정
                //FadeOut
                GameManager.Instance.player.canMove = false;
                UiManager.Instance.FadeObj.gameObject.SetActive(true);
                UiManager.Instance.FadeObj.isFadeOut = true;
                yield return new WaitForSeconds(UiManager.Instance.FadeObj.duration);
                UiManager.Instance.FadeObj.gameObject.SetActive(false);
                //FadeOut
                GameManager.Instance.player.canMove = true;
                yield return new WaitForSeconds(1f);
                SpawnEnemies();

                // 생성된 모든 몬스터 처치 여부 체크
                yield return new WaitUntil(() => AreAllEnemiesDefeated());
                Debug.Log("모든 적을 처치했습니다!");

                // 처치 완료 시 RewardDropRate 확률에 따라 보상 아이템 생성
                if (Random.Range(0f, 1f) <= RewardDropRate)  // RewardDropRate 확률로 보상 아이템 생성
                {
                    Instantiate(rewardItemPrefab, rewardSpawnPoint.position, Quaternion.identity);
                    Debug.Log("보상 아이템이 생성되었습니다.");
                }
                else
                {
                    Debug.Log("보상이 생성되지 않았습니다.");
                }

                RoomManager.Instance.ClearRoom();

                Instantiate(potalPrefab, potalSpawnPoint.position, Quaternion.identity);
                break;
            case RoomCheck.ELITE:
                //Fadein Fadeout or Shader를 통한 맵 이동 연출을 실행후 몬스터가 소환되게 로직 추가 예정
                //FadeOut
                GameManager.Instance.player.canMove = false;
                UiManager.Instance.FadeObj.gameObject.SetActive(true);
                UiManager.Instance.FadeObj.isFadeOut = true;
                yield return new WaitForSeconds(UiManager.Instance.FadeObj.duration);
                UiManager.Instance.FadeObj.gameObject.SetActive(false);
                //FadeOut
                GameManager.Instance.player.canMove = true;
                yield return new WaitForSeconds(1f);
                SpawnEnemies();

                // 생성된 모든 몬스터 처치 여부 체크
                yield return new WaitUntil(() => AreAllEnemiesDefeated());
                Debug.Log("모든 적을 처치했습니다!");

                //Elite 몬스터 소환
                SpawnElite();
                yield return new WaitUntil(() => AreAllEnemiesDefeated());
                Debug.Log("Elite몬스터를 처치했습니다.");
                eliteRoomClear = true;

                RoomManager.Instance.ClearRoom();
                Instantiate(rewardItemPrefab, rewardSpawnPoint.position, Quaternion.identity);
                Debug.Log("보상 아이템이 생성되었습니다.");
                Instantiate(potalPrefab, potalSpawnPoint.position, Quaternion.identity);

                break;
            case RoomCheck.BOSS:
                //Fadein Fadeout or Shader를 통한 맵 이동 연출을 실행후 몬스터가 소환되게 로직 추가 예정
                //FadeOut
                GameManager.Instance.player.canMove = false;
                UiManager.Instance.FadeObj.gameObject.SetActive(true);
                UiManager.Instance.FadeObj.isFadeOut = true;
                yield return new WaitForSeconds(UiManager.Instance.FadeObj.duration);
                UiManager.Instance.FadeObj.gameObject.SetActive(false);
                //FadeOut
                GameManager.Instance.player.canMove = true;
                yield return null;
                //보스방 진입
                yield return new WaitUntil(() => BossRoomEnter.UpdateDialog());

                var bosstriggerEnter = FindAnyObjectByType<BossRoomEnterTrigger>();
                bosstriggerEnter.MazeOutCollider.gameObject.SetActive(true);
                //미로 탈출
                yield return new WaitUntil(() => bosstriggerEnter.isMazeOut == true);
                bosstriggerEnter.MazeOutCollider.enabled = false;
                meetBossDialog.gameObject.SetActive(true);
                //보스 소환 전 간단한 독백 및 보스 소환 징조 대화 스크립트 
                yield return new WaitUntil(() => meetBossDialog.UpdateDialog());
                //보스 소환
                SpawnBoss();
                bosstriggerEnter.BossHp.SetActive(true);
                //보스 처치까지 대기
                yield return new WaitUntil(() => AreAllEnemiesDefeated());

                excuteBossDialog.gameObject.SetActive(true);
                //보스 처치 후 대사 출력
                yield return new WaitUntil(() => excuteBossDialog.UpdateDialog());

                //fadein
                UiManager.Instance.FadeObj.gameObject.SetActive(true);
                UiManager.Instance.FadeObj.isFadeIn = true;

                UiManager.Instance.PlayerStatusUiObj.SetActive(false);
                //로딩씬 Thank you for playing Demo Scene 이동 (Exit버튼이 하나 있다.)
                LoadingSceneController.LoadScene("EndingScene");
                break;
            case RoomCheck.UNKNOWN:
                //Fadein Fadeout or Shader를 통한 맵 이동 연출을 실행후 몬스터가 소환되게 로직 추가 예정
                //FadeOut
                GameManager.Instance.player.canMove = false;
                UiManager.Instance.FadeObj.gameObject.SetActive(true);
                UiManager.Instance.FadeObj.isFadeOut = true;
                yield return new WaitForSeconds(UiManager.Instance.FadeObj.duration);
                UiManager.Instance.FadeObj.gameObject.SetActive(false);
                
                //FadeOut
                GameManager.Instance.player.canMove = true;
                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(() => unknownClear == true);
                RoomManager.Instance.ClearRoom();

                Instantiate(potalPrefab, potalSpawnPoint.position, Quaternion.identity);
                break;
            case RoomCheck.REST:
                GameManager.Instance.player.canMove = false;
                UiManager.Instance.FadeObj.gameObject.SetActive(true);
                UiManager.Instance.FadeObj.isFadeOut = true;
                yield return new WaitForSeconds(UiManager.Instance.FadeObj.duration);
                UiManager.Instance.FadeObj.gameObject.SetActive(false);

                //FadeOut
                GameManager.Instance.player.canMove = true;
                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(() => restRoomClear == true);
                RoomManager.Instance.ClearRoom();
                UiManager.Instance.ToggleUIElement(UiManager.Instance.MapUIObj, ref UiManager.Instance.isMapUIActive);
                break;

            case RoomCheck.REWARD:
                GameManager.Instance.player.canMove = false;
                UiManager.Instance.FadeObj.gameObject.SetActive(true);
                UiManager.Instance.FadeObj.isFadeOut = true;
                yield return new WaitForSeconds(UiManager.Instance.FadeObj.duration);
                UiManager.Instance.FadeObj.gameObject.SetActive(false);
                //FadeOut
                GameManager.Instance.player.canMove = true;
                yield return new WaitForSeconds(1f);

                // 랜덤한 위치에 보상 아이템 소환
                if (rewardsPos.Length > 0)
                {
                    int randomIndex = Random.Range(0, rewardsPos.Length); // rewardsPos에서 랜덤한 인덱스 선택
                    Transform randomPos = rewardsPos[randomIndex]; // 선택된 위치
                    Instantiate(rewardItemPrefab, randomPos.position, Quaternion.identity); // 보상 소환
                    Debug.Log($"보상이 위치 {randomIndex}에 소환되었습니다.");
                }
                else
                {
                    Debug.LogWarning("rewardsPos 배열이 비어있습니다. 보상을 소환할 수 없습니다.");
                }
                RoomManager.Instance.ClearRoom();
                Instantiate(potalPrefab, potalSpawnPoint.position, Quaternion.identity);
                break;

            //case RoomCheck.SHOP:
            //    break;
        }
        
    }

    // 적 소환 메서드
    private void SpawnEnemies()
    {
        int enemiesToSpawn = Mathf.Min(spawnCount, SpawnPoints.Length);  // 소환할 몬스터 수를 spawnCount와 SpawnPoints의 수로 제한

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // 사용되지 않은 소환 위치를 찾기 위해 랜덤하게 선택
            int spawnIndex = GetRandomUnusedSpawnPoint();
            if (spawnIndex == -1)
                break;  // 더 이상 사용할 수 있는 위치가 없으면 종료

            int randomIndex = Random.Range(0, InstantiateEnemyPrefabs.Length);  // 랜덤으로 적 프리팹 선택
            GameObject enemy = Instantiate(InstantiateEnemyPrefabs[randomIndex], SpawnPoints[spawnIndex].position, Quaternion.identity);
            ParticleSystem spawnParticle = Instantiate(enemySpawnParticle, enemy.transform.position, Quaternion.identity);
            ActiveEnemies.Add(enemy);  // 활성화된 적 리스트에 추가
            usedSpawnIndexes.Add(spawnIndex);  // 사용된 위치 인덱스 기록
        }
    }

    // 사용되지 않은 소환 위치를 랜덤하게 선택하는 메서드
    private int GetRandomUnusedSpawnPoint()
    {
        List<int> availableIndexes = new List<int>();

        // 사용되지 않은 소환 위치 인덱스를 찾는다
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            if (!usedSpawnIndexes.Contains(i))
            {
                availableIndexes.Add(i);
            }
        }

        // 사용되지 않은 위치가 있으면 랜덤으로 선택하고, 없으면 -1 반환
        if (availableIndexes.Count > 0)
        {
            int randomIndex = Random.Range(0, availableIndexes.Count);
            return availableIndexes[randomIndex];
        }
        else
        {
            return -1;  // 사용할 수 있는 위치가 없으면 -1 반환
        }
    }

    // 모든 적 처치 여부 확인
    private bool AreAllEnemiesDefeated()
    {
        // ActiveEnemies 리스트에서 null(처치된 적) 제거
        ActiveEnemies.RemoveAll(enemy => enemy == null);
        return ActiveEnemies.Count == 0;
    }

    private void SpawnElite()
    {
        // Elite 몬스터 소환
        if (eliteMonsterObj != null && eliteMonsterSpawnPos != null)
        {
            GameObject eliteMonster = Instantiate(eliteMonsterObj, eliteMonsterSpawnPos.position, Quaternion.identity);

            ParticleSystem eliteSpawnParticle = Instantiate(eliteSpawnParticleStone, eliteMonster.transform.position, Quaternion.identity);
            ParticleSystem eliteSpawnParticle2 = Instantiate(eliteSpawnParticleMud, eliteMonster.transform.position, Quaternion.identity);

            ActiveEnemies.Add(eliteMonster); // Elite 몬스터를 활성화된 적 리스트에 추가
            Debug.Log("Elite 몬스터가 소환되었습니다.");
        }
        else
        {
            Debug.LogWarning("Elite 몬스터 프리팹 또는 소환 위치가 설정되지 않았습니다.");
        }

        // Minion 소환 (랜덤 위치)
        int minionCount = 3; // Minion 소환 개수
        for (int i = 0; i < minionCount; i++)
        {
            int spawnIndex = GetRandomUnusedSpawnPoint();
            if (spawnIndex == -1)
            {
                Debug.LogWarning("사용 가능한 소환 위치가 부족합니다.");
                break;
            }

            GameObject minion = Instantiate(eliteMinionObj, SpawnPoints[spawnIndex].position, Quaternion.identity);
            ActiveEnemies.Add(minion); // Minion을 활성화된 적 리스트에 추가
            usedSpawnIndexes.Add(spawnIndex); // 사용된 위치 기록
            Debug.Log($"Minion {i + 1}이(가) 소환되었습니다.");
        }
    }

    public void SpawnBoss()
    {
        // Boss 몬스터 소환
        if (BossObj != null && BossSpawnPos != null)
        {
            GameObject bossMonster = Instantiate(BossObj, BossSpawnPos.transform.position, Quaternion.identity);

            //보스 소환 파티클 추가


            ActiveEnemies.Add(bossMonster); // Elite 몬스터를 활성화된 적 리스트에 추가
            Debug.Log("Elite 몬스터가 소환되었습니다.");
            var bosstriggerEnter = FindAnyObjectByType<BossRoomEnterTrigger>();
            bosstriggerEnter.isBossSpawn = true;    
        }
        else
        {
            Debug.LogWarning("Elite 몬스터 프리팹 또는 소환 위치가 설정되지 않았습니다.");
        }
    }

}
