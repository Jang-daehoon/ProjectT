using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoonsCodes
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(Animator))]
    public abstract class Character : MonoBehaviour
    {
        [Header("Character Info")]
        [SerializeField] protected float maxHp;
        [SerializeField] protected float curHp;
        [SerializeField] protected float moveSpeed;

        [SerializeField] protected float dmgValue;
        [SerializeField] protected float atkSpeed;   //공격속도

        [SerializeField] protected bool isDead;
        [Header("----------------------------")]
        protected Rigidbody rb;
        protected Collider col;
        protected Animator animator;

        //정의 필수
        public abstract void Move();
        public abstract void Dead();

    }
}

