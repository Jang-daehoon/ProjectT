using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : Singleton<ScenesManager>
{
    [Header("SceneSet")]
    public string Title;
    public string[] Stage;
    public string RestScene;

    public void ChanageScene(string nextScene)
    {
        //���� �ε��ϴ� ���� ������ ������ �ʰ�, �ε��� ����Ǵ� ���� �ٸ� �۾� ����
        //�ε� ���α׷���(���� ��Ȳ)�� ǥ���ϰų�, �ε� ȭ���� ����
        SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
    }
}
