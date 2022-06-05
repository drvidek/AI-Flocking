using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    CircleCollider2D _collider;

    private void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        Collider2D[] _hits = (Physics2D.OverlapCircleAll(transform.position, _collider.radius));
        if (_hits.Length > 1)
        {
            Debug.Log("Found hits");
            foreach (Collider2D _hit in _hits)
            {
                Debug.Log("Try get combat agent");
                CombatAgent _hitAgent = _hit.GetComponent<CombatAgent>();
                if (_hitAgent != null)
                {
                    Debug.Log("Found combat agent");

                    _hitAgent.TakeDamage(100f);
                }
                else
                {
                    Debug.Log("Try get bullet");
                    Bullet _hitBullet = _hit.GetComponent<Bullet>();

                    if (_hitBullet != null)
                    {
                        Debug.Log("Found bullet");
                        StartCoroutine(_hitBullet.EndOfLife());
                    }
                }
            }
        }
    }
}
