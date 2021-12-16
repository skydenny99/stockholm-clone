using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ProjectileHitCallback(Creature caller, IngameObject target);

public interface IProjectile : IMovable
{
	void Fire(float speed, float range, Vector2 direction, LayerMask hitLayer, Creature caller, ProjectileHitCallback hitCallback);
}
