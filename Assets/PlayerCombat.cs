using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : CombatEntity
{
    // Start is called before the first frame update
    void Awake()
    {
        health = 5f;
    }

    public override void Die()
    {
        Debug.Log("player lost");
    }
}
