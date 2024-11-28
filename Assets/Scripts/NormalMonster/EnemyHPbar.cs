using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPbar : MonoBehaviour
{
    public Transform cam;
    public Image hpBar;
    public Image backHpBar;
    public float maxHp;
    public float currentHp;

    private bool backHpHit = false;

    public GameObject HpLineFolder;
    float unitHp = 20f;

    Coroutine coroutine;

    private void Start()
    {
        cam = Camera.main.transform;
        hpBar.fillAmount = currentHp / maxHp;
        backHpBar.fillAmount = currentHp / maxHp;
    }

    private void Update()
    {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, currentHp / maxHp, Time.deltaTime * 5f);

        if (backHpHit == true)
        {
            backHpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, currentHp / maxHp, Time.deltaTime * 5f);
            if(hpBar.fillAmount >= backHpBar.fillAmount - 0.01f)
            {
                backHpHit = false;
                backHpBar.fillAmount = hpBar.fillAmount;
            }
        }
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

    public void HpBarUpdate()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(ChangeBackHpBar());
        }
    }

    private IEnumerator ChangeBackHpBar()
    {
        yield return new WaitForSeconds(1f);
        backHpHit = true;
        coroutine = null;
    }
        
    
}
