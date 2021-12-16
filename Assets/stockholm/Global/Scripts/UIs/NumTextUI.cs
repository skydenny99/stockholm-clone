using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 숫자 텍스트를 표시하는 UI입니다.
/// </summary>
[RequireComponent(typeof(Text))]
public class NumTextUI : UI
{
	public bool hasComma = false;
	public int minLength = -1;

	private int number_;
	private int currentNumber_;
	private int numberGap_;

	// Unity 내장 함수
	void Awake()
	{
		StartCoroutine(UpdateNumber());
	}

	/// <summary>
	/// 주기적으로 숫자를 바꿔줍니다.
	/// </summary>
	private IEnumerator UpdateNumber()
	{
		while(true) {
			if (currentNumber_ != number_)
			{
				if ((numberGap_ > 0 && currentNumber_ + numberGap_ >= number_) || (numberGap_ < 0 && currentNumber_ + numberGap_ <= number_) || numberGap_ == 0)
					currentNumber_ = number_;
				else
					currentNumber_ += numberGap_;

				ChangeText();
			}
			yield return new WaitForSeconds(0.04f);
		}
	}

	/// <summary>
	/// 숫자를 바꿉니다.
	/// </summary>
	/// <param name="num">바꿀 숫자입니다.</param>
	/// <param name="changeImmediately"><c>true</c>일경우, 숫자를 바로 바꿉니다.</param>
	public void ChangeNumber(int num, bool changeImmediately = false)
	{
		this.number_ = num;
		numberGap_ = (num - currentNumber_) / 10;

		if (changeImmediately == true)
			currentNumber_ = num;

		ChangeText();
	}

	/// <summary>
	/// 텍스트를 바꿉니다.
	/// </summary>
	private void ChangeText()
	{
		string numString;
		if (minLength == -1)
			numString = currentNumber_.ToString();
		else
			numString = currentNumber_.ToString("D" + minLength);

		if (!hasComma)
			this.GetComponent<Text>().text = numString;
		else {
			// TODO: 차후 구현
		}
	}
}
