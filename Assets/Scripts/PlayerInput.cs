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

    public Vector3 InputVector => inputVector;

    private Vector3 inputVector;

    [SerializeField] private CheckWheelCollision checkWheelCollision;

    public bool IsJumping { get => isJumping; set => isJumping = value; }

    private bool isJumping;

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

        if (checkWheelCollision.IsGrounded)
        {
            isJumping = Input.GetKeyDown(jump); 
        }
    }

    void Update()
    {
        HandleInput();
    }

    public void SetJumping(bool state)
    {
        isJumping = state;
    }

}

