using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class AttackWarning : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer; // Ground ���̾�
    [SerializeField] private GameObject warningIndicator; // ��� ǥ�� ��ü
    [SerializeField] private float warningDuration = 1f; // ��� ǥ�� ���� �ð�

    private Transform owner;

    private void Start()
    {
        if (warningIndicator != null)
        {
            warningIndicator.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
        }
    }

    public void Initialize(Transform owner)
    {
        this.owner = owner;
    }

    public void ShowWarning(Vector3 position)
    {
        if (warningIndicator == null) return;

        // �� ���̾���� ������ ���
        Ray ray = new Ray(position + Vector3.up * 1f, Vector3.down); // ������ �Ʒ��� Raycast
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            // Quad�� ���� ��ġ�� ��ġ
            warningIndicator.transform.position = hit.point + Vector3.up * 0.01f; // �ణ ���� �ø�
            warningIndicator.transform.rotation = Quaternion.Euler(90, 0, 0); // ��� ����
            warningIndicator.SetActive(true);

            // ���� �ð� �� ��� �����
            CancelInvoke(nameof(HideWarning));
            Invoke(nameof(HideWarning), warningDuration);
        }
        else
        {
            Debug.LogWarning("Ground ���̾�� �������� �ʾҽ��ϴ�!");
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