using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class FlockAgent : MasterEnemy
{
    Flock _agentFlock;
    public Flock AgentFlock { get => _agentFlock; }
    private Collider2D _agentCollider;
    public ContextFilter filter;
    public Collider2D AgentCollider { get => _agentCollider; }

    new void Start()
    {
        base.Start();
        _agentCollider = GetComponent<Collider2D>();
    }

    public void Initialise(Flock flock)
    {
        _agentFlock = flock;
    }

    public void Move(Vector2 velocity)
    {
        if (velocity.magnitude > 0)
        transform.up = velocity.normalized; //rotate the AI
        transform.position += (Vector3)velocity * Time.deltaTime; //move the AI
    }

    protected override void EndOfLife()
    {
        _agentFlock.agents.Remove(this);
        Destroy(this.gameObject);
    }
}
