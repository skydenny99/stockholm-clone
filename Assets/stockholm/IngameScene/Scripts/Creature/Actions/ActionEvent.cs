public delegate void ActionCallback<T>(T e) where T : ActionEvent;

public class ActionEvent : IActionEvent
{
	public const string ACTED = "Acted";

	public ActionEvent(string type, IAction action, IActionable target)
	{
		type_ = type;
		action_ = action;
		target_ = target;
	}

	protected string type_;
	public string type
	{
		get { return type_; }
	}

	protected IAction action_;
	public IAction action
	{
		get { return action_; }
	}

	protected IActionable target_;
	public IActionable target
	{
		get { return target_; }
	}
}