using UnityEngine;
using System;

[Serializable]
public class Axis
{
	public const float POSITIVE = 1.0f;
	public const float NEGATIVE = -1.0f;
	public const float NATURAL = 0.0f;

	public string name;
	public string descriptiveName;
	public string descriptiveNegativeName;

	public KeyCode positiveButton;
	public KeyCode negativeButton;
	public KeyCode alternativePositiveButton;
	public KeyCode alternativeNegativeButton;
	public bool invert = false;

	// 힘 사용여부 (false일 경우 value는 -1, 0, 1로 고정됨)
	public bool useForce = true;

	// 키를 눌렀을 때 작용하는 힘
	public float sensitivity = 2;

	// 0으로 돌아가려는 힘 (항상 작용)
	public float gravity = 1;

	/// <summary>
	/// 현재 axis값을 반환합니다.
	/// </summary>
	public float value
	{
		get
		{
			return value_ * (invert ? -1 : 1);
		}
		set
		{
			value_ = value;
		}
	}

	private float value_;

	/// <summary>
	/// 시간에 따라 axis 값을 변경합니다.
	/// </summary>
	/// <param name="deltaTime">이전 호출로부터 경과된 시간</param>
	public void updateValue(float deltaTime)
	{
		if (useForce)
		{
			updateValueWithForce(deltaTime);
		}
		else
		{
			updateValueWithoutForce();
		}
	}

	private void updateValueWithForce(float deltaTime)
	{
		if (Input.GetKey(positiveButton) || Input.GetKey(alternativePositiveButton))
		{
			value_ += sensitivity * deltaTime;
		}
		if (Input.GetKey(negativeButton) || Input.GetKey(alternativeNegativeButton))
		{
			value_ -= sensitivity * deltaTime;
		}

		value_ -= Mathf.Clamp(value_, -(gravity * deltaTime), (gravity * deltaTime));

		value_ = Mathf.Clamp(value_, NEGATIVE, POSITIVE);
	}

	private void updateValueWithoutForce()
	{
		value_ = NATURAL;

		if (Input.GetKey(positiveButton) || Input.GetKey(alternativePositiveButton))
		{
			value_ += POSITIVE;
		}
		if (Input.GetKey(negativeButton) || Input.GetKey(alternativeNegativeButton))
		{
			value_ += NEGATIVE;
		}
	}
}
