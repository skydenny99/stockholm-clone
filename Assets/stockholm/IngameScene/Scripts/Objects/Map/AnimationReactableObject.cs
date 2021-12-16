using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationReactableObject : ReactableObject {

    public bool isPlayOneShot = true;
    public bool canBeRestored = false;
    public Animator anim;
    
    new void Start()
    {
        anim = GetComponent<Animator>();
        base.Start();
    }
    
}
