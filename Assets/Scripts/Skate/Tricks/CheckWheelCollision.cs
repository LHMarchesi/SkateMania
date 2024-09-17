using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckWheelCollision : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheelColliders;
    void Awake()
    {
        wheelColliders = GetComponentsInChildren<WheelCollider>(); 
    }

    // Update is called once per frame
    void Update()
    {
        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            
        }
    }
}
