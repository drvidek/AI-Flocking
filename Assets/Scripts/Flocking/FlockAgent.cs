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

    public bool hasSpawned;

    private List<Vector2> _pointDir = new List<Vector2>();

    [SerializeField] private AudioSource _hitSound;


    public void Initialise(Flock flock)
    {
        base.Start();
        _agentCollider = GetComponent<Collider2D>();
        _agentFlock = flock;
        _agentCollider.enabled = true;
        hasSpawned = false;
    }

    public void Move(Vector2 velocity)
    {
        _pointDir.Add(velocity.normalized);
        if (_pointDir.Count > 10)
            _pointDir.RemoveAt(0);
        Vector2 _pointDirAverage = new Vector2();
        foreach (Vector2 item in _pointDir)
        {
            _pointDirAverage += item;
        }
        _pointDirAverage /= _pointDir.Count;
        transform.up = _pointDirAverage.normalized; //rotate the AI
        transform.position += (Vector3)velocity * Time.deltaTime; //move the AI

        if (hasSpawned)
            ScreenWrap();
        else
            hasSpawned = _spriteRenderer.isVisible;
    }

    public override void TakeDamage(float hit)
    {
        _hitSound.Play();

        base.TakeDamage(hit);
    }

    protected override void EndOfLife()
    {
        CreateDeathParticles();
        _agentFlock.agents.Remove(this);
        GlobalScore.IncreaseScore(100 * (int)_healthMax, (Vector2)transform.position);
        GlobalScore.IncreaseComboMeter(1);
        _spriteRenderer.color = _homeCol;
        AgentFlock.agentPool.Release(this);
    }
}
