using Cinemachine;
using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : Singleton<RoomManager>
{
    //�� ��ȯ ����
    public MapGenerator _mapGenerator;      
    private void Awake()
    {
        // �� ���� && �� ������ �Ѱ��ֱ�
        _mapGenerator = FindObjectOfType<MapGenerator>();
        GameManager.Game.SetMapArray(_mapGenerator.GenerateMap());
        GameManager.Game.StartMap();
    }
    public void EnterRoom(ERoomType roomType)
    {
        // �÷��̾� �ʱ�ȭ
        // UI��� �����ְ� (��, ����)
        // ��ȭâ �����ְ�
        // �ʱ�ȭ
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
    // �Ϲ� �� �濡 �� ��
    public void OnEnterEnemyRoom()
    {
        Debug.Log("EnterEnemyRoom");
        // Stage2, Stage3, Stage4 �� ������ ���� �����Ͽ� �̵� Stage4�������� ����
        int randomIndex = Random.Range(0, 3); // 0���� 2������ ������ ����
        string selectedStage = ScenesManager.Instance.Stage[randomIndex];

        // ���õ� ������ �̵�
        ScenesManager.Instance.ChanageScene(selectedStage);
        UiManager.Instance.ToggleUIElement(UiManager.Instance.MapUIObj, ref UiManager.Instance.isMapUIActive);

        // �ش� ���� �ִ� �÷��̾� ���� ��ǥ�� ã�� Player�� �̵���Ų��.
        StartCoroutine(MovePlayerToStartPosition(selectedStage));
        //ī�޶� ����
        GameManager.Instance.playerCamera.transform.rotation = Quaternion.Euler(40f, 0f, 0f);
        // ī�޶� ����: FOV ����
        GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 40f; // FOV ����

        var virtualCamera = GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>();
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Follow Offset ����
        transposer.m_FollowOffset = new Vector3(0f, 15f, -15f);
    }
    // ����Ʈ �濡 �� ��
    public void OnEnterEliteRoom()
    {

        Debug.Log("OnEnterEliteRoom");
        Debug.Log("Scene Movement");

        LoadingSceneController.LoadScene("EliteScene");
        UiManager.Instance.ToggleUIElement(UiManager.Instance.MapUIObj, ref UiManager.Instance.isMapUIActive);
        // �ش� ���� �ִ� �÷��̾� ���� ��ǥ�� ã�� Player�� �̵���Ų��.
        StartCoroutine(MovePlayerToStartPosition("EliteScene"));
        //ī�޶� ����
        GameManager.Instance.playerCamera.transform.rotation = Quaternion.Euler(20f, 0f, 0f);
        // ī�޶� ����: FOV ����
        GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 60f; // FOV ����

        var virtualCamera = GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>();
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Follow Offset ����
        transposer.m_FollowOffset = new Vector3(0f, 15f, -30f);
    }
    // ���� �濡 �� ��
    public void OnEnterMerchantRoom()
    {
        Debug.Log("OnEnterMerchantRoom");
    }
    // �޽� �濡 �� ��
    private void OnEnterRestRoom()
    {
        Debug.Log("OnEnterRestRoom");
        ScenesManager.Instance.ChanageScene("RestStage");
        UiManager.Instance.ToggleUIElement(UiManager.Instance.MapUIObj, ref UiManager.Instance.isMapUIActive);

        // �ش� ���� �ִ� �÷��̾� ���� ��ǥ�� ã�� Player�� �̵���Ų��.
        StartCoroutine(MovePlayerToStartPosition("RestStage"));
        //ī�޶� ����
        GameManager.Instance.playerCamera.transform.rotation = Quaternion.Euler(40f, 0f, 0f);
        // ī�޶� ����: FOV ����
        GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 40f; // FOV ����

        var virtualCamera = GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>();
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Follow Offset ����
        transposer.m_FollowOffset = new Vector3(0f, 15f, -15f);
    }
    // ���� �濡 �� ��
    private void OnEnterTreasureRoom()
    {
        Debug.Log("OnEnterTreasureRoom");
        ScenesManager.Instance.ChanageScene("RewardScene");
        UiManager.Instance.ToggleUIElement(UiManager.Instance.MapUIObj, ref UiManager.Instance.isMapUIActive);

        // �ش� ���� �ִ� �÷��̾� ���� ��ǥ�� ã�� Player�� �̵���Ų��.
        StartCoroutine(MovePlayerToStartPosition("RewardScene"));
        //ī�޶� ����
        GameManager.Instance.playerCamera.transform.rotation = Quaternion.Euler(40f, 0f, 0f);
        // ī�޶� ����: FOV ����
        GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 40f; // FOV ����

        var virtualCamera = GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>();
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Follow Offset ����
        transposer.m_FollowOffset = new Vector3(0f, 15f, -15f);

    }
    // ���� �濡 �� ��
    public void OnEnterBossRoom()
    {
        Debug.Log("OnEnterBossRoom");
        Debug.Log("Scene Movement");

        LoadingSceneController.LoadScene("BossScene");
        UiManager.Instance.ToggleUIElement(UiManager.Instance.MapUIObj, ref UiManager.Instance.isMapUIActive);
        // �ش� ���� �ִ� �÷��̾� ���� ��ǥ�� ã�� Player�� �̵���Ų��.
        StartCoroutine(MovePlayerToStartPosition("BossScene"));
        //ī�޶� ����
        GameManager.Instance.playerCamera.transform.rotation = Quaternion.Euler(17f, 88f, 0f);
        // ī�޶� ����: FOV ����
        GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 30f; // FOV ����

        var virtualCamera = GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>();
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Follow Offset ����
        transposer.m_FollowOffset = new Vector3(-13f, 6.5f, 0f);
    }
    // ���� �濡 �� ��
    private void OnEnterUnknownRoom()
    {
        //������ UnknownRoom���� �̵��Ѵ�.
        Debug.Log("OnEnterUnknownRoom");
        ScenesManager.Instance.ChanageScene("RandomScene1");
        UiManager.Instance.ToggleUIElement(UiManager.Instance.MapUIObj, ref UiManager.Instance.isMapUIActive);

        // �ش� ���� �ִ� �÷��̾� ���� ��ǥ�� ã�� Player�� �̵���Ų��.
        StartCoroutine(MovePlayerToStartPosition("RandomScene1"));
        //ī�޶� ����
        GameManager.Instance.playerCamera.transform.rotation = Quaternion.Euler(40f, 0f, 0f);
        // ī�޶� ����: FOV ����
        GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 40f; // FOV ����

        var virtualCamera = GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>();
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Follow Offset ����
        transposer.m_FollowOffset = new Vector3(0f, 15f, -15f);

    }
    public void ClearRoom()
    {
        GameManager.Game.CurrentRoom.ClearRoom();
    }
    private IEnumerator MovePlayerToStartPosition(string selectedStage)
    {
        // ���� �ε��� ������ ���
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == selectedStage);

        // ���� �ε�� ��, Player�� ���� ��ġ�� ã�� ������ ���⿡ �߰�
        GameObject spawnPoint = GameObject.Find("PlayerSpawnPoint"); // PlayerSpawnPoint��� �̸��� ������Ʈ ã��

        if (spawnPoint != null)
        {
            // Player�� ��ġ�� ���� ��ġ�� ����
            GameManager.Instance.player.transform.position = spawnPoint.transform.position;
            Debug.Log("Player moved to spawn point: " + spawnPoint.transform.position);
        }
        else
        {
            Debug.LogWarning("Spawn point not found in " + selectedStage);
        }
    }
}
