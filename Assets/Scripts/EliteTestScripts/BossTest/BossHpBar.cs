using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    public Image hpBar;
    public BossDryad boss;
    public bool isBossSpawn = false;

    public void inHp()
    {
        hpBar.fillAmount = boss.curHp / boss.maxHp;
        isBossSpawn = true;
    }

    private void Update()
    {
        if(isBossSpawn == true)
            hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, boss.curHp / boss.maxHp, Time.deltaTime * 5f);
    }

}
