using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : Singleton<RoomManager>
{


    public void EnterRoom(ERoomType roomType)
    {
        // 플레이어 초기화
        // UI뜬거 없애주고 (맵, 보상)
        // 대화창 없애주고
        // 초기화
        switch(roomType)
        {
            case ERoomType.Elite:
                OnEnterEliteRoom();
                break;
            case ERoomType.Enemy:
                OnEnterEnemyRoom();
                break;
            case ERoomType.Merchant:
                OnEnterMerchantRoom();
                break;
            case ERoomType.Rest:
                OnEnterRestRoom();
                break;
            case ERoomType.Treasure:
                OnEnterTreasureRoom();
                break;
            case ERoomType.Unknown:
                OnEnterUnknownRoom();
                break;


        }
    }
    // 일반 적 방에 들어갈 때
    public void OnEnterEnemyRoom()
    {
        Debug.Log("EnterEnemyRoom");
        // Stage2, Stage3, Stage4 중 랜덤한 씬을 선택하여 이동
        int randomIndex = Random.Range(1, 4); // 1부터 3까지의 랜덤한 숫자
        string selectedStage = ScenesManager.Instance.Stage[randomIndex];

        // 선택된 씬으로 이동
        ScenesManager.Instance.ChanageScene(selectedStage);
        UiManager.Instance.MapUIActive();

        // 해당 씬에 있는 플레이어 시작 좌표를 찾아 Player를 이동시킨다.
        StartCoroutine(MovePlayerToStartPosition(selectedStage));
    }
    // 엘리트 방에 들어갈 때
    public void OnEnterEliteRoom()
    {
        Debug.Log("OnEnterEliteRoom");
    }
    // 상인 방에 들어갈 때
    public void OnEnterMerchantRoom()
    {
        Debug.Log("OnEnterMerchantRoom");
    }
    // 휴식 방에 들어갈 때
    private void OnEnterRestRoom()
    {
        Debug.Log("OnEnterRestRoom");
    }
    // 보물 방에 들어갈 때
    private void OnEnterTreasureRoom()
    {
        Debug.Log("OnEnterTreasureRoom");
    }
    // 보스 방에 들어갈 때
    public void OnEnterBossRoom()
    {
        Debug.Log("OnEnterBossRoom");
    }
    // 랜덤 방에 들어갈 때
    private void OnEnterUnknownRoom()
    {
        Debug.Log("OnEnterUnknownRoom");
    }
    public void ClearRoom()
    {
        GameManager.Game.CurrentRoom.ClearRoom();

        //확률로 보상 생성

        // 보상획득 시 맵 UI 출력
    }
    private IEnumerator MovePlayerToStartPosition(string selectedStage)
    {
        // 씬이 로딩될 때까지 대기
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == selectedStage);

        // 씬이 로드된 후, Player의 시작 위치를 찾는 로직을 여기에 추가
        GameObject spawnPoint = GameObject.Find("PlayerSpawnPoint"); // PlayerSpawnPoint라는 이름의 오브젝트 찾기

        if (spawnPoint != null)
        {
            // Player의 위치를 시작 위치로 설정
            GameManager.Instance.player.transform.position = spawnPoint.transform.position;
            Debug.Log("Player moved to spawn point: " + spawnPoint.transform.position);
        }
        else
        {
            Debug.LogWarning("Spawn point not found in " + selectedStage);
        }
    }
}
