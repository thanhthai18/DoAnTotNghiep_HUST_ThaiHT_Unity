using Doozy.Runtime.UIManager.Containers;
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
        if (TryGetComponent(out UIView view))
        {
            view.Hide();
        }
        else
        {
            gameObject.SetActive(isActive);
        }
    }
    public virtual void Show()
    {
        isActive = true;
        if (TryGetComponent(out UIView view))
        {
            view.Show();
        }
        else
        {
            gameObject.SetActive(isActive);
        }
    }
}
