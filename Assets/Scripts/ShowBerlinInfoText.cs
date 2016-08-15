using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class ShowBerlinInfoText : MonoBehaviour {
    
    [SerializeField]
    private OnHoverRadial m_OnHoverRadial;
    [SerializeField]
    private Text m_BerlinInfoText;

    private void Awake ()
    {
        m_BerlinInfoText.enabled = false;
    }


    private void OnEnable ()
    {
        m_OnHoverRadial.OnSelectionComplete += HandleClick;
    }


    private void OnDisable ()
    {
        m_OnHoverRadial.OnSelectionComplete -= HandleClick;
    }

    //Handle the Click event
    private void HandleClick ()
    {
        m_BerlinInfoText.enabled = true;
    }
}