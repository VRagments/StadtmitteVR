using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneRun : MonoBehaviour
{
    // scene run settings
    [SerializeField]
    private Text m_CenterText;
    [SerializeField]
    private bool m_Counting;
    [Tooltip ("Total Time Available in seconds")]
    [SerializeField]
    private int m_TimeAvailable;
    [SerializeField]
    private int m_BonusMultiplier = 50;
    [SerializeField]
    private int m_MalusMultiplier = 30;
    [SerializeField]
    private GameObject m_OSVRPlayer;
    [SerializeField]
    private GameObject m_Player;
    [SerializeField]
    private GameObject m_HUD;
    [SerializeField]
    private GameObject m_OSVRHUDAnchor;
    [SerializeField]
    private GameObject m_HUDAnchor;

    // common scene state
    public float BestTime { get; private set; }

    public float CurrentTime { get; private set; }

    public int BonusPoints { get; private set; }

    public int MalusPoints { get; private set; }

    public bool HasTicket { get; private set; }

    public bool RunStarted { get; private set; }

    public bool HasReachedFinalDestination { get; private set; }

    // private section
    private string m_SceneName;
    private bool m_GameOver;

    public void AddBonus ()
    {
        BonusPoints += 1;
    }

    public void AddMalus ()
    {
        MalusPoints += 1;
    }

    public void SetHasReachedFinalDestination (bool b)
    {
        HasReachedFinalDestination = b;
        ToggleCount ();
    }

    public void ToggleCount ()
    {
        Debug.Log ("Toggle count");
        m_Counting = !m_Counting;
    }

    public void StartRun ()
    {
        RunStarted = true;
    }

    private void Awake ()
    {
        m_SceneName = SceneManager.GetActiveScene ().name;
        BestTime = PlayerPrefs.GetFloat (m_SceneName, m_TimeAvailable);
        CurrentTime = m_TimeAvailable;
        BonusPoints = 0;
        MalusPoints = 0;
        m_GameOver = false;
        RunStarted = false;
        Vector3 HUDPos = new Vector3 (0f, -.4f, 1.3f);
        #if UNITY_ANDROID || UNITY_WEBGL
        m_Player.SetActive (true);
        m_OSVRPlayer.SetActive (false);
        if (m_HUD)
        {
            m_HUD.transform.SetParent (m_HUDAnchor.transform);
            m_HUD.transform.localPosition = HUDPos;
            m_HUD.transform.localRotation = Quaternion.identity;
        }
        #elif UNITY_STANDALONE
        m_Player.SetActive (false);
        m_OSVRPlayer.SetActive (true);
        if (m_HUD)
        {
            m_HUD.transform.SetParent (m_OSVRHUDAnchor.transform);
            m_HUD.transform.localPosition = HUDPos;
            m_HUD.transform.localRotation = Quaternion.identity;
        }
        #endif
    }

    private void Update ()
    {
        if (!m_GameOver)
        {
            if (HasReachedFinalDestination)
            {
                m_GameOver = true;
                StartCoroutine (HandleGameOver (true, CurrentTime));
            }
            if (m_Counting)
            {
                CurrentTime -= Time.deltaTime;
                if (CurrentTime <= 0.0f)
                {
                    m_GameOver = true;
                    StartCoroutine (HandleGameOver (false, 0));
                }
            }
            if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.M))
            {
                SceneManager.LoadScene ("Menu", LoadSceneMode.Single);
            }
        }
    }

    IEnumerator HandleGameOver (bool success, float time)
    {
        if (success)
        {
            int m_Score = Mathf.CeilToInt (CurrentTime) + (BonusPoints * m_BonusMultiplier) - (MalusPoints * m_MalusMultiplier);
            Debug.Log ("Score: " + m_Score.ToString ());
            m_CenterText.text = "Gratulation! Du hast " + m_Score + " Punkte!\n" +
            "Der Highscore liegt bei " + PlayerPrefs.GetInt (m_SceneName, 0).ToString () + " Punkten.";
            m_CenterText.enabled = true;
            PlayerPrefs.SetInt (m_SceneName, m_Score);
        }
        else
        {
            m_CenterText.text = "Schade, Du hast Dein Ziel leider nicht erreicht!\n" +
            "Versuche es ein anderes mal!";
            m_CenterText.enabled = true;
        }
        yield return new WaitForSeconds (10);
        SceneManager.LoadScene ("Menu", LoadSceneMode.Single);
    }
}