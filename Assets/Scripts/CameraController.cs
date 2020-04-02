using UnityEngine;

public class CameraController : MonoBehaviour
{


    private float lookSpeed, mouseX, mouseY;
    public float moveSpeed;
    private float dist;
    private float yAngleMin;
    private float yAngleMax;

    private GameObject target;
    private GameObject agent;
    private bool hasTarget;
    private Animator anim;
    float fovMin = 15f;
    float fovMax = 90f;
    float wheelZoomSens = 10f;
    // Start is called before the first frame update
    void Start()
    {
        dist = 7f;
        yAngleMin = 20f;
        yAngleMax = 50f;
        lookSpeed = 2.0f;
        hasTarget = false;
        agent = GameObject.FindGameObjectWithTag("Agent");
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
        if (hasTarget)
        {
            ThirdPerson(target);
        }
        else
        {
            FreeLook();
        }
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
        float clicked = 0;
        float clicktime = 0;
        float clickdelay = 0.5f;



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
                    //gameController.AddAgent(objectHit.gameObject);
                    SetTarget(objectHit.gameObject);

                }
                else
                {
                    ExitTarget();
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (agent)
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
                        agent.GetComponent<CharacterControllerScript>().moveSpeed = .5f;
                    }

                    if (clicked > 1 && Time.time - clicktime < clickdelay)
                    {
                        clicked = 0;
                        clicktime = 0;
                        agent.GetComponent<CharacterControllerScript>().moveSpeed = 1;
                        Debug.Log("Double CLick: ");

                    }
                    else if (clicked > 2 || Time.time - clicktime > 1)
                    {
                        clicked = 0;
                        return;
                    }

                    Debug.Log("Sending agents to destination " + hit.transform);

                    agent.GetComponent<CharacterControllerScript>().MoveAgent(hit.point);

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
