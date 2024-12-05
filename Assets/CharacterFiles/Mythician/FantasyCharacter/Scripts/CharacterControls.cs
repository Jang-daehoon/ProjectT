using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mythician.Assets.Characters
{
    public class CharacterControls : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> characterSkins = new List<GameObject>();
        private int currentSkin = 0, currentAnimation = 0;
        [SerializeField]
        private int totalAnimationClips = 0;
        [SerializeField]
        private List<CharacterAnimation> characterAnimations = new List<CharacterAnimation>();
        [SerializeField]
        private List<Renderer> weaponRenderers = new List<Renderer>();
        private void Reset()
        {
            characterSkins = new List<GameObject>();
            characterAnimations = new List<CharacterAnimation>();
            foreach (Transform child in transform)
            {
                characterSkins.Add(child.gameObject);
                characterAnimations.Add(child.GetComponent<CharacterAnimation>());
                child.gameObject.SetActive(false);
            }

            if (characterSkins.Count > 0)
            {
                characterSkins[0].SetActive(true);
            }

            totalAnimationClips = 4;
            weaponRenderers = new List<Renderer>();
            List<Renderer> allRenderers = new List<Renderer>();
            GetComponentsInChildren<Renderer>(true, allRenderers);
            weaponRenderers = allRenderers.FindAll(x => x.gameObject.name.ToLower().Contains("weapon") || x.gameObject.name.ToLower().Contains("bow") || x.gameObject.name.ToLower().Contains("sword"));
        }
        private void Update()
        {
            #region Skin
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentSkin++;
                SetSkin();
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentSkin--;
                SetSkin();
            }
            #endregion

            #region Animation
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentAnimation++;
                SetAnimation();
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentAnimation--;
                SetAnimation();
            }
            #endregion

            #region Weapon
            if (Input.GetKeyDown(KeyCode.F))
            {
                foreach (Renderer weapon in weaponRenderers) 
                {
                    weapon.enabled = !weapon.enabled;
                }
            }
            #endregion
        }
        private void SetSkin()
        {
            if (characterSkins.Count <= 0) return;
            currentSkin = (int)Mathf.Repeat(currentSkin, characterSkins.Count);
            foreach (GameObject skin in characterSkins)
            {
                if (skin.activeInHierarchy) 
                    skin.SetActive(false);
            }
            characterSkins[currentSkin].SetActive(true);
        }
        private void SetAnimation()
        {
            currentAnimation = (int)Mathf.Repeat(currentAnimation, totalAnimationClips);
            foreach (CharacterAnimation characterAnimation in characterAnimations)
            {
                characterAnimation.SetAnimation(currentAnimation);
            }
        }
    }
}
