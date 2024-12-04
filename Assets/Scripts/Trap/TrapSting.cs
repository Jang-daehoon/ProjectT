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
        //Ʈ�� �۵� ���� �ö��
        isAtk = true;
        isHit = false;
        yield return new WaitForSeconds(2f);
        //Ʈ�� �۵� ���� ������
        isAtkOff = true;
        isHit = true;
        yield return new WaitForSeconds(1f);
        //Ʈ�� ������ ���� ������
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
                //�÷��̾� ������
                //collision.gameObject.GetComponent<ITakeDamage>().TakeDamage(damage);
                Debug.Log($"{collision.gameObject.name} �ǰ�");
                isHit = true;
            }
        }
        if (isAttCoolTime == true) return;
        isAttCoolTime = true;
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{collision.gameObject.name} ����");
            StartCoroutine(Atk());
        }
    }

}
