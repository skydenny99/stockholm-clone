using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CompanionProgressWindowUI : WindowUI
{
	public BarUI progressBarUI;

	// Window가 띄워졌을 때 있는 위치
	private Vector2 windowPos;

	new void Awake()
	{
		base.Awake();

		windowPos = this.GetComponent<RectTransform>().anchoredPosition;
	}
	/// <summary>
	/// Window를 보여줍니다.
	/// </summary>
	/// <param name="duration">나타나는데 걸리는 시간입니다.</param>
	public override void Show(float duration = 0.6f)
	{
		this.GetComponent<CanvasGroup>().blocksRaycasts = true;
		this.GetComponent<CanvasGroup>().DOFade(1f, duration);

		this.GetComponent<RectTransform>().anchoredPosition = windowPos - new Vector2(0, 20);
		this.GetComponent<RectTransform>().DOAnchorPos(windowPos, duration);

		isActive = true;
	}

	/// <summary>
	/// Window를 숨깁니다.
	/// </summary>
	/// <param name="duration">숨기는데 걸리는 시간입니다.</param>
	public override void Hide(float duration = 0.6f)
	{
		this.GetComponent<CanvasGroup>().blocksRaycasts = false;
		this.GetComponent<CanvasGroup>().DOFade(0f, duration);

		this.GetComponent<RectTransform>().anchoredPosition = windowPos;
		this.GetComponent<RectTransform>().DOAnchorPos(windowPos - new Vector2(0, 20), duration);

		isActive = false;
	}
}
