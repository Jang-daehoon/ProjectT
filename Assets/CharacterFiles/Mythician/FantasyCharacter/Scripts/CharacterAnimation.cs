using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mythician.Assets.Characters
{
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private List<string> stateNames = new List<string>();
        private int targetStateId = 0;
        private static float currentStateTime = 0;

        private void Reset()
        {
            animator = GetComponent<Animator>();
            stateNames = new List<string>();
            stateNames.Add("tpose");
            stateNames.Add("idle");
            stateNames.Add("walk");
            stateNames.Add("attack");
        }

        private void OnEnable()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            animator.Play(stateNames[targetStateId], -1, currentStateTime);
        }

        private void Update()
        {
            currentStateTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        public void SetAnimation(int stateId)
        {
            stateId = Mathf.Clamp(stateId, 0, stateNames.Count - 1);
            targetStateId = stateId;

            if (animator.gameObject.activeInHierarchy)
            {
                animator.CrossFadeInFixedTime(stateNames[stateId], .2f);
            }
        }
    }
}