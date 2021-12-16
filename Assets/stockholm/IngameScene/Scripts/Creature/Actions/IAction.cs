/// <summary>
/// 행위를 정의하는 인터페이스입니다.
/// </summary>
public interface IAction
{
	void Act(IActionable target);
}