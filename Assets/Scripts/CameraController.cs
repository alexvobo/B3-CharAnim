using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    private float lookSpeed, mouseX, mouseY;
    public float moveSpeed;
    public GameObject target;
    private float dist = 7f;

    private float yAngleMin = 20f;
    private float yAngleMax = 50f;
    void Start()
    {
        lookSpeed = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX += lookSpeed * Input.GetAxis("Mouse X");
        mouseY -= lookSpeed * Input.GetAxis("Mouse Y");
        mouseY = Mathf.Clamp(mouseY, yAngleMin, yAngleMax);
    }
    private void LateUpdate()
    {
        Vector3 thirdPersonDist = new Vector3(0, 0, -dist);
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);

        transform.position = target.transform.position + rotation * thirdPersonDist;
        transform.LookAt(target.transform.position);
    }

   
}
