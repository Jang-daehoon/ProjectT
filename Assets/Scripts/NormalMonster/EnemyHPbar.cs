using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPbar : MonoBehaviour
{
    public Transform cam;
    public Image hpBar;
    public float maxHp;
    public float currentHp;

    public GameObject HpLineFolder;
    float unitHp = 20f;

    private void Start()
    {
        cam = Camera.main.transform;
        hpBar.fillAmount = currentHp / maxHp;
    }

    private void Update()
    {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, currentHp / maxHp, Time.deltaTime * 5f);
    }

    public void GetHpBoost()
    {
        float scalX = (100f / unitHp) / (maxHp / 20f);
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach ( Transform child in HpLineFolder.transform )
        {
            child.gameObject.transform.localScale = new Vector3(scalX, 1, 1);
        }
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);

    }


}
