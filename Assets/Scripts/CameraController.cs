using UnityEngine;

public class CameraController : MonoBehaviour
{


    private float lookSpeed, mouseX, mouseY;
    public float moveSpeed;
    private float dist;
    private float yAngleMin;
    private float yAngleMax;

    //public GameController gameController;
    private GameObject target;
    private bool hasTarget;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        dist = 7f;
        yAngleMin = 20f;
        yAngleMax = 50f;
        lookSpeed = 4.0f;
        hasTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX += lookSpeed * Input.GetAxis("Mouse X");
        mouseY -= lookSpeed * Input.GetAxis("Mouse Y");

        /*       if (Input.GetKey(KeyCode.LeftAlt))
               {
                   ExitTarget();

               }
               if (Input.GetKey(KeyCode.LeftControl))
               {
                   gameController.RemoveAgents();
               }
               */
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
        if (Input.GetMouseButtonDown(0))
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
            }
        }
    }

    public void SetTarget(GameObject t)
    {
        target = t;
        t.GetComponent<CharacterControllerScript>().activated = true;
        hasTarget = true;
        anim = t.GetComponent<Animator>();
        anim.SetBool("move", true);

    }
    public void ExitTarget()
    {
        if (target)
        {
            target.GetComponent<CharacterControllerScript>().activated = false;
            target = null;
            hasTarget = false;
            anim.SetBool("move", false);
            anim = null;
        }

    }
}
