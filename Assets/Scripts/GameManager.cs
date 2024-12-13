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

    [Header("����")]
    public List<RelicData> relicList;
    
    public List<RelicData> commonRelic;
    public List<RelicData> unCommonRelic;
    public List<RelicData> rareRelic;

    public static GameManagerEx Game => Instance._game;

    private bool _isInit = false;

    private void Awake()
    {
        // �� ��ȯ ���� �÷��̾�� ī�޶� �� ��ȯ �Ŀ��� �����ǵ��� ����
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
