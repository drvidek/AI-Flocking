using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float spd;
    public float power;
    public float scale;
    Transform _player;

    virtual protected void Start()
    {
        _player = GameObject.Find("Player").transform;
    }

    protected void Update()
    {
        if (!GameManager.IsPaused())
        {
            Move();
            if (Vector3.Distance(transform.position, _player.position) > 100f)
            {
                StartCoroutine(EndOfLife());
            }
        }
    }

    protected virtual void Move()
    {
        Vector3 _dest = transform.position + (direction.normalized * spd * Time.deltaTime);

        float _dist = Vector3.Distance(transform.position, _dest);

        RaycastHit2D _rayHit;

        if (_rayHit = Physics2D.CircleCast(transform.position, scale, direction, _dist))
        {
            Debug.Log("Raycast hit");

            if (Collision(_rayHit.collider))
            {
                Debug.Log("Checked Collision");

                _dest = _rayHit.point;
            }
        }

        transform.position = _dest;
    }

    virtual protected bool Collision(Collider2D _hit)
    {
        Debug.Log("Start collision check");
        if (_hit.tag != tag)
        {
            Debug.Log("Hit something valid");
            CombatAgent _hitAgent = _hit.GetComponent<CombatAgent>();
            if (_hitAgent != null)
            {
                Debug.Log("Hit agent");
                _hitAgent.TakeDamage(power);
            }
            StartCoroutine(EndOfLife());
            return true;
        }
        else
            return false;
    }

    protected IEnumerator EndOfLife()
    {

        //direction = Vector3.zero;
        //spd = 0;
        //float _newScale = 1f;
        //float _timer = _trail != null ? _trail.time * 2f : 0;

        //while (_newScale > 0 && _timer > 0)
        //{
        //    _newScale = MathExt.Approach(_newScale, 0, Time.deltaTime / (_trail.time * 2f));
        //    transform.localScale = new Vector3(scale * _newScale, scale * _newScale, scale * _newScale);
        //    yield return null;
        //}
        Object.Destroy(this.gameObject);
        yield return null;
    }
}
