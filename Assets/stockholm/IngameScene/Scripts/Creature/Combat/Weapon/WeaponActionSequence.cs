using System.Collections.Generic;

/// <summary>
/// 여러 WeaponAction들과 WeaponCondition들을 차례대로 실행시켜주는 기능을 합니다.
/// </summary>
public class WeaponActionSequence
{
	public delegate void WeaponActionCallback();

	private List<IWeaponAction> actions_ = new List<IWeaponAction>();
	private IEnumerator<IWeaponAction> currentAction_;

	private WeaponActionCallback sequenceCompleteCallback_;

	public IWeaponAction currentRunAction
	{
		get { return currentAction_.Current; }
	}

	private bool isPlaying_ = false;
	public bool isPlaying { get { return isPlaying_; } }

	/// <summary>
	/// 연결된 Action이나 Condition들을 차례대로 실행합니다.
	/// </summary>
	public void Play()
	{
		if (isPlaying_ == false)
		{
			isPlaying_ = true;

			currentAction_ = actions_.GetEnumerator();

			MoveNextAction();
		}
	}

	/// <summary>
	/// Sequence에게 명령을 줍니다. Condition에서 콤보 공격을 이어나갈 때 사용됩니다.
	/// </summary>
	public void Command()
	{
		if (isPlaying_ == true)
		{
			WeaponCondition condition = currentAction_.Current as WeaponCondition;
			if (condition != null)
			{
				condition.CommandAction();
			}
		}
	}

	/// <summary>
	/// 실행중일 때 보내는 Tick입니다. Weapon클래스에서 알아서 호출합니다.
	/// </summary>
	public void Tick()
	{
		if (isPlaying_ == true)
		{
			currentAction_.Current.UpdateAction();
		}
	}

	/// <summary>
	/// 뒤에다가 해당 WeaponAction을 붙입니다.
	/// </summary>
	/// <param name="action">붙일 WeaponAction입니다.</param>
	/// <returns></returns>
	public WeaponActionSequence Append(WeaponAction action)
	{
		actions_.Add(action);
		return this;
	}

	/// <summary>
	/// 뒤에다가 해당 WeaponCondition을 붙입니다.
	/// </summary>
	/// <param name="condition">붙일 WeaponCondition입니다.</param>
	/// <returns></returns>
	public WeaponActionSequence Append(WeaponCondition condition)
	{
		actions_.Add(condition);
		return this;
	}

	/// <summary>
	/// 연결된 모든 Action이나 Condition들이 끝나면 실행되는 콜백함수를 설정합니다.
	/// </summary>
	/// <param name="callback">다 끝날 때 실행되는 콜백함수입니다.</param>
	/// <returns></returns>
	public WeaponActionSequence OnComplete(WeaponActionCallback callback)
	{
		sequenceCompleteCallback_ = callback;
		return this;
	}

	/// <summary>
	/// 다음 WeaponAction을 실행합니다.
	/// </summary>
	private void MoveNextAction()
	{
		if (currentAction_.MoveNext() == true)
		{
			currentAction_.Current.ExecuteAction();
			currentAction_.Current.endCallback += EndAction;
		}
		else
		{
			sequenceCompleteCallback_();
			isPlaying_ = false;
			currentAction_ = null;
		}
	}

	/// <summary>
	/// 현재 실행중인 WeaponAction이 끝났을 때 실행되는 함수입니다.
	/// </summary>
	/// <param name="isSuccess">WeaponAction이 성공적으로 끝났을 경우, True를 가집니다.</param>
	private void EndAction(bool isSuccess)
	{
		currentAction_.Current.endCallback -= EndAction;

		if (isSuccess)
		{
			MoveNextAction();
		}
		else
		{
			sequenceCompleteCallback_();
			isPlaying_ = false;
			currentAction_ = null;
		}
	}
}