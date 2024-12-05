using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : Singleton<RoomManager>
{


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
    // �Ϲ� �� �濡 �� ��
    public void OnEnterEnemyRoom()
    {
        Debug.Log("EnterEnemyRoom");
        // Stage2, Stage3, Stage4 �� ������ ���� �����Ͽ� �̵�
        int randomIndex = Random.Range(1, 4); // 1���� 3������ ������ ����
        string selectedStage = ScenesManager.Instance.Stage[randomIndex];

        // ���õ� ������ �̵�
        ScenesManager.Instance.ChanageScene(selectedStage);
        UiManager.Instance.MapUIActive();

        // �ش� ���� �ִ� �÷��̾� ���� ��ǥ�� ã�� Player�� �̵���Ų��.
        StartCoroutine(MovePlayerToStartPosition(selectedStage));
    }
    // ����Ʈ �濡 �� ��
    public void OnEnterEliteRoom()
    {
        Debug.Log("OnEnterEliteRoom");
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
    }
    // ���� �濡 �� ��
    private void OnEnterTreasureRoom()
    {
        Debug.Log("OnEnterTreasureRoom");
    }
    // ���� �濡 �� ��
    public void OnEnterBossRoom()
    {
        Debug.Log("OnEnterBossRoom");
    }
    // ���� �濡 �� ��
    private void OnEnterUnknownRoom()
    {
        Debug.Log("OnEnterUnknownRoom");
    }
    public void ClearRoom()
    {
        GameManager.Game.CurrentRoom.ClearRoom();

        //Ȯ���� ���� ����

        // ����ȹ�� �� �� UI ���
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
