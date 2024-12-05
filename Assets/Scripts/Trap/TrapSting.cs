using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapSting : Trap
{
    public GameObject trapObject;

    public float attCoolTime;
    public float attDelay;
    private bool isAtk = false;
    private bool isAtkOff = false;
    private bool isAttCoolTime = false;
    private bool isHit = true;

    private Vector3 startPos;
    private Vector3 endPos;

    private void Start()
    {
        startPos = trapObject.transform.position;
        endPos = trapObject.transform.position + Vector3.up * 2f;
    }

    private void Update()
    {
        if (isAtk == false) return;
        if (isAtk == true && isAtkOff == false)
        {
            trapObject.transform.position = Vector3.Lerp(trapObject.transform.position, endPos, Time.deltaTime * 15f);
        }
        if (isAtkOff == true)
        {
            trapObject.transform.position = Vector3.Lerp(trapObject.transform.position, startPos, Time.deltaTime * 10f);
        }
    }

    private IEnumerator Atk()
    {
        //트랩 작동 시작 올라옴
        isAtk = true;
        isHit = false;
        yield return new WaitForSeconds(2f);
        //트랩 작동 종료 내려감
        isAtkOff = true;
        isHit = true;
        yield return new WaitForSeconds(1f);
        //트랩 딜레이 시작 무반응
        isAtk = false;
        isAtkOff = false;
        yield return new WaitForSeconds(attCoolTime);
        isAttCoolTime = false;
    }

    

    private void OnCollisionStay(Collision collision)
    {
        if (isHit == false)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //플레이어 데미지
                //collision.gameObject.GetComponent<ITakeDamage>().TakeDamage(damage);
                Debug.Log($"{collision.gameObject.name} 피격");
                isHit = true;
            }
        }
        if (isAttCoolTime == true) return;
        isAttCoolTime = true;
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{collision.gameObject.name} 닿음");
            StartCoroutine(Atk());
        }
    }

}
