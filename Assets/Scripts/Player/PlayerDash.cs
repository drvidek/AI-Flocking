using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    float _power = 2f;
    CircleCollider2D _collider;
    private List<CombatAgent> _hitList = new List<CombatAgent>();

    private void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        Collider2D[] _hits = Physics2D.OverlapCircleAll(transform.position, _collider.radius*1.1f);
        foreach (Collider2D other in _hits)
        {
            CombatAgent _hitAgent = other.GetComponentInParent<CombatAgent>();
            if (!_hitList.Contains(_hitAgent) && other.tag != tag)
            {
                _hitList.Add(_hitAgent);
                if (_hitAgent != null)
                {
                    _hitAgent.TakeDamage(_power);
                    GlobalScore.IncreaseScore(50, (Vector2)_hitAgent.transform.position);
                }
            }
        }
    }
}
