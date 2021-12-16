using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이시간을 표시하는 UI입니다.
/// </summary>
public class PlaytimeUI : UI
{
	public NumTextUI minNumText;
	public NumTextUI secNumText;

	/// <summary>
	/// 보여줄 시간을 바꿔줍니다.
	/// </summary>
	/// <param name="sec">흐른 시간(초)입니다.</param>
	public void ChangeTime(int sec)
	{
		int min = sec / 60;

		minNumText.ChangeNumber(min, true);
		secNumText.ChangeNumber(sec % 60, true);
	}
}
