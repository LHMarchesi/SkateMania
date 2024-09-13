using System;
using UnityEngine;

public class SkateController : MonoBehaviour
{
    public Vector3 gravity = new(0, -300f, 0);

    [SerializeField] private float pushForce;
    [SerializeField] private float maxSpeed = 7.5f;
    [SerializeField] private float kickturn_thresh = 2f;
    [SerializeField] private float kickturn_speed = 150f;
    [SerializeField] private float turn_speed = 15f;
    [SerializeField] private float sidewaysFriction = 15f;

    private Rigidbody rb;
    private PlayerInput playerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        Vector3 local_velocity = transform.InverseTransformDirection(rb.velocity);

        float h_input = Input.GetAxis("Horizontal");
        float v_input = Input.GetAxis("Vertical");

        if (!playerInput.IsJumping)
        {
            Physics(local_velocity);
            Inputs(h_input, v_input, local_velocity);
        }
    }
    private void Physics(Vector3 local_velocity) // custom physics movimiento lateral y gravedad
    {
        //gravity
        rb.AddForce(gravity * Time.fixedDeltaTime, ForceMode.Acceleration);

        //add sideways friction for turning 
        float sideways_velocity = local_velocity.x;
        if (sideways_velocity > 0.3)
        {
            sideways_velocity -= sidewaysFriction * Time.fixedDeltaTime;
        }
        else if (sideways_velocity < -0.3)
        {
            sideways_velocity += sidewaysFriction * Time.fixedDeltaTime;
        }
        else
        {
            sideways_velocity = 0;
        }
        local_velocity.x = sideways_velocity; //set back

        rb.velocity = transform.TransformDirection(local_velocity); //set back to world rel
    }

   private void Inputs(float h_input, float v_input, Vector3 local_velocity)  //controla los inputs para y agrega fuerzas   // Volver a ver
    {
        //forward
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * v_input * Time.fixedDeltaTime * 400, ForceMode.Acceleration);
        }

        //rotate rigid
        Quaternion delta_rotation;
        if (Math.Abs(local_velocity.z) <= kickturn_thresh && Math.Abs(v_input) == 0)
        {
            delta_rotation = Quaternion.Euler(new Vector3(0, h_input, 0) * Time.fixedDeltaTime * kickturn_speed);
        }
        else
        {
            delta_rotation = Quaternion.Euler(new Vector3(0, h_input, 0) * Time.fixedDeltaTime * local_velocity.z * turn_speed);
        }

        //using MoveRotation for physics handling
        rb.MoveRotation(rb.rotation * delta_rotation);

    }
}
