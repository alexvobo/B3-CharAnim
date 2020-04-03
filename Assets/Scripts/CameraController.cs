using UnityEngine;

public class CameraController : MonoBehaviour
{


    private float lookSpeed, mouseX, mouseY;
    public float moveSpeed;
    private float dist;
    private float yAngleMin;
    private float yAngleMax;
    private GameController gameController;
    private GameObject target;
    private bool hasTarget;
    private Animator anim;
    private float fovMin;
    private float fovMax;
    private float wheelZoomSens;
    private float clicked;
    private float clicktime;
    private float clickdelay;
    private float agentspeed;
    // Start is called before the first frame update
    void Start()
    {
        fovMin = 15f;
        fovMax = 90f;
        wheelZoomSens = 10f;
        clicked = 0;
        clicktime = 0;
        clickdelay = 0.5f;
        dist = 7f;
        yAngleMin = 20f;
        yAngleMax = 50f;
        lookSpeed = 2.0f;
        hasTarget = false;
        //agent = GameObject.FindGameObjectWithTag("Agent");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();


    }

    // Update is called once per frame
    void Update()
    {
        mouseX += lookSpeed * Input.GetAxis("Mouse X");
        mouseY -= lookSpeed * Input.GetAxis("Mouse Y");

        DetectObjects();

    }

    private void LateUpdate()
    {
        /*   if (hasTarget)
           {
               ThirdPerson(target);
           }
           else
           {*/
        FreeLook();
        /*}*/
    }

    private void ThirdPerson(GameObject target)
    {
        mouseY = Mathf.Clamp(mouseY, yAngleMin, yAngleMax);
        float fov = Camera.main.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * wheelZoomSens;
        fov = Mathf.Clamp(fov, fovMin, fovMax);
        Camera.main.fieldOfView = fov;
        Vector3 thirdPersonDist = new Vector3(0, 0, -dist);
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);
        transform.position = target.transform.position + rotation * thirdPersonDist;
        transform.LookAt(target.transform.position);
    }
    private void FreeLook()
    {
        transform.eulerAngles = new Vector3(mouseY, mouseX, 0.0f);

        Vector3 input = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        transform.position += input * Time.deltaTime * moveSpeed;

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += Vector3.up * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Only if we are above ground
            if (transform.position.y > 0.0f)
            {
                transform.position -= Vector3.up * Time.deltaTime * moveSpeed;
            }
        }
    }

    // Raycasting object selection method
    private void DetectObjects()
    {


        if (Input.GetMouseButtonDown(1))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1500))
            {
                Transform objectHit = hit.transform;
                Debug.DrawRay(hit.point, Vector3.up, Color.red);
                // Set our target if the object hit has a rigidbody
                if (objectHit.CompareTag("Agent"))
                {

                    if (gameController.GetAgents().Contains((objectHit.gameObject)))
                    {
                        // If selected, deselect.
                        gameController.RemoveAgents(objectHit.gameObject);
                    }
                    else
                    {
                        // Select
                        gameController.AddAgent(objectHit.gameObject);
                    }


                }
                else
                {
                    if (gameController.NumAgents() > 0)
                    {
                        Debug.Log("Sending agents to destination " + hit.transform);
                        if (hit.transform)
                        {
                            gameController.MoveAgents(hit.point);
                        }
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            agentspeed = .5f;

            if (gameController.GetAgents().Count > 0)
            {
                clicked++;


                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 1500))
                {
                    Transform objectHit = hit.transform;
                    Debug.DrawRay(hit.point, Vector3.up, Color.red);
                    // Set our target if the object hit has a rigidbody
                    if (clicked == 1)
                    {
                        clicktime = Time.time;
                    }

                    if (clicked > 1 && Time.time - clicktime < clickdelay)
                    {
                        clicked = 0;
                        clicktime = 0;
                        agentspeed = 1f;
                        Debug.Log("Double Cick");

                    }
                    else if (clicked > 2 || Time.time - clicktime > 1)
                    {
                        clicked = 0;
                        return;
                    }

                    foreach (var agent in gameController.GetAgents())
                    {
                        agent.GetComponent<CharacterControllerScript>().moveSpeed = agentspeed;
                    }
                    Debug.Log("Sending agents to destination " + hit.transform);

                    gameController.MoveAgents(hit.point);

                }
            }
        }
    }
    public void SetTarget(GameObject t)
    {
        hasTarget = true;
        //anim = t.GetComponent<Animator>();
        // anim.SetBool("move", true);
        target = t;

    }
    public void ExitTarget()
    {
        if (target)
        {
            hasTarget = false;
            //anim.SetBool("move", false);
            anim = null;
            target = null;
        }

    }
}
