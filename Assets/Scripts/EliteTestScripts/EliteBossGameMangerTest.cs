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
        // �� ��ȯ ���� �÷��̾�� ī�޶� �� ��ȯ �Ŀ��� �����ǵ��� ����
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
