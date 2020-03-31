using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class AnimationSync : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
    }

    void Update()
    {
        var destDir = GetComponent<CharacterControllerScript>().Destdir() - transform.position;
        var curDir = transform.forward;
        var angDev = Vector3.SignedAngle(destDir, curDir, Vector3.up) / 180;
        var x_vel = -1 * angDev;
        var y_vel = Mathf.Abs(x_vel) > .25f ? 0 : Mathf.Min(destDir.magnitude, .5f);

        anim.SetFloat("horizontal", agent.desiredVelocity.x);
        anim.SetFloat("vertical", agent.desiredVelocity.z);
/*        bool shouldMove = agent.remainingDistance > agent.radius;
        anim.SetBool("move", shouldMove);*/
        Debug.DrawLine(transform.position, transform.position + agent.desiredVelocity * 10, Color.cyan);
        //activate movement on satisfying condition
        // Map 'worldDeltaPosition' to local space
        /*		float dx = Vector3.Dot(transform.right, worldDeltaPosition);
				float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
				Vector2 deltaPosition = new Vector2(dx, dy);

				// Low-pass filter the deltaMove
				float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
				smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

				// Update velocity if delta time is safe
				if (Time.deltaTime > 1e-5f)
					velocity = smoothDeltaPosition / Time.deltaTime;*/

        //bool shouldMove =  agent.remainingDistance > agent.radius;

        // Update animation parameters


    }

    void OnAnimatorMove()
    {/**/
        // Update postion to agent position
        transform.position = agent.nextPosition;

        // Update position based on animation movement using navigation surface height
        Vector3 position = anim.rootPosition;
        position.y = agent.nextPosition.y;
        transform.position = position;
    }
}
