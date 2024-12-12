using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameClearEnding : MonoBehaviour
{
    public Button button;

    private void Awake()
    {
        button.onClick.AddListener(GameExit);
    }

    public void GameExit()
    {
        print("asdasdasd");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}
