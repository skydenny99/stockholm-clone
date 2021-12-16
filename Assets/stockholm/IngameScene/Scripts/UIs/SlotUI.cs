using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 오른쪽 아래에 아이템/아티펙트를 표시하는 UI입니다.
/// </summary>
public class SlotUI : UI
{
	public Text countNumText;
	public Image itemImage;
	/// <summary>
	/// 개수가 1이여도 슬룻에 숫자를 표시할 것인가?
	/// </summary>
	public bool isShowOneCount = true;

	/// <summary>
	/// 슬룻에 있는 이미지를 바꿔줍니다,
	/// </summary>
	/// <param name="image">바꿀 이미지입니다.</param>
	public void ChangeSlotImage(Sprite image)
	{
		itemImage.sprite = image;
	}

	/// <summary>
	/// 슬룻에 있는 개수를 바꿔줍니다.
	/// </summary>
	/// <param name="num">바꿀 개수입니다.</param>
	public void ChangeSlotNumber(int num)
	{
		if (countNumText != null)
		{
			if (num != 1 || (num == 1 && isShowOneCount == true))
				countNumText.text = num.ToString();
			else
				countNumText.text = "";
		}
	}
}
