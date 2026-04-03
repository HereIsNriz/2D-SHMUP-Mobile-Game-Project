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

    //
    private bool m_isExitAppPanelShowing;

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
        SceneManager.LoadScene(2);
    }

    private void ShowExitAppPanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !m_isExitAppPanelShowing)
        {
            m_exitAppPanel.gameObject.SetActive(true);
            m_isExitAppPanelShowing = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && m_isExitAppPanelShowing)
        {
            m_exitAppPanel.gameObject.SetActive(false);
            m_isExitAppPanelShowing = false;
        }
    }

    public void YesButton()
    {
        //Application.Quit();
        EditorApplication.isPlaying = false;
    }

    public void NoButton()
    {
        if (m_isExitAppPanelShowing)
        {
            m_exitAppPanel.gameObject.SetActive(false);
            m_isExitAppPanelShowing = false;
        }
    }
}
