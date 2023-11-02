using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvyKode
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController controller;
        private AudioSource audioSource;

        public float speed = 12f;
        public float gravity = -9.81f * 2;
        public float jumpHeight = 3f;
        public float groundDistance = 0.4f;

        public Transform groundCheck;
        public LayerMask groundMask;

        Vector3 velocity;

        private bool isGrounded;

        private Vector3 lastposition = Vector3.zero;
        public bool isMoving;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (InventorySystem.Instance.isOpen == false && CraftingSystem.Instance.isOpen == false && DialogSystem.Instance.dialogUIActive == false && MenuManager.Instance.isMenuOpen == false)
            {
                Movement();
            }
        }

        public void Movement()
        {
            //checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            //right is the red Axis, foward is the blue axis
            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            //check if the player is on the ground so he can jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                //the equation for jumping
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            if (lastposition != gameObject.transform.position && isGrounded)
            {
                isMoving = true;

                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SoundManager.Instance.grassWalkSound;
                    audioSource.Play();
                }
            }
            else
            {
                isMoving = false;
                audioSource.Stop();
            }

            lastposition = gameObject.transform.position;
        }
    }
}