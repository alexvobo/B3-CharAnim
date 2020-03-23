using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterController : MonoBehaviour
{
    public float movementSpeed;
    private Camera cam;
    private Rigidbody rb;
    public bool grounded;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        moveChar();
    }
    void moveChar()
    {

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input = transform.TransformDirection(input);
        if (input.magnitude > 1) input.Normalize();
        float gravity = Physics.gravity.y * 3;
        Vector3 move = rb.velocity;

        Vector3 p1 = transform.position + Vector3.down * 0.5f;

        if (this.transform.position.y <= .5f)
        {
            grounded = true;
        } else
        {
            grounded = false;
        }

        if (grounded)
        {
            move = input * movementSpeed;
            move.y = gravity * Time.deltaTime;

            if (Input.GetKeyDown("space"))
            {
                move.y += 10f ;
                grounded = false;
            }
        }
        else
        {
            move.x = Mathf.Clamp(move.x + input.x * 0.01f * movementSpeed, -movementSpeed, movementSpeed);
            move.y = Mathf.Clamp(move.y + gravity * Time.deltaTime, gravity, movementSpeed);
            move.z = Mathf.Clamp(move.z + input.z * 0.01f * movementSpeed, -movementSpeed, movementSpeed);
        }

        transform.rotation = cam.transform.rotation;
        Vector3 euler = transform.eulerAngles;
        euler.x = 0;
        transform.eulerAngles = euler;

        rb.AddForce(move);
    }
}
