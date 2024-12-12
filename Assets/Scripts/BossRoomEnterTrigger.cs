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
        //카메라 설정
        GameManager.Instance.playerCamera.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
        // 카메라 설정: FOV 설정
        GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 60f; // FOV 설정

        var virtualCamera = GameManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>();
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // Follow Offset 수정
        transposer.m_FollowOffset = new Vector3(0f, 15f, -30f);
        //보스방 진입 시 탈출 못하게 길 막기
        BossRoomCloseObj.SetActive(true);
        //미로 비활성화
        MazeObj.SetActive(false);
    }
}
