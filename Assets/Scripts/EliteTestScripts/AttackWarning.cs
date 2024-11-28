using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class AttackWarning : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer; // Ground 레이어
    [SerializeField] private GameObject warningIndicator; // 경고 표시 객체
    [SerializeField] private float warningDuration = 1f; // 경고 표시 지속 시간

    private Transform owner;

    private void Start()
    {
        if (warningIndicator != null)
        {
            warningIndicator.SetActive(false); // 초기에는 비활성화
        }
    }

    public void Initialize(Transform owner)
    {
        this.owner = owner;
    }

    public void ShowWarning(Vector3 position)
    {
        if (warningIndicator == null) return;

        // 땅 레이어와의 교차점 계산
        Ray ray = new Ray(position + Vector3.up * 1f, Vector3.down); // 위에서 아래로 Raycast
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            // Quad를 땅의 위치에 배치
            warningIndicator.transform.position = hit.point + Vector3.up * 0.01f; // 약간 위로 올림
            warningIndicator.transform.rotation = Quaternion.Euler(90, 0, 0); // 평면 정렬
            warningIndicator.SetActive(true);

            // 일정 시간 후 경고 숨기기
            CancelInvoke(nameof(HideWarning));
            Invoke(nameof(HideWarning), warningDuration);
        }
        else
        {
            Debug.LogWarning("Ground 레이어와 교차하지 않았습니다!");
        }
    }

    public void HideWarning()
    {
        if (warningIndicator != null)
        {
            warningIndicator.SetActive(false);
        }
    }
}