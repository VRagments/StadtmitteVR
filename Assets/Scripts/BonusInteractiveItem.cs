using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;
using System.Collections;

// This script is a simple example of how an interactive item can
// be used to change things on gameobjects by handling events.
using UnityEngine.SceneManagement;


public class BonusInteractiveItem : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_HoverSound;
    [SerializeField] private AudioClip m_ClickedSound;

    [SerializeField] private VRInteractiveItem m_InteractiveItem;
    [SerializeField] private GameObject m_PointMarker;
    [SerializeField] private OnHoverRadial m_OnHoverRadial;

    [SerializeField]
    private int m_Bonus = 50;

    [SerializeField]
    private SceneRun m_SceneRun;

    private Transform root;
    private Agent agent;

    // this is true if this point is on the u6 in the last scenario
    private bool final;

    private void Awake ()
    {
        final = false;
        root = transform;
        while (root.parent != null && root.parent.name != "BonusTargets")
        {
            root = root.parent;
            if (root.parent.name == "FinalDoors")
            {
                final = true;
                break;
            }
        }
        agent = FindObjectOfType<Agent> ();
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
        float dist = Vector3.Distance (root.position, agent.transform.position);
        // check if we are actually close enough to see it
        if (dist < 7.0f)
        {
            m_AudioSource.clip = m_ClickedSound;
            m_AudioSource.Play ();
            m_OnHoverRadial.Hide ();

            if (final)
            {
                // win game
                m_SceneRun.ToggleCount ();
                m_SceneRun.SetHasReachedFinalDestination (true);
            } else
            {
                //Add or deduct bonus points
                m_SceneRun.AddBonus ();
                agent.SecondaryTargets.Remove (root);
            }
            GameObject.DestroyImmediate (m_PointMarker);
        }
    }
}