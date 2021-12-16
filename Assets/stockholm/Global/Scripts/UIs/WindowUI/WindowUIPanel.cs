using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// WindowUI안에 들어가있는 Panel의 기본 클래스입니다.
/// </summary>
public class WindowUIPanel : UI
{
	protected WindowUI parentWindowUI_;
	protected bool isActive_ = false;

	protected void Awake()
	{
		parentWindowUI_ = this.GetComponentInParent<WindowUI>();
	}

	/// <summary>
	/// 패널을 보여줍니다.
	/// </summary>
	/// <param name="duration">보이는데 걸리는 시간입니다.</param>
	public virtual void Show(float duration = 0.3f)
	{
		this.GetComponent<CanvasGroup>().blocksRaycasts = true;
		this.GetComponent<CanvasGroup>().DOFade(1f, duration);

		isActive_ = true;
	}

	/// <summary>
	/// 패널을 숨겨줍니다.
	/// </summary>
	/// <param name="duration">숨기는데 걸리는 시간입니다.</param>
	public virtual void Hide(float duration = 0.3f)
	{
		this.GetComponent<CanvasGroup>().blocksRaycasts = false;
		this.GetComponent<CanvasGroup>().DOFade(0f, duration);

		isActive_ = false;
	}
}
