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
    private int segments = 50;         // 원의 세그먼트 수 (원형 분할 정도)
    public Material material;         // LineRenderer에 적용할 마테리얼
    private float lineWidth = 0.1f;    // 선의 두께

    private void Awake()
    {
        // LineRenderer 컴포넌트 추가
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
                Debug.Log($"{coll.name} 폭발 피해");
            }
        }
    }


    private void OnRange()
    {
        // LineRenderer 속성 설정
        lineRenderer.material = material;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.positionCount = segments + 1;  // 원을 그리기 위해 점의 수는 segments + 1개

        // 부모 오브젝트의 위치를 기준으로 원 모양 점 계산
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
