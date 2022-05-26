using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    public List<FlockAgent> agents;// = new List<FlockAgent>();

    public FlockBehaviour behavior;

    [Range(1, 500)]
    public int startingCount = 250;
    public float agentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighbourRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;


    float _squareMaxSpeed { get { return maxSpeed * maxSpeed; } }
    float _squareNeighbourRadius { get { return neighbourRadius * neighbourRadius; } }
    float _squareAvoidanceRadius { get { return _squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier; } }

    public float SquareAvoidanceRadius { get { return _squareAvoidanceRadius; } }


    private void Start()
    {
        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate( //creates a clone of gameobject or prefab
                agentPrefab, // this is the prefab
                Random.insideUnitCircle * startingCount * agentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0, 360f)),
                transform
                );
            newAgent.name = "Agent " + i;
            newAgent.Initialise(this);
            agents.Add(newAgent);
        }
    }

    private void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            //FOR TESING
            //agent.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            Vector2 move = behavior.CalculateMove(agent, context, this);
            agent.spd = agent.CalculateSpeed(context, agent.spd);

            move *= driveFactor;
            if (move.sqrMagnitude > _squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }

            agent.Move(move * agent.spd);
        }
    }


    private List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighbourRadius);
        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }

        return context;
    }
}