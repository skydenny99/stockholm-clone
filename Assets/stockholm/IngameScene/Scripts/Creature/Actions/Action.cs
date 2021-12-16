using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : IAction
{
	public Action()
	{
		
	}

	public virtual void Act(IActionable target)
	{
		if(acted_ != null)
			acted_.Invoke(new ActionEvent(ActionEvent.ACTED, this, target));
	}

	private ActionCallback<ActionEvent> acted_;
	public ActionCallback<ActionEvent> acted
	{
		set
		{
			acted_ = value;
		}
	}
	
	
}
