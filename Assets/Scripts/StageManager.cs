using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public enum RoomCheck { NOMAL, ELITE, BOSS, UNKNOWN, REST, REWARD, SHOP}
    public RoomCheck roomCheck;

    public float RewardDropRate = 0.6f; // 보상 획득 확률
    public int spawnCount;  // 소환할 카운트
    public GameObject[] InstantiateEnemyPrefabs; // 소환할 적 프리팹 배열
    public Transform[] SpawnPoints;             // 적 소환 위치 배열
    public List<GameObject> ActiveEnemies = new List<GameObject>(); // 활성화된 적 리스트

    //랜덤 방 관련 
    public bool unknownClear;

    //휴식 방 관련
    public bool restRoomClear;

    //EliteRoom 관련
    public bool eliteRoomClear;
    //EliteRoom

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
                break;
            case RoomCheck.BOSS:
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
}
