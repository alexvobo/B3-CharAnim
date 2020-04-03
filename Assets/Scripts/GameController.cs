using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{

    private List<GameObject> agents;
    public GameObject agentPrefab;
    public int numBots;
    private GameObject spawn;
    // Start is called before the first frame update
    void Start()
    {
        spawn = GameObject.FindGameObjectWithTag("spawn");
        agents = new List<GameObject>();
        LoadAgents(spawn);
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
                agent.GetComponent<CharacterControllerScript>().MoveAgent(pos);

        }
    }
    // Randomize a spawn location and return a vector
    private Vector3 GenSpawnPoint(GameObject spawnPoint)
    {
        return new Vector3(spawnPoint.transform.position.x + (Random.insideUnitCircle * 3).x, 1f, spawnPoint.transform.position.z + (Random.insideUnitCircle * 3).x);
    }
    //load #numBots agents at specified spawn
    public void LoadAgents(GameObject spawnPoint)
    {

        //Get random coordinates in spawn object
        GameObject bot_father = new GameObject("Hero");
        for (int i = 0; i < numBots; i++)
        {
            GameObject bot = Instantiate(agentPrefab) as GameObject;
            //bot.GetComponent<LookAt>().head = bot.transform;
            bot.transform.parent = bot_father.transform;
            bot.transform.position = GenSpawnPoint(spawnPoint);
        }
    }
}
