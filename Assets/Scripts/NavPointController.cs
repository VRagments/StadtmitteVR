using System;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Common;

public class NavPointController : MonoBehaviour
{
    public enum NavPointTypes
    {
        Regular,
        Checkpoint,
        Final,
        Bonus,
        Malus,
    }

    public event Action<NavPointController> OnGateReached;

    [SerializeField] private NavPointTypes m_NavPointType;
    [SerializeField] private int m_Score = 100;
    // The amount added to the player's score when the NavPointController is activated.
    [SerializeField] private AudioSource m_AudioSource;
    // Reference to the audio source that plays a clip when the player activates the NavPointController.
    [SerializeField] private Color m_BaseColor = Color.cyan;
    [SerializeField] private Color m_ActivatedColor = Color.green;
    [SerializeField]
    public GameObject m_NavPointActivator;
    [SerializeField]
    public GameObject m_PointMarker;
    [SerializeField]
    public GameObject m_Gate;
    [SerializeField]
    private SceneRun m_SceneRun;

    private bool m_HasTriggered;
    private GameObject m_Player;
    private GameObject m_CullingCollider;
    private List<Material> m_Materials;
    private bool m_Active;

    public bool Activated
    {
        set
        {
            m_Active = value;

            if (m_HasTriggered)
                return;

            SetRingColour (m_Active ? m_ActivatedColor : m_BaseColor);
        }

        get { return m_Active; }
    }


    private void Awake ()
    {
        // Create a list of materials and add the main material on each child renderer to it.
        m_Materials = new List<Material> ();
        Renderer[] renderers = m_Gate.transform.GetComponentsInChildren<Renderer> ();

        for (int i = 0; i < renderers.Length; i++)
        {
            m_Materials.Add (renderers [i].material);
        }

        m_Player = GameObject.FindGameObjectWithTag ("Player");
        m_CullingCollider = GameObject.FindGameObjectWithTag ("CullingCollider");
    }

    private void Start ()
    {
        m_NavPointActivator.SetActive (false);
    }


    private void Update ()
    {
    }

    /// <summary>
    /// Used to activate interactive components based on player position.
    /// 
    /// Activate this game object, if in reach of the player's CullingCollider.
    /// Highlight the base ring, if the player has actually reached this position.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter (Collider other)
    {
        // If this ring has already triggered or the ring has not collided with the flyer return.
        if (m_HasTriggered || (other.gameObject != m_Player && other.gameObject != m_CullingCollider))
        {
            return;
        }

        if (other.gameObject == m_CullingCollider)
        {
            m_NavPointActivator.SetActive (true);
            return;
        }

        if (other.gameObject == m_Player)
        {
            // Otherwise the ring has been triggered.
            m_HasTriggered = true;

            // Play audio if not of regular type (too annoying otherwise)
            if (m_NavPointType != NavPointTypes.Regular)
            {
                m_AudioSource.Play ();
            }            

            // Set the ring's colour.
            SetRingColour (m_ActivatedColor);

            if (m_NavPointType == NavPointTypes.Final)
            {
                m_SceneRun.ToggleCount ();
                m_SceneRun.SetHasReachedFinalDestination (true);
            }
            if (m_NavPointType == NavPointTypes.Bonus)
            {
                m_SceneRun.AddBonus ();
            }
            if (m_NavPointType == NavPointTypes.Malus)
            {
                m_SceneRun.AddMalus ();
            }
        }    
    }

    /// <summary>
    /// Used to deactivate interactive components based on player position.
    /// 
    /// Dectivate this game object, if out of reach of the player's CullingCollider.
    /// De-Highlight the base ring, if the player has actually left this position and restore the PointMarker.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit (Collider other)
    {
        // If this ring has already triggered or the ring has not collided with the flyer return.
        if (!m_HasTriggered && other.gameObject != m_Player && other.gameObject != m_CullingCollider)
        {
            return;
        }

        if (other.gameObject == m_CullingCollider)
        {
            m_NavPointActivator.SetActive (false);
            return;
        }

        if (other.gameObject == m_Player)
        {
            // Otherwise the ring has been triggered.
            m_HasTriggered = false;

            // Set the ring's colour.
            SetRingColour (m_BaseColor);

            m_PointMarker.SetActive (true);
        }
    }


    private void OnDestroy ()
    {
        // Ensure the event is completely unsubscribed when the ring is destroyed.
        OnGateReached = null;
    }


    public void Restart ()
    {
        // Reset the colour to it's original colour.
        SetRingColour (m_BaseColor);

        // The ring has no longer been triggered.
        m_HasTriggered = false;
    }


    private void SetRingColour (Color color)
    {
        // Go through all the materials and set their colour appropriately.
        for (int i = 0; i < m_Materials.Count; i++)
        {
            m_Materials [i].color = color;
        }
    }
}