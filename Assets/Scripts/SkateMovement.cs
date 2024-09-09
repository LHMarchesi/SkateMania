using System;
using UnityEngine;

public class SkateMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float pushForce;
    [SerializeField] private float maxSpeed = 7.5f;
    private float kickturn_thresh = 2f;
    private float kickturn_speed = 150f;
    private float turn_speed = 15f;
    private float sidewaysFriction = 15f;
    private Vector3 gravity = new(0, -300f, 0);

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 local_velocity = transform.InverseTransformDirection(rb.velocity);

        float h_input = Input.GetAxis("Horizontal");
        float v_input = Input.GetAxis("Vertical");

        physics(local_velocity);
        inputs(h_input, v_input, local_velocity);
    }
    void physics(Vector3 local_velocity) //applys custom physics like sideways friction and gravity
    {
        //gravity
        rb.AddForce(gravity * Time.fixedDeltaTime, ForceMode.Acceleration);

        //add sideways friction for realistic turning when on ground
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

    void inputs(float h_input, float v_input, Vector3 local_velocity)  //controls player inputs like turning and moving
    {

        //ground movement
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
