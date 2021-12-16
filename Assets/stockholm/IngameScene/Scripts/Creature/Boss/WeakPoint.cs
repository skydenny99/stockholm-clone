using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour, ILegacyCreature
{
    public GameObject bossController;
    private BuildingBoss _boss;

    void Start()
    {
        _boss = bossController.GetComponent<BuildingBoss>();
    }
    
    public void attack(bool isMelee)
    {
        throw new NotImplementedException();
    }

    public void die()
    {
    }

    public void getDamaged(Transform t, float dmg, float knockBack)
    {
        _boss.getHit(dmg);
    }


    public void move(float axis)
    {
        throw new NotImplementedException();
    }

    public void playKnockBack(Transform t, float knockBack)
    {
        throw new NotImplementedException();
    }
}
