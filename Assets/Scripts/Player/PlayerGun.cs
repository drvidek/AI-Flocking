using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGun : MasterGun
{

    // Start is called before the first frame update
    new void Start()
    {
        _owner = GetComponent<PlayerMain>();
    }

    override protected bool Shoot()
    {
        return _shotDelay == 0 && (Input.GetKey(KeyBinds.keys["Shoot"]));
    }


    override protected Vector3 CalculateDir()
    {
        Vector3 _dir = (transform.up);
        _dir.Normalize();
        return _dir;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (Shoot())
            Player.Shot = true;
    }
}