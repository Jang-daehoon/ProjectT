using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Godness : MonoBehaviour
{
    public bool isTalkDone;
    public ParticleSystem healParticle;

    private void Awake()
    {
        isTalkDone = false; 
    }

    public void Heal()
    {
        isTalkDone = true;
        StageManager stageManager = FindObjectOfType<StageManager>();
        stageManager.restRoomClear = true;

        ParticleSystem healParticleActive = Instantiate(healParticle, GameManager.Instance.player.transform.position, Quaternion.identity);
        GameManager.Instance.player.curHp = GameManager.Instance.player.maxHp;
        UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
        Debug.Log("������ �Ǵ����� �ִ� ü������ ȸ�������־����ϴ�.");
    }

}
