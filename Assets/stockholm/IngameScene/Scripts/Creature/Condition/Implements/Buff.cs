﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : Effect {
    protected override void Awake()
    {
        isBuff = true;
        base.Awake();
    }
}
