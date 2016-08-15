using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;
using System.Collections;

// This script is a simple example of how an interactive item can
// be used to change things on gameobjects by handling events.
using UnityEngine.SceneManagement;


public class SceneLoad : MonoBehaviour
{
    [SerializeField]
    private Canvas m_SymbolGUI;

    [SerializeField]
    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip m_HoverSound;
    [SerializeField]
    private AudioClip m_ClickedSound;

    [SerializeField]
    private VRInteractiveItem m_InteractiveItem;
    [SerializeField]
    private OnHoverRadial m_OnHoverRadial;
    [SerializeField]
    private string m_LoadSceneName;

    private void Awake ()
    {
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
        //Debug.Log ("Show click state");
        m_AudioSource.clip = m_ClickedSound;
        m_AudioSource.Play ();

        if (!string.IsNullOrEmpty (m_LoadSceneName))
        {
            SceneManager.LoadScene (m_LoadSceneName, LoadSceneMode.Single);
        }
    }
}