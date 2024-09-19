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
        bool allWheelsGrounded = true; // Asumimos que todas están tocando el suelo inicialmente

        foreach (WheelCollider wheel in wheelColliders)
        {
            if (!wheel.isGrounded) // Si alguna rueda no está tocando el suelo
            {
                allWheelsGrounded = false; // Cambia a false
                break;
            }
        }
       
        IsGrounded = allWheelsGrounded; // Asigna el resultado a IsGrounded
    }
}
