using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterControllerScript : MonoBehaviour
{

    private NavMeshAgent nma;
    public float moveSpeed;
    private Vector3 dest;

    void Start()
    {
        moveSpeed = 1;
        nma = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
  
    }

    // Moves agent to desired vector.
    public void MoveAgent(Vector3 t)
    {
        nma.destination = t;
        dest = t;
    }

}
