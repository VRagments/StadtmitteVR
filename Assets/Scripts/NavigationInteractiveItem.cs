using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;
using System.Collections;

// This script is a simple example of how an interactive item can
// be used to change things on gameobjects by handling events.
using UnityEngine.SceneManagement;


public class NavigationInteractiveItem : MonoBehaviour
{
    [SerializeField] private Canvas m_SymbolGUI;

    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_HoverSound;
    [SerializeField] private AudioClip m_ClickedSound;

    [SerializeField] private VRInteractiveItem m_InteractiveItem;
    [SerializeField] private GameObject m_PointMarker;
    [SerializeField] private OnHoverRadial m_OnHoverRadial;

    [SerializeField] private Transform m_Target;

    private GameObject m_Player;
    private NavMeshAgent m_NavMeshAgent;
    private PlayerState m_PlayerState;
    private SceneRun m_SceneRun;

    private void Awake ()
    {
        m_Player = GameObject.FindGameObjectWithTag ("Player");
        m_NavMeshAgent = m_Player.GetComponent<NavMeshAgent> ();
        m_PlayerState = m_Player.GetComponent<PlayerState> ();
        m_SceneRun = FindObjectOfType<SceneRun> ();
    }


    private void OnEnable ()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
        m_InteractiveItem.OnClick += HandleClick;
        m_OnHoverRadial.OnSelectionComplete += HandleClick;
    }


    private void OnDisable ()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
        m_InteractiveItem.OnClick -= HandleClick;
        m_OnHoverRadial.OnSelectionComplete -= HandleClick;
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
        if (!m_SceneRun.RunStarted)
        {
            return;
        }
        //Debug.Log ("Show click state");
        m_AudioSource.clip = m_ClickedSound;
        m_AudioSource.Play ();
        m_OnHoverRadial.Hide ();

        //store the PointMarker game object in case we can't reactivate via collider
        m_PlayerState.ToggleCurrentPointMarker (m_PointMarker);
        m_NavMeshAgent.SetDestination (m_Target.position);
    }
}