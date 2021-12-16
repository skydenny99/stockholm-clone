using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 가장 가까운 몬스터가 어디에 있는지 표시하는 UI입니다.
/// </summary>
public class MonsterTrackingUI : UI
{
	/// <summary>
	/// 해당 방향으로 돌립니다.
	/// </summary>
	/// <param name="angle">돌릴 각도입니다.</param>
	public void RotateUI(float angle)
	{
		this.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, angle);
	}
}
