using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyController;
public class AttackWarning : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer; // Ground ���̾�
    [SerializeField] private GameObject warningIndicatorAttack; // ��� ǥ�� ��ü
    [SerializeField] private GameObject warningIndicatorSkill; // ��� ǥ�� ��ü
    [SerializeField] private float warningDuration = 1.2f; // ��� ǥ�� ���� �ð�

    private Transform owner;

    private void Start()
    {
        warningIndicatorAttack.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
        warningIndicatorSkill.SetActive(false);
    }

    public void Initialize(Transform owner)
    {
        this.owner = owner;
    }

    public void ShowWarning(Vector3 position, Quaternion baseRotation, EliteState state)
    {
        // �� ���̾���� ������ ���
        Ray ray = new Ray(new Vector3(position.x, position.y + 10f, position.z), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            if (state == EliteState.ATTACK)
            {
                // Quad�� ���� ��ġ�� ��ġ
                warningIndicatorAttack.transform.position = hit.point + Vector3.up * 0.01f; // �ణ ���� �ø�

                // �⺻ ȸ������ X�� 90�� ȸ�� �߰�
                warningIndicatorAttack.transform.rotation = baseRotation * Quaternion.Euler(90, 0, 0);
                warningIndicatorAttack.SetActive(true);
            }
            else if (state == EliteState.SKILL)
            {
                if (warningIndicatorAttack != null) warningIndicatorAttack.SetActive(false);
                // Quad�� ���� ��ġ�� ��ġ
                warningIndicatorSkill.transform.position = hit.point + Vector3.up * 0.01f; // �ణ ���� �ø�

                // �⺻ ȸ������ X�� 90�� ȸ�� �߰�
                warningIndicatorSkill.transform.rotation = baseRotation;
                warningIndicatorSkill.SetActive(true);
            }
        }
    }

    public void HideWarning(EliteState state)
    {
        if (state == EliteState.ATTACK) warningIndicatorAttack.SetActive(false);
        else warningIndicatorSkill.SetActive(false);
    }
}