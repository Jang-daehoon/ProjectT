using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTest : MonoBehaviour
{
    public float speed = 5f; // 이동 속도

    void Update()
    {
        // 방향키 입력 받기
        float horizontal = Input.GetAxis("Horizontal"); // 좌우 입력
        float vertical = Input.GetAxis("Vertical"); // 상하 입력

        // 이동 벡터 계산
        Vector3 movement = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;

        // Transform을 사용하여 이동
        transform.Translate(movement, Space.World);

        // 이동 방향이 있는 경우에만 회전
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        if (direction.magnitude > 0.1f) // 미세한 입력 방지
        {
            // 이동 방향으로 플레이어 회전
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // 부드러운 회전
        }
    }
}
