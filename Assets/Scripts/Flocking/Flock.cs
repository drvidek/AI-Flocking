using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    public List<FlockAgent> agents;// = new List<FlockAgent>();

    public FlockBehaviour behavior;

    [Range(1, 500)]
    public int spawnCount = 250;
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

    public List<Transform> spawnPoints;

    float _squareMaxSpeed { get { return maxSpeed * maxSpeed; } }
    float _squareNeighbourRadius { get { return neighbourRadius * neighbourRadius; } }
    float _squareAvoidanceRadius { get { return _squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier; } }

    public float SquareAvoidanceRadius { get { return _squareAvoidanceRadius; } }

    [SerializeField] private GameObject[] _warningIcons;


    private void Start()
    {
        int i = GetSpawnLocation(false);
        SpawnAgents(i, spawnCount);
    }

    void SpawnAgents(int index, int count)
    {
        Transform _spawnpoint = spawnPoints[index];
        for (int i = 0; i < count; i++)
        {
            FlockAgent newAgent = Instantiate( //creates a clone of gameobject or prefab
                agentPrefab, // this is the prefab
                _spawnpoint.position + (Vector3)(Random.insideUnitCircle * spawnCount * agentDensity),
                Quaternion.Euler(Vector3.forward * Random.Range(0, 360f)),
                transform
                );
            newAgent.name = "Agent " + i;
            newAgent.Initialise(this);
            agents.Add(newAgent);
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
                spawnCount = (int)Mathf.Round((float)spawnCount * spawnIncrease);
                int i = GetSpawnLocation(true);
                SpawnAgents(i, spawnCount);
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

            FlockAgent newAgent = Instantiate( //creates a clone of gameobject or prefab
                agentPrefab, // this is the prefab
                transform
                );

            newAgent.transform.position = MathExt.StringToVector3(agentTransform[0]);
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
            Destroy(agent.gameObject);
        }

        agents.Clear();
    }
}