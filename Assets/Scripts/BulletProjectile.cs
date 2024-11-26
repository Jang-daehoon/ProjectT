using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public float Damage;
    public float Speed;
    public float Lifetime = 5f; // 총알의 생명 시간 (초)

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 총알을 전방으로 이동
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체가 적인지 확인 
        // 적에게 데미지 전달 (Enemy 스크립트 필요)

        // 충돌 시 총알 파괴
    }
}
