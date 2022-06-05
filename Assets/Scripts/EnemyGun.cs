using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MasterGun
{
    [SerializeField] private Transform _player;
    [SerializeField] private MasterEnemy _myEnemy;
    [SerializeField] private Transform _myGun;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        _shotDelay = _shotDelayMax;
        _player = GameObject.Find("Player").transform;
        _myEnemy = GetComponentInParent<MasterEnemy>();
    }

    new void Update()
    {
        base.Update();

        _myGun.up = (Vector2)(_player.position - _shotSpawnPoint.position).normalized;
    }

    protected override bool Shoot()
    {
        if (_shotDelay == 0)
        {
            int i = Random.Range(0, 3);
            if (i != 0)
                _shotDelay += _shotDelayMax / (i + 1);
        }
        return _shotDelay == 0;
    }

    protected override Vector3 CalculateDir()
    {
        Vector3 _dir = (_player.position - _shotSpawnPoint.position);
        _dir.Normalize();
        return _dir;
    }
}