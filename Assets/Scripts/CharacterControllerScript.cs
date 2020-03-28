using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{

  float rotationSpeed = 50;
  float rotation = 0f;


  Animator anim;
  CharacterController controller;

    void Start()
    {
      anim = GetComponent<Animator>();
      controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {


       float moveVertical = Input.GetAxis ("Vertical");
       float moveHorizontal = Input.GetAxis("Horizontal");
       print(Input.GetAxis ("Horizontal"));

        anim.SetFloat("vertical", moveVertical);
        anim.SetFloat("horizontal", moveHorizontal);

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        // if(Input.GetKeyDown(KeyCode.Space) && stateInfo.nameHash == runStateHash)
        // {
        //     anim.SetTrigger (jumpHash);
        // }



        rotation += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
    //    if(moveHorizontal = 0)
        transform.eulerAngles = new Vector3(0,rotation,0);

    }
}
