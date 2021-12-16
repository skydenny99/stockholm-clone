using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeymapData : IData
{
	public List<Axis> axes = new List<Axis>();
	public KeymapData() 
	{
		Axis horizontal = new Axis();
		horizontal.name = "Horizontal";
		horizontal.positiveButton = KeyCode.RightArrow;
		horizontal.alternativePositiveButton = KeyCode.D;
		horizontal.negativeButton = KeyCode.LeftArrow;
		horizontal.alternativeNegativeButton = KeyCode.A;
		horizontal.sensitivity = 2;
		horizontal.gravity = 1;

		axes.Add(horizontal);

		Axis vertical = new Axis();
		vertical.name = "Vertical";
		vertical.positiveButton = KeyCode.UpArrow;
		vertical.alternativePositiveButton = KeyCode.W;
		vertical.negativeButton = KeyCode.DownArrow;
		vertical.alternativeNegativeButton = KeyCode.S;
		vertical.sensitivity = 2;
		vertical.gravity = 1;

		axes.Add(vertical);
	}
	public KeymapData(List<Axis> axes)
	{
		this.axes = axes;
	}
}
