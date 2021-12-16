using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 현재 시간을 표시해주는 UI입니다.
/// </summary>
public class TimerUI : UI
{

	public Sprite daySprite;
	public Sprite nightSprite;

	public RectTransform outMask;
	public Image outImage;
	public RectTransform inMask;
	public Image inImage;

	public enum State { Day, Night };
	public State state = State.Day;
	public float statePercent = 1f;

	private float outMaskWidth_;
	private float inMaskWidth_;

	// Unity 내장 함수
	void Awake()
	{
		outMaskWidth_ = outMask.sizeDelta.x;
		inMaskWidth_ = inMask.sizeDelta.x;
	}
	
	public void ChangeState(State state)
	{
		this.state = state;
		UpdateUI();
	}

	public void ChangeStatePercent(float percent)
	{
		statePercent = percent;
		UpdateUI();
	}

	/// <summary>
	/// 타이머의 UI를 갱신합니다.
	/// </summary>
	private void UpdateUI()
	{
		switch (state)
		{
			case State.Day:
				inImage.sprite = nightSprite;
				outImage.sprite = daySprite;
				break;

			case State.Night:
				inImage.sprite = daySprite;
				outImage.sprite = nightSprite;
				break;

			default:
				break;
		}
		inMask.sizeDelta = new Vector2(inMaskWidth_ * statePercent, inMask.sizeDelta.y);
		outMask.sizeDelta = new Vector2(outMaskWidth_ * (1f - statePercent), outMask.sizeDelta.y);
	}
}
