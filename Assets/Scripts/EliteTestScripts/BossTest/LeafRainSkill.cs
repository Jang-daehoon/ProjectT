using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafRainSkill : MonoBehaviour
{
    public GameObject warningIndicatorPrefab; // ��� ǥ�� ������
    public GameObject leafRainPrefab; // LeafRain ������
    public LayerMask groundLayer; // Ground ���̾�
    public float warningDuration = 1.2f; // ��� ǥ�� ���� �ð�
    public float skillRadius = 30f; // ��ų ����
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
            // ���� ��ġ ����
            Vector3 randomPosition = GetRandomGroundPosition();

            // ��� ǥ��
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
        // �� ���̾�� ������ ���
        Ray ray = new Ray(new Vector3(position.x, position.y + 10f, position.z), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            // Quad�� Ground ��ġ�� ��ġ
            GameObject warningIndicator = Instantiate(warningIndicatorPrefab, hit.point + Vector3.up * 0.01f, baseRotation);
            Destroy(warningIndicator, warningDuration); // ��� ǥ�� ����
        }
    }

    private Vector3 GetRandomGroundPosition()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * skillRadius;
        randomPoint.y = transform.position.y + 10f; // ���̸� ���� �ø�

        if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
