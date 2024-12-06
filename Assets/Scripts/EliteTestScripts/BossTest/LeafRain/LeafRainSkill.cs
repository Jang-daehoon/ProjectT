using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafRainSkill : MonoBehaviour
{
    public GameObject warningIndicatorPrefab; // 경고 표시 프리팹
    public GameObject leafRainPrefab; // LeafRain 프리팹
    public LayerMask groundLayer; // Ground 레이어
    public float warningDuration = 1.2f; // 경고 표시 지속 시간
    public float skillRadius = 30f; // 스킬 범위
    public float rainCount = 25f;
    public float rainInterval = 0.2f;

    public void UseLeafRainSkill()
    {
        StartCoroutine(ExecuteLeafRain());
    }

    private IEnumerator ExecuteLeafRain()
    {
        for (int i = 0; i < rainCount; i++)
        {
            // 랜덤 위치 생성
            Vector3 randomPosition = GetRandomGroundPosition();

            // 경고 표시
            ShowWarning(randomPosition, Quaternion.identity);
            if (randomPosition != Vector3.zero)
            {
                Instantiate(leafRainPrefab, randomPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(rainInterval);
        }
    }

    private void ShowWarning(Vector3 position, Quaternion baseRotation)
    {
        // 땅 레이어와 교차점 계산
        Ray ray = new Ray(new Vector3(position.x, position.y + 10f, position.z), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            // Quad를 Ground 위치에 배치
            GameObject warningIndicator = Instantiate(warningIndicatorPrefab, hit.point + Vector3.up * 0.01f, baseRotation);
            Destroy(warningIndicator, warningDuration); // 경고 표시 제거
        }
    }

    private Vector3 GetRandomGroundPosition()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * skillRadius;
        randomPoint.y = transform.position.y + 10f; // 높이를 위로 올림

        if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
