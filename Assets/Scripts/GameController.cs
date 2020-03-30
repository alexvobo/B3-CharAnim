using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{

    private List<GameObject> agents;
    // Start is called before the first frame update
    void Start()
    {
        agents = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public int NumAgents()
    {
        return agents.Count;
    }
    public List<GameObject> GetAgents()
    {
        return agents;
    }
    public void AddAgent(GameObject agent)
    {
        agents.Add(agent);
    }
    public void RemoveAgents(GameObject agent = null)
    {
        if (agent)
        {
            agents.Remove(agent);
        }
        else
        {
            agents.Clear();
        }
    }
    public void MoveAgents(Vector3 pos)
    {
        foreach (GameObject agent in agents)
        {
            if (agent)
                MoveAgent(agent, pos);

        }
    }

    // Moves agent to desired vector.
    public void MoveAgent(GameObject agent, Vector3 t)
    {
        agent.GetComponent<NavMeshAgent>().destination = t;
    }
}
