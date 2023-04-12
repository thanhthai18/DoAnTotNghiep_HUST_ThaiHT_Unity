using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour
{
	public bool isActive;
	public abstract void Initialize();

	public virtual void Hide()
	{
		isActive = false;
		gameObject.SetActive(isActive);
	}
	public virtual void Show()
	{
		isActive = true;
		gameObject.SetActive(isActive);
	}
}
