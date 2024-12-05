using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    public CinemachineVirtualCamera playerCamera;


    private GameManagerEx _game = new GameManagerEx();
    

    public static GameManagerEx Game => Instance._game;

    private bool _isInit = false;

    private void Awake()
    {
        // �� ��ȯ ���� �÷��̾�� ī�޶� �� ��ȯ �Ŀ��� �����ǵ��� ����
        DontDestroyOnLoad(player.gameObject);
        DontDestroyOnLoad(playerCamera.gameObject);

        Init();
    }
    public void Init()
    {

        if (!_isInit)
        {
            _isInit = true;

            _game.Init();
        }
    }
}
