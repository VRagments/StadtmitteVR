using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrain : MonoBehaviour
{
    void Awake ()
    {
        Vector3 rootVector = this.transform.position;
        float rootX = rootVector.x;
        float rootY = rootVector.y;
        float rootZ = rootVector.z;
        Sequence seq = DOTween.Sequence ();
        if (this.gameObject.name.Contains ("u2"))
        {
            float forwardOutX = 110.0f;
            float backOutX = -200.0f;
            float outZ = -20.0f;
            float timeOut = 10.0f;
            float timeIn = 12.0f;
            float timeNotVisible = 20.0f;
            float waitingTime = 10.0f;
            seq.Append (this.transform.DOMove (new Vector3 (forwardOutX, rootY, rootZ), timeOut).SetEase (Ease.InQuad));
            seq.Append (this.transform.DOMove (new Vector3 (forwardOutX, rootY, outZ), 0.1f));
            seq.Append (this.transform.DOMove (new Vector3 (backOutX, rootY, outZ), timeNotVisible));
            seq.Append (this.transform.DOMove (new Vector3 (backOutX, rootY, rootZ), 0.1f));
            seq.Append (this.transform.DOMove (rootVector, timeIn).SetEase (Ease.OutQuad));
            seq.AppendInterval (waitingTime);
        } else
        {
            GameObject finalDoors = GameObject.Find ("FinalDoors");
            finalDoors.SetActive (false);
            float forwardOutZ = 300.0f;
            float backOutZ = 0.0f;
            float outX = 100.0f;
            float timeOut = 13.0f;
            float timeIn = 11.0f;
            float timeNotVisible = 23.0f;
            float waitingTime = 11.0f;
            seq.Append (this.transform.DOMove (new Vector3 (rootX, rootY, forwardOutZ), timeOut).SetEase (Ease.InQuad));
            seq.Append (this.transform.DOMove (new Vector3 (outX, rootY, forwardOutZ), 0.1f));
            seq.Append (this.transform.DOMove (new Vector3 (outX, rootY, backOutZ), timeNotVisible));
            seq.Append (this.transform.DOMove (new Vector3 (rootX, rootY, backOutZ), 0.1f));
            seq.Append (this.transform.DOMove (rootVector, timeIn).SetEase (Ease.OutQuad));
            seq.AppendCallback (() => { finalDoors.SetActive (true); });
            seq.AppendInterval (waitingTime);
            seq.AppendCallback (() => { finalDoors.SetActive (false); });
        }
        seq.SetLoops (-1);
    }

    void Start ()
    {
    }

    void Update ()
    {
    }
}