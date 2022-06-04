using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CombatAgent : MonoBehaviour
{
    [Header("Health + Damage")]
    [SerializeField] protected float _healthMax;
    [SerializeField] protected float _health;
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Color _hitEffectCol;
    [SerializeField] protected Color _homeCol;

    virtual protected void Start()
    {
        _health = _healthMax;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _homeCol = _spriteRenderer.color;
    }

    protected abstract void EndOfLife();

    public void TakeDamage(float hit)
    {
        _health -= hit;
        if (_health <= 0)
        {
            EndOfLife();
        }
            StartCoroutine(HitEffect());
    }


    IEnumerator HitEffect()
    {
        _spriteRenderer.color = _hitEffectCol;
        yield return new WaitForSecondsRealtime(0.05f);
        _spriteRenderer.color = _homeCol;
    }


}
