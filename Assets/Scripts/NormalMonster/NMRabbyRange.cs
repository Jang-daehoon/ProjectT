using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMRabbyRange : MonoBehaviour
{
    public Transform firePoint;     // �Ѿ� �߻� ���� (�ѱ�)
    public float maxRange = 20f;    // �Ѿ� ��Ÿ�
    public LineRenderer lineRenderer;  // LineRenderer ������Ʈ

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        // LineRenderer �ʱ� ����
        lineRenderer.positionCount = 2;  // �� ������ ������ �׸��Ƿ� positionCount�� 2�� ����
        lineRenderer.startWidth = 0.5f;  // ���� ���� ����
        lineRenderer.endWidth = 0.5f;    // ���� �� ����
        lineRenderer.startColor = Color.red;  // ���� ���� ��
        lineRenderer.endColor = Color.red;    // ���� �� ��
        firePoint = this.transform;
    }
    public void OnRange()
    {
        // �Ѿ��� �߻�Ǵ� ������ ���� (ī�޶� ��ǥ �������� ������ �� �ֽ��ϴ�)
        Vector3 direction = transform.forward;  // �Ѿ� �߻� ���� (�� ��������)

        // �ִ� ��Ÿ��� �ݿ��� ���� ���
        Vector3 endPoint = firePoint.position + direction * maxRange;

        // LineRenderer�� �������� ���� ����
        lineRenderer.SetPosition(0, firePoint.position);  // ������ (�ѱ� ��ġ)
        lineRenderer.SetPosition(1, endPoint);            // ���� (��Ÿ� ��)
    }


}
