using DG.Tweening;
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
        foreach (Transform child in t) Object.Destroy(child.gameObject);
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
}
