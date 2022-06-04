using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : CombatAgent
{
    [SerializeField] private float _spdMax = 10f, _fric = 1f, _turnSpd = 200f, _recoil = 2f;
    private bool _shot;
    CircleCollider2D _myCollider;
    public bool Shot { set { _shot = value; } }
    [SerializeField] private Vector2 _myVelocity;

    new void Start()
    {
        base.Start();
        HandleKeybindFile.ReadSaveFile();
        _myCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Turn());
        _myVelocity += Thrust();
        _myVelocity += Recoil();

        Move((Vector3)_myVelocity * Time.deltaTime);

        //transform.position += (Vector3)_myVelocity * Time.deltaTime;

        _myVelocity.x = Mathf.Clamp(_myVelocity.x, -_spdMax, _spdMax);
        _myVelocity.y = Mathf.Clamp(_myVelocity.y, -_spdMax, _spdMax);

        _myVelocity.x = MathExt.Approach(_myVelocity.x, 0, _fric * Time.deltaTime);
        _myVelocity.y = MathExt.Approach(_myVelocity.y, 0, _fric * Time.deltaTime);
    }

    Vector3 Turn()
    {
        float _rotZ = (Input.GetKey(KeyBinds.keys["Left"])) ? 1 : (Input.GetKey(KeyBinds.keys["Right"])) ? -1 : 0;
        return new Vector3(0, 0, _rotZ) * _turnSpd * Time.deltaTime;
    }

    Vector2 Thrust()
    {
        bool _thrust = (Input.GetKey(KeyBinds.keys["Forward"]));
        if (_thrust)
            return (Vector2)transform.up * _spdMax * Time.deltaTime;
        else
            return Vector2.zero;
    }

    Vector2 Recoil()
    {
        if (_shot)
        {
            _shot = false;
            return transform.up * -_recoil;
        }
        else
            return Vector2.zero;
    }

    void Move(Vector3 velocity)
    {
        Vector3 _dest = transform.position + velocity;

        float _dist = Vector3.Distance(transform.position, _dest);

        RaycastHit2D _rayHit;

        if (_rayHit = Physics2D.CircleCast(transform.position, _myCollider.radius, velocity.normalized, _dist))
        {
            Debug.Log("Raycast hit");

            if (_rayHit.collider.tag == "Block")
            {
                Debug.Log("Checked Collision");

                _dest = transform.position;
            }
        }

        transform.position = _dest;
    }

    protected override void EndOfLife()
    {
        throw new System.NotImplementedException();
    }
}
