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
    public SkillManager skillManager;
    private GameManagerEx _game = new GameManagerEx();

    [Header("유물")]
    public List<RelicData> relicList;
    
    public List<RelicData> commonRelic;
    public List<RelicData> unCommonRelic;
    public List<RelicData> rareRelic;

    public static GameManagerEx Game => Instance._game;

    private bool _isInit = false;

    private void Awake()
    {
        // 씬 전환 전에 플레이어와 카메라를 씬 전환 후에도 유지되도록 설정
        DontDestroyOnLoad(player.gameObject);
        DontDestroyOnLoad(playerCamera.gameObject);
        DontDestroyOnLoad(mapGenerator.gameObject);
        DontDestroyOnLoad(eventSystem.gameObject);  
        DontDestroyOnLoad(skillManager.gameObject);
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
