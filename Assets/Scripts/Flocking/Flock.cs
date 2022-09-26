using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    public List<FlockAgent> agents;// = new List<FlockAgent>();
    public ObjectPool<FlockAgent> agentPool;
    public FlockBehaviour behavior;

    public int spawnCountInit;
    public int spawnCountCurrent;
    public int spawnMax;
    public float agentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighbourRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    public float spawnIncrease = 1f;

    public Transform agentPoolLoc;

    public List<Transform> spawnPoints;

    float _squareMaxSpeed { get { return maxSpeed * maxSpeed; } }
    float _squareNeighbourRadius { get { return neighbourRadius * neighbourRadius; } }
    float _squareAvoidanceRadius { get { return _squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier; } }

    public float SquareAvoidanceRadius { get { return _squareAvoidanceRadius; } }

    [SerializeField] private GameObject[] _warningIcons;


    private void Start()
    {
        agentPool = new ObjectPool<FlockAgent>(() =>
            {
                return Instantiate(agentPrefab, agentPoolLoc.position, Quaternion.Euler(Vector3.forward * Random.Range(0, 360f)), transform).GetComponent<FlockAgent>();
            }, newAgent =>
            {
                newAgent.gameObject.SetActive(true);
                newAgent.Initialise(this);
                agents.Add(newAgent);
            }, newAgent =>
            {
                newAgent.AgentCollider.enabled = false;
                newAgent.transform.position = agentPoolLoc.position;
                newAgent.gameObject.SetActive(false);
            }, newAgent =>
             {
                 Destroy(newAgent.gameObject);
             }, false, spawnMax
            );
        Initialise();
    }

    public void Initialise()
    {
        ClearAgents();
        int i = GetSpawnLocation(false);
        spawnCountCurrent = spawnCountInit;
        SpawnAgents(i, spawnCountCurrent);
    }

    void SpawnAgents(int index, int count)
    {
        Transform _spawnpoint = spawnPoints[index];
        for (int i = 0; i < count; i++)
        {
            FlockAgent newAgent = agentPool.Get();
            newAgent.transform.position = _spawnpoint.position + (Vector3)(Random.insideUnitCircle * spawnCountCurrent * agentDensity);
            newAgent.AgentCollider.enabled = true;
        }
    }

    int GetSpawnLocation(bool random)
    {
        int i;
        if (random)
            i = Random.Range(0, spawnPoints.Count);
        else
            i = 0;
        return i;
    }

    private void Update()
    {
        if (GameManager.IsPlaying())
        {
            if (agents.Count == 0)
            {
                spawnCountCurrent = Mathf.Min((int)Mathf.Round((float)spawnCountCurrent * spawnIncrease), spawnMax);
                int i = GetSpawnLocation(true);
                SpawnAgents(i, spawnCountCurrent);
                Animator animator = _warningIcons[i].GetComponentInChildren<Animator>();
                animator.SetTrigger("Flash");
            }
            else
                foreach (FlockAgent agent in agents)
                {
                    List<Transform> context = GetNearbyObjects(agent);

                    //FOR TESING
                    //agent.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

                    Vector2 move = behavior.CalculateMove(agent, context, this);

                    move *= driveFactor;
                    if (move.sqrMagnitude > _squareMaxSpeed)
                    {
                        move = move.normalized * maxSpeed;
                    }

                    agent.Move(move);
                }
        }
    }


    private List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighbourRadius * agent.transform.lossyScale.x);
        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }

        return context;
    }


    public string SaveAgentTransform()
    {
        string data = "|";

        bool _first = true;

        foreach (FlockAgent agent in agents)
        {
            //first entry doesn't need a ~ to start
            if (_first)
                data = data + agent.transform.position + ":" + agent.transform.up + ":" + agent.Health;
            else
                data = data + "~" + agent.transform.position + ":" + agent.transform.up + ":" + agent.Health;
            _first = false;
        }

        return data;
    }

    public void LoadAgents(string[] loadAgents)
    {
        ClearAgents();
        for (int i = 0; i < loadAgents.Length; i++)
        {
            string[] agentTransform = loadAgents[i].Split(':');

            Vector3 agentPos = MathExt.StringToVector3(agentTransform[0]);

            FlockAgent newAgent = Instantiate( //creates a clone of gameobject or prefab
                agentPrefab, // this is the prefab
                agentPos,
                new Quaternion(0, 0, 0, 0),
                transform
                );

            newAgent.transform.up = MathExt.StringToVector3(agentTransform[1]);
            newAgent.Health = float.Parse(agentTransform[2]);
            newAgent.name = "Agent " + i;
            newAgent.Initialise(this);
            agents.Add(newAgent);

        }
    }

    public void ClearAgents()
    {
        foreach (FlockAgent agent in agents)
        {
            agentPool.Release(agent);
        }

        agents.Clear();
    }
}