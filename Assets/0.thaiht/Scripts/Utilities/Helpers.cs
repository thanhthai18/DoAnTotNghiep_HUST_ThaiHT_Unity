using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A static class for general helpful methods
/// </summary>
public static class Helpers
{
    /// <summary>
    /// Destroy all child objects of this transform (Unintentionally evil sounding).
    /// Use it like so:
    /// <code>
    /// transform.DestroyChildren();
    /// </code>
    /// </summary>
    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t) UnityEngine.Object.Destroy(child.gameObject);
    }

    public static void ShowMessageText(this TextMeshProUGUI txt, string text, bool isSucces)
    {
        txt.color = isSucces ? Color.green : Color.red;
        txt.text = text;
    }

    public static void AnimButton(this Button btn, int indexChild = -1)
    {
        if (indexChild == -1)
        {
            btn.transform.DOKill();
            btn.interactable = false;
            btn.transform.DOPunchScale(0.2f * Vector3.one, 0.2f).SetEase(Ease.Linear).OnComplete(() => 
            {
                btn.interactable = true;
            });
        }
        else
        {
            btn.transform.GetChild(indexChild).transform.DOKill();
            btn.interactable = false;
            btn.transform.GetChild(indexChild).DOPunchScale(0.2f * Vector3.one, 0.2f).SetEase(Ease.Linear).OnComplete(() => 
            {
                btn.interactable = true;
            });
        }
    }

    public static void Wait(this MonoBehaviour mono, float delay, Action action)
    {
        mono.StartCoroutine(ExecuteAction(delay, action));
    }

    private static IEnumerator ExecuteAction(float delay, Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action?.Invoke();
        yield break;
    }
    public static void GetComponentAtPath<T>(
            this Transform transform,
            string path,
            out T foundComponent) where T : Component
    {
        Transform t = null;
        if (path == null)
        {
            // Return the component of the first child that have that type of component
            foreach (Transform child in transform)
            {
                T comp = child.GetComponent<T>();
                if (comp != null)
                {
                    foundComponent = comp;
                    return;
                }
            }
        }
        else
            t = transform.Find(path);

        if (t == null)
            foundComponent = default(T);
        else
            foundComponent = t.GetComponent<T>();
    }

    public static T GetComponentAtPath<T>(
        this Transform transform,
        string path) where T : Component
    {
        T foundComponent;
        transform.GetComponentAtPath(path, out foundComponent);

        return foundComponent;
    }
}
