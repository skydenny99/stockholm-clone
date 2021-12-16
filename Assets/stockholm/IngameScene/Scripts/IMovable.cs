using UnityEngine;

/// <summary>
/// 움직일 수 있는 개체에 대한 인터페이스입니다.
/// </summary>
public interface IMovable
{
	void Move(Vector2 offset);
}
