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

    protected bool isWrappingX = false;
    protected bool isWrappingY = false;

    public bool IsWrapping { get { return (isWrappingX || isWrappingY); } }

    virtual protected void Start()
    {
        _health = _healthMax;
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

    virtual protected void ScreenWrap()
    {
        bool isVisible = _spriteRenderer.isVisible;

        if (isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if (isWrappingX && isWrappingY)
        {
            return;
        }

        Camera cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);
        var newPosition = transform.position;

        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;

            isWrappingX = true;
        }

        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;

            isWrappingY = true;
        }

        transform.position = newPosition;
    }

    IEnumerator HitEffect()
    {
        _spriteRenderer.color = _hitEffectCol;
        yield return new WaitForSecondsRealtime(0.05f);
        _spriteRenderer.color = _homeCol;
    }


}
