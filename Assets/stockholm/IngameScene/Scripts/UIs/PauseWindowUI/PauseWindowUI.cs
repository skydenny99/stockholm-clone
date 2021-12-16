using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 일시정지를 했을 때 나오는 Window입니다.
/// </summary>
public class PauseWindowUI : WindowUI
{
	/// <summary>
	/// Window를 보여줍니다.
	/// </summary>
	/// <param name="duration">나타나는데 걸리는 시간입니다.</param>
	public override void Show(float duration = 0.6f)
	{
		base.Show(duration);
		
		ChangePanel<PauseWindowUIMainPanel>();
	}
}
