using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Image hpGauge; // 체력 게이지 이미지 (FillAmount로 설정)
    public TextMeshProUGUI hpText; // 체력 상태를 표시할 텍스트

    private GameManager gameManager;


    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
    }

    // 체력 게이지와 텍스트를 업데이트하는 메서드
    void UpdateHealthUI()
    {
        float curHp = GameManager.Instance.player.curHp;
        float maxHp = GameManager.Instance.player.maxHp;

        // 체력 게이지 업데이트 (0~1 사이 값)
        hpGauge.fillAmount = curHp / maxHp;

        // 체력 텍스트 업데이트
        hpText.text = $"{curHp} / {maxHp}";
    }
}
