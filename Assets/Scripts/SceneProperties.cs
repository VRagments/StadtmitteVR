using UnityEngine;
using System.Collections;

public class SceneProperties : MonoBehaviour
{
    // Player properties (speed, turn rate, etc.)
    [SerializeField] GameObject m_Player;
    [SerializeField]
    Vector3 m_InitialRotation;
    [SerializeField] bool m_NavMeshAgentTurnsPlayer;

    private NavMeshAgent m_NavMeshAgent;

    void Awake ()
    {
        m_Player = GameObject.FindGameObjectWithTag ("Player");
        m_Player.transform.rotation = Quaternion.Euler (m_InitialRotation);
        m_NavMeshAgent = m_Player.transform.GetComponent<NavMeshAgent> ();
    }

    // Use this for initialization
    void Start ()
    {
        InitialzeProperties ();
    }
	
    // Update is called once per frame
    void Update ()
    {
	
    }

    private void InitialzeProperties ()
    {
        m_NavMeshAgent.updateRotation = m_NavMeshAgentTurnsPlayer;
    }
}
