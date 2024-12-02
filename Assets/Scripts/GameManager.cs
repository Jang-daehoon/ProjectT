using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;

    private GameManagerEx _game = new GameManagerEx();
    

    public static GameManagerEx Game => Instance._game;

    private bool _isInit = false;


    public void Init()
    {

        if (!_isInit)
        {
            _isInit = true;

            _game.Init();
        }
    }
}
