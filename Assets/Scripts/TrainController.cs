using UnityEngine;
using System.Collections;

public class TrainController : MonoBehaviour
{
    public bool m_UsePhysics;
    [SerializeField]
    private float m_Thrust;
    [SerializeField]
    private Rigidbody m_Rigidbody;

    public bool m_UseTween;
    [SerializeField]
    private iTweenPath m_Path;
    [SerializeField]
    private iTween.EaseType m_EaseType;
    [SerializeField]
    public GameObject m_Train;
    [SerializeField]
    public float m_TravelDuration = 120;
    [SerializeField]
    public float m_StopDuration = 30;

    // Use this for initialization
    void Start ()
    {
        if (m_UseTween)
        {
            if (m_Train == null || m_Path == null || m_TravelDuration == 0)
            {
                Debug.LogWarning ("MoveUserAlongPath parameters undefined!");
            }
            else
            {
                Debug.Log ("Init iTween for Train " + m_Train.name);
                iTween.Init (m_Train);
                // train may roll
                TrainBeDriving ();
            }
        }
        else
        if (m_UsePhysics)
        {
            m_Rigidbody = gameObject.AddComponent<Rigidbody> ();
        }
    }
	
    // Update is called once per frame
    void Update ()
    {
	
    }

    void FixedUpdate ()
    {
        if (m_UsePhysics)
        {
            m_Rigidbody.AddForce (transform.right * m_Thrust);
        }
    }

    void TrainBeDriving ()
    {
        Debug.Log ("StartCoroutine TrainBeDriving.");
        StartRoute ();
        StartCoroutine (WaitInStation (m_StopDuration));
    }

    IEnumerator WaitInStation (float dur)
    {
        Debug.Log ("StartCoroutine WaitInStation.");
        yield return new WaitForSeconds (dur);
        TrainBeDriving ();
    }

    void StartRoute ()
    {
        Debug.Log ("StartCoroutine StartRoute.");
        iTween.MoveTo (m_Train, iTween.Hash (
            "path", iTweenPath.GetPath (m_Path.pathName),
            "orienttopath", true,
            "lookahead", .5f,
            "time", m_TravelDuration,
            "easeType", m_EaseType,
            "oncomplete", "TrainBeDriving",
            "oncompletetarget", gameObject));
    }
}
