using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
        [Header("Inputs")]
        [SerializeField] private KeyCode forward = KeyCode.W;
        [SerializeField] private KeyCode backward = KeyCode.S;
        [SerializeField] private KeyCode left = KeyCode.A;
        [SerializeField] private KeyCode right = KeyCode.D;
        [SerializeField] private KeyCode jump = KeyCode.Space;
        [SerializeField] private KeyCode run = KeyCode.LeftShift;

        private Vector3 inputVector;
        public Vector3 InputVector => inputVector;

        public bool IsJumping { get => isJumping; set => isJumping = value; }

        private bool isJumping;

        private bool isRunning;
        public bool IsRunning { get => isRunning; set => isRunning = value; }

        private float xInput;
        private float yInput;
        private float zInput;


        public void HandleInput()
        {
            xInput = 0;
            yInput = 0;
            zInput = 0;

            if (Input.GetKey(forward))
            {
                zInput++;
            }
            if (Input.GetKey(backward))
            {
                zInput--;
            }
            if (Input.GetKey(left))
            {
                xInput--;
            }
            if (Input.GetKey(right))
            {
                xInput++;
            }

            inputVector = new Vector3(xInput, yInput, zInput);

            isJumping = Input.GetKeyDown(jump);
            isRunning = Input.GetKey(run);
        }

        void Update()
        {
            HandleInput();
        }

    }

