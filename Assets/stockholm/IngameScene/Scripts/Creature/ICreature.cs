/// <summary>
/// HP를 갖고있으며 살거나 죽을 수 있는 개체에 대한 인터페이스입니다.
/// </summary>
public interface ICreature : IMovable
{
	int hp { get; }
	int maxHp { get; }
	bool isAlive { get; }

	void Damage(int damage);
	void Heal(int amount);
	void Kill();
}