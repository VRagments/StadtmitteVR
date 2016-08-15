using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LookAt : MonoBehaviour
{
    private Text text;

    void Awake ()
    {
        text = GetComponentInChildren<Text> ();
    }

    public void DoLookAt (Vector3 vec)
    {
        transform.DOLookAt (vec, 1.0f);
    }

    public void DoPosition (Vector3 vec)
    {
        transform.DOMove (vec, 1.0f);
    }

    internal void MarkDistance (float shortestDist)
    {
        if (text != null)
        {
            text.text = LeastDistracting (shortestDist);
        }
    }

    private string LeastDistracting (float num)
    {
        if (num > 10.0f)
        {
            return string.Format ("{0:0} m", Mathf.Round (num / 10.0f) * 10.0f);
        }
        if (num > 1.0f)
        {
            return string.Format ("{0:0.0} m", num);
        }
        return string.Format ("{0:0.00} m", num);
    }
}
