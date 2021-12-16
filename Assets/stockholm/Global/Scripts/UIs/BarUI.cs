using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Bar UI입니다.
/// </summary>
public class BarUI : UI
{
	public RectTransform barImage;

	public int currentValue = 100;
	public bool isHasMaxValue = true;
	public int maxValue = 100;
	public float widthPerValue = 50f;

	private float barWidth_;

	// Unity 내장 함수
	void Awake()
	{
		barWidth_ = barImage.sizeDelta.x;
	}
	// Unity 내장 함수
	void Update()
	{
		// TODO: Bar 변화를 부드럽게
		UpdateBar();
	}

	/// <summary>
	/// Bar의 현재값을 바꿉니다.
	/// </summary>
	/// <param name="value">바꿀 값입니다.</param>
	public void ChangeValue(int value)
	{
		currentValue = value;
		UpdateBar();
	}

	/// <summary>
	/// Bar의 최댓값을 바꿉니다.
	/// isHasMaxValue가 false인 경우, 이 함수를 실행해도 변화가 없습니다.
	/// </summary>
	/// <param name="maxValue">바꿀 최댓값입니다.</param>
	public void ChangeMaxValue(int maxValue)
	{
		this.maxValue = maxValue;
		UpdateBar();
	}

	/// <summary>
	/// Bar를 값에 맞게 갱신합니다.
	/// </summary>
	private void UpdateBar()
	{
		if (isHasMaxValue)
		{
			float percent = (float)currentValue / maxValue;
			barImage.sizeDelta = new Vector2(barWidth_ * percent, barImage.sizeDelta.y);
		}
		else
		{
			barImage.sizeDelta = new Vector2(currentValue * widthPerValue, barImage.sizeDelta.y);
		}
	}
}
