using UnityEngine;

public interface IController<T> where T : class, IControlable
{
	T targetMonster { get; set; }
}