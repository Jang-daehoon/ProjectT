using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAx : Trap
{

    public HingeJoint joint;
    public Transform tarpAxTrans;
    public float startRotation;
    public float motorForce = 10f;
    public float motorSpeed = 100f;

    private void Start()
    {
        tarpAxTrans.Rotate(new Vector3(0, 0, startRotation));
        JointMotor motor = joint.motor;
        motor.force = motorForce;
        motor.targetVelocity = motorSpeed;
        joint.useMotor = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //플레이어 데미지
            //collision.gameObject.GetComponent<ITakeDamage>().TakeDamage(damage);
            Debug.Log($"{other.gameObject.name} 피격");
        }
    }


}
