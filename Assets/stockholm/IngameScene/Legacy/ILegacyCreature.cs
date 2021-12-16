using UnityEngine;

public interface ILegacyCreature
{
	void move(float axis);
	void attack(bool isMelee);
	void getDamaged(Transform t, float dmg, float knockBack);
	void playKnockBack(Transform t, float knockBack);
	void die();
}