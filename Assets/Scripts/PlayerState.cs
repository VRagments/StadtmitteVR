using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour {

    private GameObject m_CurrentPointMarker;

	// Use this for initialization
	void Start () {
        m_CurrentPointMarker = null;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ToggleCurrentPointMarker (GameObject pm)
    {
        if (m_CurrentPointMarker != null)
        {
            StartCoroutine (ReactivatePointMarker (m_CurrentPointMarker));
        }
        m_CurrentPointMarker = pm;
        m_CurrentPointMarker.SetActive (false);
    }

    IEnumerator ReactivatePointMarker (GameObject go)
    {
        yield return new WaitForSeconds (1.0f);
        go.SetActive (true);
    }
}
