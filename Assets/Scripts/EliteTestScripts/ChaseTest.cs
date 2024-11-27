using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTest : MonoBehaviour
{
    public float speed = 5f; // �̵� �ӵ�

    void Update()
    {
        // ����Ű �Է� �ޱ�
        float horizontal = Input.GetAxis("Horizontal"); // �¿� �Է�
        float vertical = Input.GetAxis("Vertical"); // ���� �Է�

        // �̵� ���� ���
        Vector3 movement = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;

        // Transform�� ����Ͽ� �̵�
        transform.Translate(movement, Space.World);

        // �̵� ������ �ִ� ��쿡�� ȸ��
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        if (direction.magnitude > 0.1f) // �̼��� �Է� ����
        {
            // �̵� �������� �÷��̾� ȸ��
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // �ε巯�� ȸ��
        }
    }
}
