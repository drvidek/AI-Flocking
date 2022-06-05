using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float spd;
    public float scale;
    public float power;
    SpriteRenderer _myRenderer;


    virtual protected void Start()
    {
        transform.localScale = new Vector3(scale, scale, 1);
        _myRenderer = GetComponent<SpriteRenderer>();

        _myRenderer.color = tag == "Player" ? Color.white : Color.red;
    }

    protected void Update()
    {
        if (!GameManager.IsPaused())
        {
            Move();

            if (Vector2.Distance(transform.position, Vector2.zero) > 200f)
                StartCoroutine(EndOfLife());
        }
    }

    protected virtual void Move()
    {
        Vector3 _dest = transform.position + (direction.normalized * spd * Time.deltaTime);

        float _dist = Vector3.Distance(transform.position, _dest);

        RaycastHit2D[] _rayHit = Physics2D.CircleCastAll(transform.position, scale, direction, _dist);

        if (_rayHit.Length > 0)
        {
            foreach (RaycastHit2D item in _rayHit)
            {
                if (Collision(item.collider))
                {
                    _dest = item.point;
                    continue;
                }
            }
        }

        transform.position = _dest;
    }

    virtual protected bool Collision(Collider2D _hit)
    {
        if (_hit.tag != tag)
        {
            CombatAgent _hitAgent = _hit.GetComponent<CombatAgent>();
            if (_hitAgent != null)
            {
                _hitAgent.TakeDamage(power);
            }
                StartCoroutine(EndOfLife());
            return true;
        }
        else
            return false;
    }

    public IEnumerator EndOfLife()
    {
        Destroy(this.gameObject);
        yield return null;
    }
}
