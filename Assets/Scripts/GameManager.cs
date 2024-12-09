using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    public CinemachineVirtualCamera playerCamera;
    public MapGenerator mapGenerator;
    public EventSystem eventSystem;
    public TutorialManager tutorialManager;

    private GameManagerEx _game = new GameManagerEx();
    

    public static GameManagerEx Game => Instance._game;

    private bool _isInit = false;

    private void Awake()
    {
        // 씬 전환 전에 플레이어와 카메라를 씬 전환 후에도 유지되도록 설정
        DontDestroyOnLoad(player.gameObject);
        DontDestroyOnLoad(playerCamera.gameObject);
        DontDestroyOnLoad(mapGenerator.gameObject);
        DontDestroyOnLoad(eventSystem.gameObject);  
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
