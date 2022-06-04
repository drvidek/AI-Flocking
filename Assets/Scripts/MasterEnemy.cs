using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterEnemy : CombatAgent
{
    protected override void EndOfLife()
    {
        Destroy(this.gameObject);
    }

}
