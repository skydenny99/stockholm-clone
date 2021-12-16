using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PauseWindow에 있는 MainPanel에 있는 아티펙트 슬룻입니다.
/// </summary>
public class PauseWindowUIMainPanelArtifactSlot : MonoBehaviour
{
	public Image itemImage;
	public Text nameText;
	public Text contentText;

	/// <summary>
	/// 해당 슬룻의 아티펙트를 바꿉니다.
	/// </summary>
	/// <param name="name">바꿀 아티펙트의 이름입니다.</param>
	/// <param name="content">바꿀 아티펙트의 내용입니다.</param>
	/// <param name="image">바꿀 아티펙트의 이미지입니다.</param>
	public void ChangeArtifact(string name, string content, Sprite image)
	{
		nameText.text = name;
		contentText.text = content;
		this.itemImage.sprite = image;
	}
}
