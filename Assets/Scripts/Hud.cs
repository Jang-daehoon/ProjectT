using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Image hpGauge; // ü�� ������ �̹��� (FillAmount�� ����)
    public TextMeshProUGUI hpText; // ü�� ���¸� ǥ���� �ؽ�Ʈ

    private GameManager gameManager;


    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
    }

    // ü�� �������� �ؽ�Ʈ�� ������Ʈ�ϴ� �޼���
    void UpdateHealthUI()
    {
        float curHp = GameManager.Instance.player.curHp;
        float maxHp = GameManager.Instance.player.maxHp;

        // ü�� ������ ������Ʈ (0~1 ���� ��)
        hpGauge.fillAmount = curHp / maxHp;

        // ü�� �ؽ�Ʈ ������Ʈ
        hpText.text = $"{curHp} / {maxHp}";
    }
}
