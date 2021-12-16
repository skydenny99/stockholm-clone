using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassReactableObject : AnimationReactableObject
{
    public override void React(Collider2D col)
    {

        float dist = transform.position.x - col.transform.position.x;
        if(col.GetComponent<Creature>())
        if(dist > 0)
        {
            anim.SetTrigger("Right Bend");
        }
        else
        {
            anim.SetTrigger("Left Bend");
        }
    }
    
    private void resetAllTriggers()
    {
        anim.ResetTrigger("Left Bend");
        anim.ResetTrigger("Right Bend");
    }
}
