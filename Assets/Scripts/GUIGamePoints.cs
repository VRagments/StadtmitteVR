using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GUIGamePoints : MonoBehaviour
{

    [SerializeField]
    SceneRun m_SceneRun;
    [SerializeField]
    Image m_TopImage;
    [SerializeField]
    Text m_TopText;
    [SerializeField]
    Image m_BotImage;
    [SerializeField]
    Text m_BotText;
    [SerializeField]
    Text m_TimeUI;

    private int m_CurrentBonusPoints;
    private int m_CurrentMalusPoints;

    // Use this for initialization
    void Start ()
    {
        m_CurrentBonusPoints = m_SceneRun.BonusPoints;
        m_CurrentMalusPoints = m_SceneRun.MalusPoints;
        m_TopImage.color = Color.yellow;
        m_BotImage.color = Color.red;
    }
	
    // Update is called once per frame
    void Update ()
    {
        //was updated
        if (m_CurrentBonusPoints != m_SceneRun.BonusPoints)
        {
            m_CurrentBonusPoints = m_SceneRun.BonusPoints;
            m_TopText.text = "x " + m_CurrentBonusPoints.ToString ();
        }
        if (m_CurrentMalusPoints != m_SceneRun.MalusPoints)
        {
            m_CurrentMalusPoints = m_SceneRun.MalusPoints;
            m_BotText.text = "x " + m_CurrentMalusPoints.ToString ();
        }
        TimeSpan m_TimeSpan = TimeSpan.FromSeconds (m_SceneRun.CurrentTime);
        m_TimeUI.text = string.Format ("{0:D2}:{1:D2}", m_TimeSpan.Minutes, m_TimeSpan.Seconds);
    }
}
