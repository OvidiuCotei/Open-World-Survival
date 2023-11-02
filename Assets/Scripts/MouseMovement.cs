using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvyKode
{
    public class MouseMovement : MonoBehaviour
    {
        public float mouseSensitivity = 100f;

        private float xRotation = 0f;
        private float YRotation = 0f;

        private void Start()
        {
            //Locking the cursor to the middle of the screen and making it invisible

        }

        private void Update()
        {
            if (InventorySystem.Instance.isOpen == false && CraftingSystem.Instance.isOpen == false && !MenuManager.Instance.isMenuOpen && !DialogSystem.Instance.dialogUIActive) {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                //control rotation around x axis (Look up and down)
                xRotation -= mouseY;

                //we clamp the rotation so we cant Over-rotate (like in real life)
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                //control rotation around y axis (Look up and down)
                YRotation += mouseX;

                //applying both rotations
                transform.localRotation = Quaternion.Euler(xRotation, YRotation, 0f);
            }
        }
    }
}