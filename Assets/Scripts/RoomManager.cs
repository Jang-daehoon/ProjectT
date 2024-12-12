using Cinemachine;
using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : Singleton<RoomManager>
{
    //맵 소환 관련
    public MapGenerator _mapGenerator;      
    private void Awake()
    {
        // 맵 생성 && 맵 데이터 넘겨주기
        _mapGenerator = FindObjectOfType<MapGenerator>();
        GameManager.Game.SetMapArray(_mapGenerator.GenerateMap());
        GameManager.Game.StartMap();
    }
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
            /*case ERoomType.Merchant:
                OnEnterMerchantRoom();
                break;
            */
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
        // Stage2, Stage3, Stage4 중 랜덤한 씬을 선택하여 이동 Stage4는제거할 예정
        int randomIndex = Random.Range(0, 3); // 0부터 2까지의 랜덤한 숫자
        string selectedStage = ScenesManager.Instance.Stage[randomIndex];

        // 선택된 씬으로 이동
        ScenesManager.Instance.ChanageScene(selectedStage);
        UiManager.Instance.ToggleUIElement(UiManager.Instance.MapUIObj, ref UiManager.Instance.isMapUIActive);

        // 해당 씬에 있는 플레이어 시작 좌표를 찾아 Player를 이동시킨다.
        StartCoroutine(MovePlayerToStartPosition(selectedStage));
        //카메라 설정
        GameManager.Instance.playerCamera.transform.rotation = Quaternion.Euler(40f, 0f, 0f);
        // 카메라 설정: FOV 설정
        GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 40f; // FOV 설정

        var virtualCamera = GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>();
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Follow Offset 수정
        transposer.m_FollowOffset = new Vector3(0f, 15f, -15f);
    }
    // 엘리트 방에 들어갈 때
    public void OnEnterEliteRoom()
    {

        Debug.Log("OnEnterEliteRoom");
        Debug.Log("Scene Movement");

        LoadingSceneController.LoadScene("EliteScene");
        UiManager.Instance.ToggleUIElement(UiManager.Instance.MapUIObj, ref UiManager.Instance.isMapUIActive);
        // 해당 씬에 있는 플레이어 시작 좌표를 찾아 Player를 이동시킨다.
        StartCoroutine(MovePlayerToStartPosition("EliteScene"));
        //카메라 설정
        GameManager.Instance.playerCamera.transform.rotation = Quaternion.Euler(20f, 0f, 0f);
        // 카메라 설정: FOV 설정
        GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 60f; // FOV 설정

        var virtualCamera = GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>();
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Follow Offset 수정
        transposer.m_FollowOffset = new Vector3(0f, 15f, -30f);
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
        ScenesManager.Instance.ChanageScene("RestStage");
        UiManager.Instance.ToggleUIElement(UiManager.Instance.MapUIObj, ref UiManager.Instance.isMapUIActive);

        // 해당 씬에 있는 플레이어 시작 좌표를 찾아 Player를 이동시킨다.
        StartCoroutine(MovePlayerToStartPosition("RestStage"));
        //카메라 설정
        GameManager.Instance.playerCamera.transform.rotation = Quaternion.Euler(40f, 0f, 0f);
        // 카메라 설정: FOV 설정
        GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 40f; // FOV 설정

        var virtualCamera = GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>();
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Follow Offset 수정
        transposer.m_FollowOffset = new Vector3(0f, 15f, -15f);
    }
    // 보물 방에 들어갈 때
    private void OnEnterTreasureRoom()
    {
        Debug.Log("OnEnterTreasureRoom");
        ScenesManager.Instance.ChanageScene("RewardScene");
        UiManager.Instance.ToggleUIElement(UiManager.Instance.MapUIObj, ref UiManager.Instance.isMapUIActive);

        // 해당 씬에 있는 플레이어 시작 좌표를 찾아 Player를 이동시킨다.
        StartCoroutine(MovePlayerToStartPosition("RewardScene"));
        //카메라 설정
        GameManager.Instance.playerCamera.transform.rotation = Quaternion.Euler(40f, 0f, 0f);
        // 카메라 설정: FOV 설정
        GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 40f; // FOV 설정

        var virtualCamera = GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>();
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Follow Offset 수정
        transposer.m_FollowOffset = new Vector3(0f, 15f, -15f);

    }
    // 보스 방에 들어갈 때
    public void OnEnterBossRoom()
    {
        Debug.Log("OnEnterBossRoom");
        Debug.Log("Scene Movement");

        LoadingSceneController.LoadScene("BossScene");
        UiManager.Instance.ToggleUIElement(UiManager.Instance.MapUIObj, ref UiManager.Instance.isMapUIActive);
        // 해당 씬에 있는 플레이어 시작 좌표를 찾아 Player를 이동시킨다.
        StartCoroutine(MovePlayerToStartPosition("BossScene"));
        //카메라 설정
        GameManager.Instance.playerCamera.transform.rotation = Quaternion.Euler(17f, 88f, 0f);
        // 카메라 설정: FOV 설정
        GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 30f; // FOV 설정

        var virtualCamera = GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>();
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Follow Offset 수정
        transposer.m_FollowOffset = new Vector3(-13f, 6.5f, 0f);
    }
    // 랜덤 방에 들어갈 때
    private void OnEnterUnknownRoom()
    {
        //랜덤한 UnknownRoom으로 이동한다.
        Debug.Log("OnEnterUnknownRoom");
        ScenesManager.Instance.ChanageScene("RandomScene1");
        UiManager.Instance.ToggleUIElement(UiManager.Instance.MapUIObj, ref UiManager.Instance.isMapUIActive);

        // 해당 씬에 있는 플레이어 시작 좌표를 찾아 Player를 이동시킨다.
        StartCoroutine(MovePlayerToStartPosition("RandomScene1"));
        //카메라 설정
        GameManager.Instance.playerCamera.transform.rotation = Quaternion.Euler(40f, 0f, 0f);
        // 카메라 설정: FOV 설정
        GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 40f; // FOV 설정

        var virtualCamera = GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>();
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Follow Offset 수정
        transposer.m_FollowOffset = new Vector3(0f, 15f, -15f);

    }
    public void ClearRoom()
    {
        GameManager.Game.CurrentRoom.ClearRoom();
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
