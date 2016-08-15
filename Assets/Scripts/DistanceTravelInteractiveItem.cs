using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections;

public class DistanceTravelInteractiveItem : MonoBehaviour
{

    public bool m_TravelWithFade;
    public float m_TravelSpeed;

    [SerializeField]
    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip m_HoverSound;
    [SerializeField]
    private AudioClip m_ClickedSound;

    [SerializeField]
    private VRInteractiveItem m_InteractiveItem;
    [SerializeField]
    private GameObject m_PointMarker;
    [SerializeField]
    private OnHoverRadial m_OnHoverRadial;

    [SerializeField]
    private Transform m_PortalGate;
    [SerializeField]
    private Transform m_RemotePortalGate;

    private SceneFadeCamera m_SceneFadeCamera;
    private GameObject m_Player;
    private NavMeshAgent m_NavMeshAgent;
    private float m_OriginalSpeed;
    private PlayerState m_PlayerState;
    private Collider m_Collider;

    // fade type that is next
    private enum FadeType
    {
        fadeIn,
        fadeOut,
    }

    private FadeType m_FadeType = FadeType.fadeOut;

    void Awake ()
    {
        Debug.Log ("Awake() RUN: " + gameObject.name);
        while (GameObject.FindGameObjectWithTag ("MainCamera") == null)
        {
            //wait for MainCamera object to come into existence
        }
        m_SceneFadeCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<SceneFadeCamera> ();
    }

    // Use this for initialization
    void Start ()
    {
        Debug.Log ("Start() RUN: " + gameObject.name);
        m_Player = GameObject.FindGameObjectWithTag ("Player");
        m_NavMeshAgent = m_Player.GetComponent<NavMeshAgent> ();
        m_OriginalSpeed = m_NavMeshAgent.speed;
        m_PlayerState = m_Player.GetComponent<PlayerState> ();
        m_Collider = gameObject.GetComponent<Collider> ();
    }


    private void OnEnable ()
    {
        Debug.Log ("OnEnable() RUN: " + gameObject.name);
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
        m_InteractiveItem.OnClick += HandleClick;
        m_OnHoverRadial.OnSelectionComplete += HandleClick;
        m_SceneFadeCamera.OnFadeComplete += HandleFade;
    }


    private void OnDisable ()
    {
        Debug.Log ("OnDisable() RUN: " + gameObject.name);
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
        m_InteractiveItem.OnClick -= HandleClick;
        m_OnHoverRadial.OnSelectionComplete -= HandleClick;
        m_SceneFadeCamera.OnFadeComplete -= HandleFade;
    }


    //Handle the Over event
    private void HandleOver ()
    {
        //Debug.Log ("Show over state");
        m_AudioSource.clip = m_HoverSound;
        m_AudioSource.Play ();
    }


    //Handle the Out event
    private void HandleOut ()
    {
        //Debug.Log ("Show out state");
    }


    //Handle the Click event
    private void HandleClick ()
    {
        Debug.Log ("Show click state");
        m_AudioSource.clip = m_ClickedSound;
        m_AudioSource.Play ();
        m_OnHoverRadial.Hide ();

        m_Collider.enabled = false;
        StartCoroutine (StartDistanceTrip ());
    }

    private void HandleFade (int callGO)
    {
        //has the fade been called from this game object?
        if (callGO != this.gameObject.GetInstanceID ())
        {
            Debug.Log ("Unmatching fade event from " + callGO + " in " + this.gameObject.GetInstanceID ());
            return;
        }
        Debug.Log ("Matching fade event from " + callGO + " in " + this.gameObject.GetInstanceID ());
        //we have gone on elevator trip
        if (m_FadeType == FadeType.fadeOut)
        {
            Debug.Log ("HandleFadeOut()");
            StartCoroutine (RunDistanceTrip ());
            m_FadeType = FadeType.fadeIn;
        }
        //we return from elevator trip
        else
        if (m_FadeType == FadeType.fadeIn)
        {
            Debug.Log ("HandleFadeIn()");
            StopDistanceTrip ();
            m_FadeType = FadeType.fadeOut;
        }
    }

    /// <summary>
    /// Move towards elevator and fade out
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDistanceTrip ()
    {
        Debug.Log ("StartDistanceTrip()");
        if (m_TravelWithFade)
        {
            // move in front of elevator
            yield return StartCoroutine (MoveInPlace ());
            m_SceneFadeCamera.FadeOut (3, this.gameObject.GetInstanceID ());
        }
        else
        if (!m_TravelWithFade)
        {
            //store the PointMarker game object in case we can't reactivate via collider
            StartCoroutine (RunDistanceTrip ());
        }
        
    }

    IEnumerator MoveInPlace ()
    {
        Debug.Log ("MoveInPlace()");
        //store the PointMarker game object in case we can't reactivate via collider
        m_NavMeshAgent.SetDestination (m_PortalGate.position);

        while (m_NavMeshAgent.pathPending)
        {
            yield return null;
        }

        while (m_NavMeshAgent.remainingDistance > 0.4f)
        {
            Debug.Log ("Remaining distance: " + m_NavMeshAgent.remainingDistance.ToString ());
            yield return null;
        }
        m_NavMeshAgent.ResetPath ();
        Debug.Log ("Moved In Place.");
    }

    /// <summary>
    /// After fade out, run animation for elevator trip
    /// </summary>
    /// <returns></returns>
    IEnumerator RunDistanceTrip ()
    {
        Debug.Log ("RunDistanceTrip()");
        if (m_TravelWithFade)
        {
            Debug.Log ("Travel from " + m_Player.transform.position.ToString () + " to " + m_RemotePortalGate.position.ToString ());
            m_NavMeshAgent.enabled = false;
            m_Player.transform.position = m_RemotePortalGate.position;
            m_NavMeshAgent.enabled = true;
            m_SceneFadeCamera.FadeIn (3, this.gameObject.GetInstanceID ());
        }
        else
        if (!m_TravelWithFade)
        {
            m_NavMeshAgent.speed = m_TravelSpeed;
            m_NavMeshAgent.SetDestination (m_RemotePortalGate.position);
            while (Vector3.Distance (m_NavMeshAgent.transform.position, m_RemotePortalGate.position) > 0.4f)
            {
                Debug.Log ("Remaining distance: " + m_NavMeshAgent.remainingDistance.ToString ());
                yield return null;
            }
            m_NavMeshAgent.ResetPath ();
            m_NavMeshAgent.speed = m_OriginalSpeed;
            StopDistanceTrip ();
        }
    }

    /// <summary>
    /// After arrival, restore previous settings.
    /// </summary>
    /// <returns></returns>
    void StopDistanceTrip ()
    {
        Debug.Log ("StopDistanceTrip()");
        m_Collider.enabled = true;
    }
}