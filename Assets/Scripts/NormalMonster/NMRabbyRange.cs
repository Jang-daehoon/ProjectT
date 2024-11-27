using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMRabbyRange : MonoBehaviour
{
    public Transform firePoint;     // 총알 발사 지점 (총구)
    public float maxRange = 20f;    // 총알 사거리
    public LineRenderer lineRenderer;  // LineRenderer 컴포넌트

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        // LineRenderer 초기 설정
        lineRenderer.positionCount = 2;  // 두 점으로 직선을 그리므로 positionCount를 2로 설정
        lineRenderer.startWidth = 0.5f;  // 선의 시작 굵기
        lineRenderer.endWidth = 0.5f;    // 선의 끝 굵기
        lineRenderer.startColor = Color.red;  // 선의 시작 색
        lineRenderer.endColor = Color.red;    // 선의 끝 색
        firePoint = this.transform;
    }
    public void OnRange()
    {
        // 총알이 발사되는 방향을 설정 (카메라나 목표 방향으로 설정할 수 있습니다)
        Vector3 direction = transform.forward;  // 총알 발사 방향 (앞 방향으로)

        // 최대 사거리를 반영한 끝점 계산
        Vector3 endPoint = firePoint.position + direction * maxRange;

        // LineRenderer의 시작점과 끝점 설정
        lineRenderer.SetPosition(0, firePoint.position);  // 시작점 (총구 위치)
        lineRenderer.SetPosition(1, endPoint);            // 끝점 (사거리 끝)
    }


}
