using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legarcy
{
	[System.Obsolete("이 클래스는 더이상 사용하지 않습니다.")]
	public class MonsterSensorLegacy : MonoBehaviour
	{

		public GameObject mController;
		public GameObject mEye;
		public bool isDetectableOverWall = false;
		public bool canDetectBehind = true;

		public Collider2D[] cols;
		public float detectRange;
		public float detectAngle;
		public float detectBehindMinRange;
		private MonsterLegacy control_;
		private Transform transform_;
		private float defaultDetectRange;
		private float defaultDetectAngle;
		private float defaultDetectBehindRange;


		// Use this for initialization
		void Start()
		{
			defaultDetectRange = detectRange;
			defaultDetectAngle = detectAngle;
			defaultDetectBehindRange = detectBehindMinRange;
			control_ = mController.GetComponent<MonsterLegacy>();
			transform_ = transform;
		}

		public void resetDetectArea()
		{
			detectRange = defaultDetectRange;
			detectAngle = defaultDetectAngle;
			detectBehindMinRange = defaultDetectBehindRange;
		}


		// TODO : 땅바닥 체크
		public bool checkFrontGround()
		{
			//정면 체크
			RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(-control_.transform.parent.localScale.x, 0), detectRange, ~(1 << LayerMask.NameToLayer("Monster")));
			Debug.DrawRay(transform.position, new Vector2(-control_.transform.parent.localScale.x, 0), Color.red);
			if (hits != null)
				foreach (RaycastHit2D hit in hits)
				{
					if (hit.transform.tag.Equals("Ground"))
					{
						Debug.Log("앞에 벽이 있소 갈 수 없소");
						return false;
					}
				}
			hits = null;
			//대각선 체크
			hits = Physics2D.RaycastAll(transform.TransformPoint(new Vector3(transform.localPosition.x, 0)), new Vector2(-control_.transform.parent.localScale.x, -(transform.root.GetComponent<BoxCollider2D>().size.y / 2 - transform.localPosition.y)), detectRange, ~(1 << LayerMask.NameToLayer("Monster")));
			Debug.DrawRay(transform.TransformPoint(new Vector3(transform.localPosition.x, 0)), new Vector2(-control_.transform.parent.localScale.x, -(transform.root.GetComponent<BoxCollider2D>().size.y / 2 - transform.localPosition.y)), Color.blue);
			foreach (RaycastHit2D hit in hits)
			{
				if (hit.transform.tag.Equals("Ground"))
				{
					Debug.Log("앞에 바닥이 있어서 갈수있어");
					return true;
				}
			}
			return false;
		}

		public bool findPlayer()
		{
			bool hasFound = false;
			if (detectRange < Vector3.Distance(transform_.position, control_.Player.transform.position))
				hasFound = false;

			Vector3 dir = control_.Player.transform.position - mEye.transform.position;


			if (isDetectableOverWall)
			{
				hasFound = true;
			}
			else
			{
				if (detectAngle > Vector3.Angle(Vector3.left * transform_.root.localScale.x, dir))
				{
					RaycastHit2D hit = Physics2D.Raycast(mEye.transform.position, dir, detectRange, ~(1 << LayerMask.NameToLayer("Monster")));
					//Debug.DrawRay(mEye.transform.position, dir);
					//Debug.Log(hit.transform);
					if (hit.transform != null)
						if (hit.transform.root.tag.Equals("Player"))
							hasFound = true;
				}

			}

			if (canDetectBehind)
			{
				RaycastHit2D hit = Physics2D.Raycast(mEye.transform.position, dir, detectRange, ~(1 << LayerMask.NameToLayer("Monster")));

				if (hit.transform != null)
					if (hit.transform.root.tag.Equals("Player"))
						if (detectBehindMinRange > Vector3.Distance(hit.point, transform_.position))
							hasFound = true;
			}

			return hasFound;
		}


	}
}