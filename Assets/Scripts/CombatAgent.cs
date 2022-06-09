using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CombatAgent : MonoBehaviour
{
    [Header("Health + Damage")]
    [SerializeField] protected float _healthMax;
    [SerializeField] protected float _health;
    public float Health { get { return _health; } set { _health = value; } }
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Color _hitEffectCol;
    [SerializeField] protected Color _homeCol;
    public GameObject _deathPart;

    protected bool _dead = false;
    public bool Dead { get { return _dead; } }

    protected bool _isWrappingX = false;
    protected bool _isWrappingY = false;

    virtual protected void Start()
    {
        //on first create, fill health
        if (_health == 0)
        _health = _healthMax;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _homeCol = _spriteRenderer.color;
    }

    protected abstract IEnumerator EndOfLife();

    public void TakeDamage(float hit)
    {
        _health -= hit;
        if (_health <= 0)
        {
            StartCoroutine(EndOfLife());
        }
            StartCoroutine(HitEffect());
    }

    virtual protected void ScreenWrap()
    {
        bool isVisible = _spriteRenderer.isVisible;

        if (isVisible)
        {
            _isWrappingX = false;
            _isWrappingY = false;
            return;
        }

        if (_isWrappingX && _isWrappingY)
        {
            return;
        }

        Camera cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);
        var newPosition = transform.position;

        if (!_isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;

            _isWrappingX = true;
        }

        if (!_isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;

            _isWrappingY = true;
        }

        transform.position = newPosition;
    }

    IEnumerator HitEffect()
    {
        _spriteRenderer.color = _hitEffectCol;
        yield return new WaitForSecondsRealtime(0.05f);
        _spriteRenderer.color = _homeCol;
    }

    protected ParticleSystem CreateDeathParticles()
    {
        GameObject _partOb = Instantiate(_deathPart);
        _partOb.transform.position = transform.position;
        ParticleSystem _partSys = _partOb.GetComponent<ParticleSystem>();
        var _main = _partSys.main;
        _main.startColor = _homeCol;
        return _partSys;
    }

}
