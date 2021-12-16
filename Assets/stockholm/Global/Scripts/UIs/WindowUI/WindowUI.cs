using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 창과 관련된 UI입니다.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class WindowUI : UI
{
	// Window를 띄우면 뒷부분을 어둡게 해주는 이미지
	public Image blackBackgroundImg;

	public List<WindowUIPanel> panels;
	public bool hasBlurred = true;

	public bool isActive = false;
	
	// 유니티 내장 함수
	protected void Awake()
	{
		if (blackBackgroundImg != null)
		{
			blackBackgroundImg.GetComponent<CanvasGroup>().alpha = 0f;
			blackBackgroundImg.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		this.GetComponent<CanvasGroup>().alpha = 0f;
		this.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	/// <summary>
	/// Window를 보여줍니다.
	/// </summary>
	/// <param name="duration">나타나는데 걸리는 시간입니다.</param>
	public virtual void Show(float duration = 0.6f)
	{
		if (blackBackgroundImg != null)
		{
			blackBackgroundImg.GetComponent<CanvasGroup>().blocksRaycasts = true;
			blackBackgroundImg.GetComponent<CanvasGroup>().DOFade(1f, duration);
		}
		this.GetComponent<CanvasGroup>().blocksRaycasts = true;
		this.GetComponent<CanvasGroup>().DOFade(1f, duration);

		if (hasBlurred == true)
		{
			IngameUIModule im = ModuleManager.GetInstance().GetModule<IngameUIModule>();
			if(im != null)
				im.pauserBlur.ApplyPauseBlur(duration);
		}

		isActive = true;
	}

	/// <summary>
	/// Window를 숨깁니다.
	/// </summary>
	/// <param name="duration">숨기는데 걸리는 시간입니다.</param>
	public virtual void Hide(float duration = 0.6f)
	{
		if (blackBackgroundImg != null)
		{
			blackBackgroundImg.GetComponent<CanvasGroup>().blocksRaycasts = false;
			blackBackgroundImg.GetComponent<CanvasGroup>().DOFade(0f, duration);
		}
		this.GetComponent<CanvasGroup>().blocksRaycasts = false;
		this.GetComponent<CanvasGroup>().DOFade(0f, duration);

		if (hasBlurred == true)
		{
			ModuleManager.GetInstance().GetModule<IngameUIModule>().pauserBlur.DiscardPauseBlur(duration);
		}

		isActive = false;
	}

	/// <summary>
	/// 보여주는 패널을 해당 패널로 바꿔줍니다.
	/// </summary>
	/// <typeparam name="T">바꿀 패널의 클래스입니다.</typeparam>
	public void ChangePanel<T>() where T : WindowUIPanel
	{
		foreach (WindowUIPanel panel in panels)
		{
			if (panel is T)
			{
				panel.Show();
			}
			else
			{
				panel.Hide();
			}
		}
	}
}