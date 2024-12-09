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
        [SerializeField] public float atkSpeed;   //공격속도

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

        //정의 필수.
        public abstract void Move();
        public abstract void Dead();
    }
}
