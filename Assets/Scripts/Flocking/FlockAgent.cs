using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class FlockAgent : CombatAgent
{
    Flock _agentFlock;
    public Flock AgentFlock { get => _agentFlock; }
    private Collider2D _agentCollider;
    public ContextFilter filter;
    public Collider2D AgentCollider { get => _agentCollider; }

    private List<Vector2> _pointDir = new List<Vector2>();

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
        _pointDir.Add(velocity.normalized);
        if (_pointDir.Count > 10)
        _pointDir.RemoveAt(0);
        Vector2 _pointDirAverage = new Vector2();
        foreach(Vector2 item in _pointDir)
        {
            _pointDirAverage += item;
        }
        _pointDirAverage /= _pointDir.Count;
        transform.up = _pointDirAverage.normalized; //rotate the AI
        transform.position += (Vector3)velocity * Time.deltaTime; //move the AI

        ScreenWrap();
    }

    protected override IEnumerator EndOfLife()
    {
        _agentFlock.agents.Remove(this);

        CreateDeathParticles();

        GlobalScore.IncreaseScore(100 * (int)_healthMax, (Vector2)transform.position);

        Destroy(this.gameObject);
        yield return null;
    }
}
