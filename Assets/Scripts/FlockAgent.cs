using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class FlockAgent : MonoBehaviour
{
    Flock _agentFlock;
    public Flock AgentFlock { get => _agentFlock; }
    private Collider2D _agentCollider;
    public Collider2D AgentCollider { get => _agentCollider; }

    void Start() => _agentCollider = GetComponent<Collider2D>();

    public void Initialise(Flock flock)
    {
        _agentFlock = flock;
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity.normalized; //rotate the AI
        transform.position += (Vector3)velocity * Time.deltaTime; //move the AI
    }
}
