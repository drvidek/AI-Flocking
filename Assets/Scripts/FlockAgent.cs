using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class FlockAgent : MonoBehaviour
{
    Flock _agentFlock;
    public Flock AgentFlock { get => _agentFlock; }
    private Collider2D _agentCollider;
    public ContextFilter filter;
    public Collider2D AgentCollider { get => _agentCollider; }
    public float spd;
    public float panic;
    void Start() => _agentCollider = GetComponent<Collider2D>();

    public void Initialise(Flock flock)
    {
        _agentFlock = flock;
    }

    private void Update()
    {
        if (tag == "Sheep")
        {
            panic = MathExt.Approach(panic, 0, Time.deltaTime);
            spd = MathExt.Approach(spd, 0, 0.5f * Time.deltaTime);
        }
        else
            spd = 0;
        if (Input.GetKey(KeyCode.Space))
            spd = 1f;
    }

    public float CalculateSpeed(List<Transform> context, float currentSpd)
    {
        if (context.Count == 0)
        {
            return currentSpd;
        }

        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(this, context);
        foreach (Transform item in filteredContext)
        {
            FlockAgent agent = item.GetComponent<FlockAgent>();
            if (agent != null && agent.spd > currentSpd && agent.spd > 0.5f)
                currentSpd = agent.spd;
        }
        return currentSpd;
    }

    public void Move(Vector2 velocity)
    {
        if (velocity.magnitude > 0)
        transform.up = velocity.normalized; //rotate the AI
        transform.position += (Vector3)velocity * Time.deltaTime; //move the AI
    }
}
