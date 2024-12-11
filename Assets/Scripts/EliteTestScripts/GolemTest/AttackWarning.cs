using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyController;
public class AttackWarning : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer; // Ground 레이어
    [SerializeField] private GameObject warningIndicatorAttack; // 경고 표시 객체
    [SerializeField] private GameObject warningIndicatorSkill; // 경고 표시 객체

    private Transform owner;

    private void Start()
    {
        warningIndicatorAttack.SetActive(false); // 초기에는 비활성화
        warningIndicatorSkill.SetActive(false);
    }

    public void ShowWarning(Vector3 position, Quaternion baseRotation, EliteState state)
    {
        // 땅 레이어와의 교차점 계산
        Ray ray = new Ray(new Vector3(position.x, position.y + 10f, position.z), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            if (state == EliteState.SKILL)
            {
                if (warningIndicatorAttack != null) warningIndicatorAttack.SetActive(false);
                // Quad를 땅의 위치에 배치
                warningIndicatorSkill.transform.position = hit.point + Vector3.up * 0.1f; // 약간 위로 올림

                warningIndicatorSkill.transform.rotation = baseRotation;
                warningIndicatorSkill.SetActive(true);
            }
            else
            {
                // Quad를 땅의 위치에 배치
                warningIndicatorAttack.transform.position = hit.point + Vector3.up * 0.01f; // 약간 위로 올림

                // 기본 회전값에 X축 90도 회전 추가
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