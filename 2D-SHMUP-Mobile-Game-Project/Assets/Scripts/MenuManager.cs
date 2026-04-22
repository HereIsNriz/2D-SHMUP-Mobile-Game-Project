using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuManager : MonoBehaviour
{
    //

    //
    [SerializeField] private GameObject m_exitAppPanel;
    [SerializeField] private AudioSource m_playButtonSound;
    [SerializeField] private AudioSource m_yesButtonSound;

    //
    private bool m_isExitAppPanelShowing;
    private float m_playButtonSoundDuration = 0.2f;
    private float m_yesButtonSoundDuration = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        m_isExitAppPanelShowing = false;
    }

    // Update is called once per frame
    void Update()
    {
        ShowExitAppPanel();
    }

    public void PlayButton()
    {
        m_playButtonSound.PlayOneShot(m_playButtonSound.clip, 1f);
        StartCoroutine(PlayButtonSound());
    }

    private IEnumerator PlayButtonSound()
    {
        yield return new WaitForSeconds(m_playButtonSoundDuration);
        SceneManager.LoadScene(2);
    }

    private void ShowExitAppPanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !m_isExitAppPanelShowing)
        {
            m_exitAppPanel.gameObject.SetActive(true);
            m_isExitAppPanelShowing = true;
            m_playButtonSound.PlayOneShot(m_playButtonSound.clip, 1f);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && m_isExitAppPanelShowing)
        {
            m_exitAppPanel.gameObject.SetActive(false);
            m_isExitAppPanelShowing = false;
            m_playButtonSound.PlayOneShot(m_playButtonSound.clip, 1f);
        }
    }

    public void YesButton()
    {
        m_yesButtonSound.PlayOneShot(m_yesButtonSound.clip, 1f);
        StartCoroutine(YesButtonSound());
    }

    private IEnumerator YesButtonSound()
    {
        yield return new WaitForSeconds(m_yesButtonSoundDuration);
        //Application.Quit();
        EditorApplication.isPlaying = false;
    }

    public void NoButton()
    {
        if (m_isExitAppPanelShowing)
        {
            m_exitAppPanel.gameObject.SetActive(false);
            m_isExitAppPanelShowing = false;
            m_playButtonSound.PlayOneShot(m_playButtonSound.clip, 1f);
        }
    }
}
