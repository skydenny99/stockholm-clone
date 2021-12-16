using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 게임 도중에 나오는 텍스트들을 관리하는 UI입니다.
/// Ex) 낮/밤이 되었습니다, Game Over...
/// </summary>
public class PopupTextWindowUI : WindowUI
{
	// 여기는 일단 Panel별로 관리하지 않음
	public RectTransform gameOverText;
	public RectTransform dayStartText;
	public RectTransform nightStartText;

	private Vector2 gameOverTextOriginPos;
	private Vector2 dayStartTextOriginPos;
	private Vector2 nightStartTextOriginPos;

	// Unity 내장 함수
	new void Awake()
	{
		base.Awake();

		// 이 Window는 항상 보여짐
		this.GetComponent<CanvasGroup>().alpha = 1f;

		gameOverText.GetComponent<CanvasGroup>().alpha = 0f;
		dayStartText.GetComponent<CanvasGroup>().alpha = 0f;
		nightStartText.GetComponent<CanvasGroup>().alpha = 0f;

		gameOverTextOriginPos = gameOverText.anchoredPosition;
		dayStartTextOriginPos = dayStartText.anchoredPosition;
		nightStartTextOriginPos = nightStartText.anchoredPosition;
	}

	// 이 Window는 WindowUI에 정의된 Show, Hide를 쓰면 안 됨. 그래서 막아둠
	// 대신에 밑에 있는 함수들을 써야 됨
	public override void Show(float duration = 0.6F)
	{
		Debug.Log("You cannot call Show function in PopupTextWindowUI");
	}
	public override void Hide(float duration = 0.6F)
	{
		Debug.Log("You cannot call Hide function in PopupTextWindowUI");
	}

	/// <summary>
	/// GameOverText를 표시합니다.
	/// </summary>
	public void ShowGameOverText()
	{
		MoveText(gameOverText, MoveDirection.TopToBottom, gameOverTextOriginPos, 100f);
	}

	/// <summary>
	/// Day Start를 표시합니다.
	/// </summary>
	public void ShowDayStartText()
	{
		MoveText(dayStartText, MoveDirection.LeftToRight, dayStartTextOriginPos, 400f);
	}

	/// <summary>
	/// Night Start를 표시합니다.
	/// </summary>
	public void ShowNightStartText()
	{
		MoveText(nightStartText, MoveDirection.LeftToRight, nightStartTextOriginPos, 400f);
	}

	private enum MoveDirection { TopToBottom, BottomToTop, LeftToRight, RightToLeft };
	/// <summary>
	/// 텍스트를 해당 방향으로 보여주고 사라지게 됩니다.
	/// </summary>
	/// <param name="textTransform">움직일 텍스트의 RectTransform입니다.</param>
	/// <param name="direction">움직일 방향입니다.</param>
	/// <param name="originAnchorPos">해당 텍스트의 원래 위치입니다.</param>
	/// <param name="distance">움직일 거리입니다.</param>
	private void MoveText(RectTransform textTransform, MoveDirection direction, Vector2 originAnchorPos, float distance)
	{
		textTransform.DOKill();
		textTransform.GetComponent<CanvasGroup>().DOKill();

		Sequence seq = DOTween.Sequence();

		Vector2 initPos, moveFastPos, moveSlowPos, endPos;

		switch (direction)
		{
			case MoveDirection.TopToBottom:
				initPos = originAnchorPos + Vector2.up * distance * 2;
				moveFastPos = originAnchorPos + Vector2.up * distance;
				moveSlowPos = originAnchorPos + Vector2.down * distance;
				endPos = originAnchorPos + Vector2.down * distance * 2;
				break;

			case MoveDirection.BottomToTop:
				initPos = originAnchorPos + Vector2.down * distance * 2;
				moveFastPos = originAnchorPos + Vector2.down * distance;
				moveSlowPos = originAnchorPos + Vector2.up * distance;
				endPos = originAnchorPos + Vector2.up * distance * 2;
				break;

			case MoveDirection.LeftToRight:
				initPos = originAnchorPos + Vector2.left * distance * 2;
				moveFastPos = originAnchorPos + Vector2.left * distance;
				moveSlowPos = originAnchorPos + Vector2.right * distance;
				endPos = originAnchorPos + Vector2.right * distance * 2;
				break;

			case MoveDirection.RightToLeft:
				initPos = originAnchorPos + Vector2.right * distance * 2;
				moveFastPos = originAnchorPos + Vector2.right * distance;
				moveSlowPos = originAnchorPos + Vector2.left * distance;
				endPos = originAnchorPos + Vector2.left * distance * 2;
				break;

			default:
				return;
		}
		
		textTransform.GetComponent<CanvasGroup>().alpha = 0;
		textTransform.anchoredPosition = initPos;

		seq.Append(textTransform.GetComponent<CanvasGroup>().DOFade(1f, 0.2f).SetEase(Ease.Linear));
		seq.Join(textTransform.DOAnchorPos(moveFastPos, 0.2f).SetEase(Ease.Linear));

		seq.Append(textTransform.DOAnchorPos(moveSlowPos, 3f).SetEase(Ease.Linear));

		seq.Append(textTransform.GetComponent<CanvasGroup>().DOFade(0f, 0.2f).SetEase(Ease.Linear));
		seq.Join(textTransform.DOAnchorPos(endPos, 0.2f).SetEase(Ease.Linear));

		seq.Play();
	}
}
