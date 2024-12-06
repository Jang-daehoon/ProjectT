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
        //���� �ε��ϴ� ���� ������ ������ �ʰ�, �ε��� ����Ǵ� ���� �ٸ� �۾� ����
        //�ε� ���α׷���(���� ��Ȳ)�� ǥ���ϰų�, �ε� ȭ���� ����
        SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
     }

    
}
