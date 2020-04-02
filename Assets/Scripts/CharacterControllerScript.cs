using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{

    float rotationSpeed = 50;
    float rotation = 0f;
    public float jumpHeight;
    public bool grounded;
    public float moveSpeed;
    public bool activated;
    private Animator anim;
    private Rigidbody rb;
    private Vector3 dest;

    void Start()
    {
        jumpHeight = 20f;
        grounded = true;
        activated = false;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

        if (activated)
            Move();
        else
            anim.SetBool("move", false);

    }
    private void Move()
    {
        //anim.SetBool("move", true);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 2f;
        }
        else
        {
            moveSpeed = 1f;
        }


        //print(Input.GetAxis("Vertical"));
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        bool shouldMove = moveVertical != 0 || moveHorizontal != 0;

        // Update animation parameters
        anim.SetBool("move", shouldMove);
        anim.SetFloat("vel_y", moveVertical * moveSpeed);
        anim.SetFloat("vel_x", moveHorizontal * moveSpeed);


        //hold spacebar to jump, wip
        if (Input.GetKey(KeyCode.Space))
        {
                      grounded = false;
            anim.SetFloat("jump", 1);
        } else {
          anim.SetFloat("jump", 0);

        }




        rotation += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        if (moveVertical == 0)
        {
            transform.eulerAngles = new Vector3(0, rotation, 0);
        }
        //anim.SetFloat("vel_x", rotation);
    }
    // Detects collision with the ground to enable jumping, with walls and players to reduce points and reset speed.
    private void OnCollisionEnter(Collision collision)
    {
      print("OnCollisionEnter");

        if (collision.gameObject.CompareTag("Ground"))
        {
          print("OnCollisionEnter if ");

            grounded = true;
            anim.SetFloat("jump", 0);
            print("OnCollisionEnter after");

        }
    }
    private void OnCollisionExit(Collision collision)
    {
      print("OnCollisionExit");
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
            anim.SetFloat("jump", 1);
        }
    }
    // Moves agent to desired vector.
    public void MoveAgent(Vector3 t)
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().destination = t;
        //anim.SetBool("move", true);
        dest = t;
    }
    public Vector3 Destdir()
    {
        return dest;
    }
}
