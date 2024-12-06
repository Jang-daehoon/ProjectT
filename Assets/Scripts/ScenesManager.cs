using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : Singleton<ScenesManager>
{
    [Header("SceneSet")]
    public string Title;
    public string[] Stage;
    public string RestScene;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void ChanageScene(string nextScene)
     {
        //씬을 로드하는 동안 게임이 멈추지 않고, 로딩이 진행되는 동안 다른 작업 가능
        //로딩 프로그레스(진행 상황)을 표시하거나, 로딩 화면을 구현
        SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
     }

    
}
