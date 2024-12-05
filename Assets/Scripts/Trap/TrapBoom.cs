using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class TrapBoom : Trap, ITakeDamage
{
    public ParticleSystem boomParticle;
    public float range;
    public float boomDelay;
    public Transform rangePos;

    private CapsuleCollider myCollider;
    public GameObject boomRange;
    private LineRenderer lineRenderer;
    private int segments = 50;         // ���� ���׸�Ʈ �� (���� ���� ����)
    public Material material;         // LineRenderer�� ������ ���׸���
    private float lineWidth = 0.1f;    // ���� �β�

    private void Awake()
    {
        // LineRenderer ������Ʈ �߰�
        lineRenderer = boomRange.gameObject.GetComponent<LineRenderer>();
        myCollider = gameObject.GetComponent<CapsuleCollider>();
        boomRange.gameObject.SetActive(false);
    }


    private IEnumerator Boom()
    {
        boomRange.gameObject.SetActive(true);
        OnRange();
        MatChange();
        yield return new WaitForSeconds(boomDelay);
        Instantiate(boomParticle, transform.position, transform.rotation);
        Att();
        yield return null;
        Destroy(this.gameObject);
    }
    private void Att()
    {
        Collider[] hitColl = Physics.OverlapSphere(rangePos.transform.position, range);
        foreach (Collider coll in hitColl)
        {
            if (coll.CompareTag("Player") || coll.CompareTag("Enemy"))
            {
                //coll.GetComponent<ITakeDamage>().TakeDamage(damage);
                Debug.Log($"{coll.name} ���� ����");
            }
        }
    }


    private void OnRange()
    {
        // LineRenderer �Ӽ� ����
        lineRenderer.material = material;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.positionCount = segments + 1;  // ���� �׸��� ���� ���� ���� segments + 1��

        // �θ� ������Ʈ�� ��ġ�� �������� �� ��� �� ���
        Vector3 parentPosition = rangePos.transform.position;

        for (int i = 0; i < segments + 1; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            Vector3 point = new Vector3(Mathf.Cos(angle) * range, 0f, Mathf.Sin(angle) * range) + parentPosition;
            lineRenderer.SetPosition(i, point);
        }
    }

    private void MatChange()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    public void TakeDamage(float damage)
    {
        myCollider.enabled = false;
        StartCoroutine(Boom());
    }
}
