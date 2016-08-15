using UnityEngine;
using System.Collections;

public class CanvasCullingController : MonoBehaviour
{

    [SerializeField]
    private Transform[] m_Transforms;
    [SerializeField]
    private GameObject m_CullingCollider;

    void Awake ()
    {
        foreach (Transform t in m_Transforms)
        {
            t.gameObject.SetActive (false);
        }
    }

    // Use this for initialization
    void Start ()
    {
	
    }
	
    // Update is called once per frame
    void Update ()
    {
	
    }

    /// <summary>
    /// Used to activate canvas components based on player position.
    /// 
    /// Activate this game object, if in reach of the player's CullingCollider.
    /// Highlight the base ring, if the player has actually reached this position.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject != m_CullingCollider)
        {
            return;
        }
        else
        {
            foreach (Transform t in m_Transforms)
            {
                t.gameObject.SetActive (true);
            }
        }
    }

    /// <summary>
    /// Used to deactivate canvas components based on player position.
    /// 
    /// Dectivate this game object, if out of reach of the player's CullingCollider.
    /// /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit (Collider other)
    {
        if (other.gameObject != m_CullingCollider)
        {
            return;
        }
        else
        {
            foreach (Transform t in m_Transforms)
            {
                t.gameObject.SetActive (false);
            }
        }
    }

}
