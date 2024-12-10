using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnknownManager : Singleton<UnknownManager>
{
   
    //Unknown���� ����� �޼����

    //���� ���� ȹ��
    public void GetRandomRelic()
    {
        // StageManager ������Ʈ �˻�
        StageManager stageManager = FindObjectOfType<StageManager>();

        // StageManager�� �����ϴ��� Ȯ��
        if (stageManager != null)
        {
            //���� ����
            //stageManager�� ã�Ƽ� spawnPos�� ã�Ƽ� �ش� ��ġ�� ��ȯ
            Instantiate(stageManager.rewardItemPrefab, stageManager.rewardSpawnPoint.position, Quaternion.identity);
            Debug.Log("���� �������� �����Ǿ����ϴ�.");

            // unknownClear �Ӽ��� true�� ����
            stageManager.unknownClear = true;
            Debug.Log("StageManager�� unknownClear�� true�� ����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("StageManager�� ã�� �� �����ϴ�.");
        }

        if (UiManager.Instance.isUnknownUiActive == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi, ref UiManager.Instance.isUnknownUiActive);
        }
        else if (UiManager.Instance.isUnknownUiActive2 == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi2, ref UiManager.Instance.isUnknownUiActive);
        }
        else if (UiManager.Instance.isUnknownUiActive3 == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi3, ref UiManager.Instance.isUnknownUiActive);
        }

        RoomManager.Instance.ClearRoom();
    }

    public void someoneIsWatchingMe()
    {
        int ranDamage =  Random.Range(10, 30);
        GameManager.Instance.player.curHp -= ranDamage;

        // StageManager ������Ʈ �˻�
        StageManager stageManager = FindObjectOfType<StageManager>();

        // StageManager�� �����ϴ��� Ȯ��
        if (stageManager != null)
        {
            // unknownClear �Ӽ��� true�� ����
            stageManager.unknownClear = true;
            Debug.Log("StageManager�� unknownClear�� true�� ����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("StageManager�� ã�� �� �����ϴ�.");
        }

        if (UiManager.Instance.isUnknownUiActive == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi, ref UiManager.Instance.isUnknownUiActive);
        }
        else if (UiManager.Instance.isUnknownUiActive2 == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi2, ref UiManager.Instance.isUnknownUiActive);
        }
        else if (UiManager.Instance.isUnknownUiActive3 == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi3, ref UiManager.Instance.isUnknownUiActive);
        }

        RoomManager.Instance.ClearRoom();
    }

    public void RunAway()
    {
        // StageManager ������Ʈ �˻�
        StageManager stageManager = FindObjectOfType<StageManager>();

        // StageManager�� �����ϴ��� Ȯ��
        if (stageManager != null)
        {
            // unknownClear �Ӽ��� true�� ����
            stageManager.unknownClear = true;
            Debug.Log("StageManager�� unknownClear�� true�� ����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("StageManager�� ã�� �� �����ϴ�.");
        }

        if (UiManager.Instance.isUnknownUiActive == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi, ref UiManager.Instance.isUnknownUiActive);
        }
        else if (UiManager.Instance.isUnknownUiActive2 == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi2, ref UiManager.Instance.isUnknownUiActive);
        }
        else if (UiManager.Instance.isUnknownUiActive3 == true)
        {
            UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi3, ref UiManager.Instance.isUnknownUiActive);
        }

        RoomManager.Instance.ClearRoom();
    }
    
}
