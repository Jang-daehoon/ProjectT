using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMSuicideUnitRange : MonoBehaviour
{
    public float radius = 5f;         // 원의 반지름
    public int segments = 50;         // 원의 세그먼트 수 (원형 분할 정도)
    public Material material;         // LineRenderer에 적용할 마테리얼
    public float lineWidth = 0.1f;    // 선의 두께

    private LineRenderer lineRenderer;
    private void Awake()
    {
        // LineRenderer 컴포넌트 추가
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    public void OnRange()
    {
        // LineRenderer 속성 설정
        lineRenderer.material = material;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.positionCount = segments + 1;  // 원을 그리기 위해 점의 수는 segments + 1개

        // 부모 오브젝트의 위치를 기준으로 원 모양 점 계산
        Vector3 parentPosition = transform.position;

        for (int i = 0; i < segments + 1; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            Vector3 point = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius) + parentPosition;
            lineRenderer.SetPosition(i, point);
        }
    }
}
