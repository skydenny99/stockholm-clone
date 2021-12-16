using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionEvent
{
	string type
	{
		get;
	}
	
	IAction action
	{
		get;
	}

	IActionable target
	{
		get;
	}
}
