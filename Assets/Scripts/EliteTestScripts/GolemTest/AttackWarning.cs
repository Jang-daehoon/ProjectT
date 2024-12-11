using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyController;
public class AttackWarning : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer; // Ground ���̾�
    [SerializeField] private GameObject warningIndicatorAttack; // ��� ǥ�� ��ü
    [SerializeField] private GameObject warningIndicatorSkill; // ��� ǥ�� ��ü

    private Transform owner;

    private void Start()
    {
        warningIndicatorAttack.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
        warningIndicatorSkill.SetActive(false);
    }

    public void ShowWarning(Vector3 position, Quaternion baseRotation, EliteState state)
    {
        // �� ���̾���� ������ ���
        Ray ray = new Ray(new Vector3(position.x, position.y + 10f, position.z), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            if (state == EliteState.SKILL)
            {
                if (warningIndicatorAttack != null) warningIndicatorAttack.SetActive(false);
                // Quad�� ���� ��ġ�� ��ġ
                warningIndicatorSkill.transform.position = hit.point + Vector3.up * 0.1f; // �ణ ���� �ø�

                warningIndicatorSkill.transform.rotation = baseRotation;
                warningIndicatorSkill.SetActive(true);
            }
            else
            {
                // Quad�� ���� ��ġ�� ��ġ
                warningIndicatorAttack.transform.position = hit.point + Vector3.up * 0.01f; // �ణ ���� �ø�

                // �⺻ ȸ������ X�� 90�� ȸ�� �߰�
                warningIndicatorAttack.transform.rotation = baseRotation * Quaternion.Euler(90, 0, 0);
                warningIndicatorAttack.SetActive(true);
            }
        }
    }

    public void HideWarning()
    {
        warningIndicatorAttack.SetActive(false);
        warningIndicatorSkill.SetActive(false);
    }
}