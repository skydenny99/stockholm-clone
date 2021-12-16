/// <summary>
/// 아이템들의 효과를 설정하는 인터페이스입니다.
/// </summary>
public interface IItemEffect
{
	float effectValue { get; }
	Player target { get; set; }

	/// <summary>
	/// 해당 아이템의 효과를 적용합니다.
	/// </summary>
	void ApplyEffect();
	/// <summary>
	/// 해당 아이템의 효과를 해제합니다.
	/// </summary>
	void DiscardEffect();
}
