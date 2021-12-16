using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legarcy
{
	[System.Obsolete("이 클래스는 더이상 사용하지 않습니다.")]
	public class MonsterSampleLegacy : MonoBehaviour, ILegacyCreature
	{
		public float HP = 100f;
		public float dmg = 10f;
		public float push = 3f;
		public GameObject hitbox;
		//public HitBox box;

		// Use this for initialization
		void Start()
		{
			//box = hitbox.GetComponent<HitBox>();

		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.K))
				attack();
		}


		public void attack(bool isMelee = true)
		{
			/*if (box.checkHitBox())
			{
				Collider2D[] cols = box.getCols();
				foreach (Collider2D col in cols)
				{
					if (col.tag.Equals("Player"))
					{
						col.transform.FindChild("Character Controller").GetComponent<Character>().getDamaged(transform.root, dmg, push);
						//col.gameObject.GetComponent<Rigidbody2D>().AddForce()
					}
				}
			}*/
		}

		public void die()
		{
			throw new NotImplementedException();
		}

		public void getDamaged(Transform t, float dmg, float knockBack)
		{
			HP -= dmg;
			playKnockBack(t, knockBack);
			Debug.Log("Monster Ouch");
		}

		public void playKnockBack(Transform t, float knockBack)
		{
			float dist = t.position.x - transform.root.position.x;
			transform.root.gameObject.GetComponent<Rigidbody2D>().velocity = (Vector2.left * dist).normalized * knockBack;
		}

		public void move(float axis)
		{
			throw new NotImplementedException();
		}


	}
}