using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckWheelCollision : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheelColliders;

    private bool isGrounded;
    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }

    void Awake()
    {
        wheelColliders = GetComponentsInChildren<WheelCollider>();
    }

    void Update()
    {
        CheckIfGrounded();
    }

    private void CheckIfGrounded()
    {
        bool allWheelsGrounded = true;

        foreach (WheelCollider wheel in wheelColliders)
        {
            if (!wheel.isGrounded)
            {
                allWheelsGrounded = false;
                break;
            }
        }
       
        IsGrounded = allWheelsGrounded;
    }
}
