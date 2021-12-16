public delegate void EndActionCallback(bool isSuccess);

/// <summary>
/// WeaponAction을 구현할 인터페이스입니다.
/// </summary>
public interface IWeaponAction
{
	/// <summary>
	/// 공격할 때 재생할, 이 무기를 가지고 있는 Actor의 애니메이션 이름입니다.
	/// </summary>
	string actorAnimationName { get; set; }
	/// <summary>
	/// 공격할 때 재생할, 이 무기의 애니메이션 이름입니다.
	/// </summary>
	string weaponAnimationName { get; set; }
	/// <summary>
	/// 해당 WeaponAction을 실행할 Weapon입니다.
	/// </summary>
	Weapon weapon { get; set; }


	/// <summary>
	/// 해당 WeaponAction이 끝났을 때 실행할 콜백입니다.
	/// </summary>
	EndActionCallback endCallback { get; set; }

	/// <summary>
	/// 해당 Action이 처음 실행될 때 한 번 실행되는 함수입니다.
	/// </summary>
	void OnExecute();

	/// <summary>
	/// 해당 Action이 실행될 때 매 프레임마다 실행되는 함수입니다.
	/// </summary>
	void OnUpdate();

	/// <summary>
	/// 해당 Action이 끝났을 때 한 번 실행되는 함수입니다.
	/// </summary>
	void OnEnd();


	/// <summary>
	/// 해당 Action을 실행합니다.
	/// </summary>
	void ExecuteAction();

	/// <summary>
	/// Action을 업데이트합니다. 매 프레임마다 실행해주세요.
	/// </summary>
	void UpdateAction();

	/// <summary>
	/// 현재 실행중인 Action을 중지합니다. Sequence에 연결된 다음 Action을 실행하지 않습니다.
	/// </summary>
	void CancelAction();

	/// <summary>
	/// 현재 실행중인 Action을 끝냅니다. Sequence에 연결된 다음 Action을 실행합니다.
	/// </summary>
	void EndAction();
}