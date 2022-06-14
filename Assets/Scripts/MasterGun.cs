using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class MasterGun : MonoBehaviour
{
    [SerializeField] protected CombatAgent _owner;

    [Header("Assets")]
    [SerializeField] protected GameObject _bulletPrefab;
    [SerializeField] protected Transform _shotSpawnPoint;

    [Header("Bullet Stats")]
    [SerializeField] protected int _shotCount = 1;
    [SerializeField] protected int _shotPower = 1;
    [SerializeField] protected int _shotSpeed = 40;
    [SerializeField] protected float _shotSize = 0.2f;
    protected float _shotDelay;
    [SerializeField] protected float _shotDelayMax = 0.3f;
    [SerializeField] protected float _shotRechargeRate = 1f;

    [Header("Audio")]
    [SerializeField] private AudioSource _shotSound;

    public PlayerMain Player { get { return _owner as PlayerMain; } }
    public FlockAgent Enemy { get { return _owner as FlockAgent; } }


    virtual protected void Start()
    {
        _owner = GetComponent<CombatAgent>();
    }

    abstract protected bool Shoot();

    abstract protected Vector3 CalculateDir();

    virtual protected void CreateBullets(int count)
    {
        Vector3 _dir = CalculateDir();
        for (int i = 0; i < count; i++)
        {
            Vector3 _spawnPoint = (_shotSpawnPoint != null ? _shotSpawnPoint.position : transform.position);
            GameObject _prefab = Instantiate(_bulletPrefab, _spawnPoint, new Quaternion(0,0,0,0));
            Bullet _newBullet = _prefab.GetComponent<Bullet>();
            ApplyBulletProperties(_newBullet, _dir, this.tag);
            _shotDelay = _shotDelayMax;
        }
    }

    virtual public void ApplyBulletProperties(Bullet _bullet, Vector3 _dir, string tag)
    {
        _bullet.transform.position = (_shotSpawnPoint != null) ? _shotSpawnPoint.position : transform.position;
        _bullet.direction = _dir;
        _bullet.tag = tag;
        _bullet.spd = _shotSpeed;
        _bullet.power = _shotPower;
        _bullet.scale = _shotSize;
    }

    // Update is called once per frame
    public void Update()
    {
        if (GameManager.IsPlaying())
        {
            if (Shoot())
            {
                CreateBullets(_shotCount);
                _shotSound.Play();
            }
            else
            {
                _shotDelay = MathExt.Approach(_shotDelay, 0f, _shotRechargeRate * Time.deltaTime);
            }
        }
    }
}
