using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomEnterTrigger : MonoBehaviour
{
    public GameObject BossRoomCloseObj;
    public GameObject MazeObj;
    public bool isMazeOut;


    private void Awake()
    {
        isMazeOut = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isMazeOut = true;
            BossBattleCamReset();
        }
    }
    public void BossBattleCamReset()
    {
        //ī�޶� ����
        GameManager.Instance.playerCamera.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
        // ī�޶� ����: FOV ����
        GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 60f; // FOV ����

        var virtualCamera = GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>();
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Follow Offset ����
        transposer.m_FollowOffset = new Vector3(0f, 15f, -30f);
        //������ ���� �� Ż�� ���ϰ� �� ����
        BossRoomCloseObj.SetActive(true);
        //�̷� ��Ȱ��ȭ
        MazeObj.SetActive(false);
    }
}
