using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mythician.Assets.Characters
{
    public class CharacterRotation : MonoBehaviour
    {
        public float sensitivity = 5f;
        void Update()
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                float rotation = Input.GetAxis("Mouse X") * sensitivity;
                transform.Rotate(new Vector3(0f, -rotation, 0f));
            }
        }
    }
}
