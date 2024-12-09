using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoonsCodes
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(Animator))]
    public abstract class Character : MonoBehaviour
    {
        [Header("Character Info")]
        public float maxHp;
        public float curHp;
        [SerializeField] public float moveSpeed;

        public float dmgValue;
        [SerializeField] public float atkSpeed;   //���ݼӵ�

        [SerializeField] protected bool isDead;
        [Header("----------------------------")]

        [Header("CharacterData")]
        public CharacterData characterData; //ScriptableObj

        protected Rigidbody rb;
        protected Collider col;
        protected Animator animator;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            animator = GetComponent<Animator>();
        }

        //���� �ʼ�.
        public abstract void Move();
        public abstract void Dead();
    }
}
