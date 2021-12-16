using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCamera : MonoBehaviour
{
	public enum TrackingTarget
	{
		Object,
		MainCamera,
		Player
	}
	public TrackingTarget targetType;
	public Transform target;

	public bool checkBoundary;
	public Rect boundary;

	private Camera camera_;

	private void Start()
	{
		camera_ = GetComponent<Camera>();
	}

	private void UpdatePosition()
	{
		if (target == null)
		{
			if (targetType == TrackingTarget.Player)
			{
				GameObject gameobj = ModuleManager.GetInstance().GetModule<GameModule>().player;
				if (gameobj != null)
					target = gameobj.transform;
			}
			else if (targetType == TrackingTarget.MainCamera)
			{
				target = Camera.main.transform;
			}
			else
			{
				Debug.LogError("Target Object is null");
				Destroy(this.gameObject);
			}

			if (target == null)
			{
				return;
			}
		}

		Vector3 targetPosition;
		if (checkBoundary)
		{
			float camheight = camera_.orthographicSize * 2;
			float camwidth = camheight * camera_.aspect;
			targetPosition = new Vector3(
				Mathf.Clamp(target.transform.position.x, boundary.x + camwidth / 2, (boundary.x + boundary.width) - camwidth / 2),
				Mathf.Clamp(target.transform.position.y, (boundary.y - boundary.height) + camheight / 2, boundary.y - camheight / 2),
				transform.position.z);
		}
		else
		{
			targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
		}

		transform.position = Vector3.Lerp(transform.position, targetPosition, 10f * Time.deltaTime);
	}
	private void Update()
	{
		UpdatePosition();
	}
	private void LateUpdate()
	{
		UpdatePosition();
	}
}
