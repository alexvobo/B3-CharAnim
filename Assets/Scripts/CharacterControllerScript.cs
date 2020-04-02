using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterControllerScript : MonoBehaviour
{

    float rotationSpeed = 50;
    float rotation = 0f;
    public float jumpHeight;
    public bool grounded;
    public float moveSpeed;
    public bool activated;
    private Animator anim;
    private NavMeshAgent nma;
    private Rigidbody rb;
    private Vector3 dest;
    private float distanceThresh = .5f;

    void Start()
    {
        jumpHeight = 20f;
        grounded = true;
        activated = false;
        nma = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("jump", 0);
        Move();

        if (nma.hasPath && nma.remainingDistance <= distanceThresh)
        {
            GetComponent<AnimationSync>().enabled = false;
        }
    }
    private void Move()
    {
        if (activated)
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
            }
            else
            {
                anim.SetFloat("jump", 0);
            }

            rotation += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            if (moveVertical == 0)
            {
                transform.eulerAngles = new Vector3(0, rotation, 0);
            }
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
        GetComponent<AnimationSync>().enabled = true;
        GetComponent<NavMeshAgent>().destination = t;
        //anim.SetBool("move", true);
        dest = t;
    }
    public Vector3 Destdir()
    {
        return dest;
    }
    // Decelerate. If value is lower than 0, set to 0. 
    /*    private void Decelerate(float val)
        {
            if (agent.speed > 0)
            {
                agent.speed -= val;
            }
            else if (agent.speed < 0)
            {
                agent.speed = 0;
            }
        }
        // Accelerate. If value is higher than max speed, cap it. 
        private void Accelerate(float val)
        {
            if (agent.speed < agentSpeed)
            {
                agent.speed += val;
            }
            else if (agent.speed > agentSpeed)
            {
                agent.speed = agentSpeed;
            }
        }*/
    // Checks if agent near other agents, stops it if it is.
    /*  private void CheckDist()
      {
          agents.ForEach(delegate (GameObject a)
          {
              if (a.GetComponent<AgentController>().Picked() == true)
              {
                  float distToOtherAgent = DistToTarget(agent.gameObject, a.transform.position);
                  //Debug.Log(distToOtherAgent);
                  if (distToOtherAgent < brakingDistance)
                  {

                      //Debug.Log("Agent " + distToOtherAgent + " units away, SLOWING DOWN");
                      Decelerate(intensity);

                  }
                  else
                  {
                      Accelerate(agent.speed);
                  }
              }
          });

          float distanceToTarget = DistToTarget(agent.gameObject, target);

          //If distance is less than threshold, initiate braking protocol.
          if (distanceToTarget <= brakingDistance)
          {
              Decelerate(intensity);
          }
          // If agent exits range, accelerate
          else if (distanceToTarget > brakingDistance)
          {
              Accelerate(intensity);
          }
      }*/
    private float DistToTarget(GameObject obj, Vector3 target)
    {
        return Vector3.Distance(obj.transform.position, target);
    }
}
