using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMSuicideUnitRange : MonoBehaviour
{
    public float radius = 5f;         // ���� ������
    public int segments = 50;         // ���� ���׸�Ʈ �� (���� ���� ����)
    public Material material;         // LineRenderer�� ������ ���׸���
    public float lineWidth = 0.1f;    // ���� �β�

    private LineRenderer lineRenderer;
    private void Awake()
    {
        // LineRenderer ������Ʈ �߰�
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    public void OnRange()
    {
        // LineRenderer �Ӽ� ����
        lineRenderer.material = material;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.positionCount = segments + 1;  // ���� �׸��� ���� ���� ���� segments + 1��

        // �θ� ������Ʈ�� ��ġ�� �������� �� ��� �� ���
        Vector3 parentPosition = transform.position;

        for (int i = 0; i < segments + 1; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            Vector3 point = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius) + parentPosition;
            lineRenderer.SetPosition(i, point);
        }
    }
}
