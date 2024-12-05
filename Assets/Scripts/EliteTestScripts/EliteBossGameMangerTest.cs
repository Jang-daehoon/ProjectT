using Cinemachine;
using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteBossGameMangerTest : Singleton<EliteBossGameMangerTest>
{
    public GameObject player;

    private bool _isInit = false;

    private void Awake()
    {
        // 씬 전환 전에 플레이어와 카메라를 씬 전환 후에도 유지되도록 설정
        DontDestroyOnLoad(player.gameObject);

        Init();
    }
    public void Init()
    {

        if (!_isInit)
        {
            _isInit = true;
        }
    }
}
