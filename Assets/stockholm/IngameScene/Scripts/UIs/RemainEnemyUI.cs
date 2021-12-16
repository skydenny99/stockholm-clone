using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 현재 남은 적의 수를 표시해주는 UI입니다.
/// </summary>
public class RemainEnemyUI : UI
{
	public Image remainEnemyIcon;
	public NumTextUI remainNumText;

	public enum Mode { Day, Night };
	private Mode mode_ = Mode.Day;

	/// <summary>
	/// 남은 수를 변경합니다.
	/// </summary>
	/// <param name="num">변경할 남은 수입니다.</param>
	public void ChangeRemainNum(int num)
	{
		remainNumText.ChangeNumber(num);
	}

	/// <summary>
	/// 모드를 변경합니다. (낮 / 밤)
	/// </summary>
	/// <param name="mode">변경할 모드입니다.</param>
	public void ChangeMode(Mode mode)
	{
		this.mode_ = mode;

		switch (mode)
		{
			case Mode.Day:
				remainEnemyIcon.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 0), 0.5f);
				break;

			case Mode.Night:
				remainEnemyIcon.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 180f), 0.5f);
				break;

			default:
				break;
		}
	}
}
