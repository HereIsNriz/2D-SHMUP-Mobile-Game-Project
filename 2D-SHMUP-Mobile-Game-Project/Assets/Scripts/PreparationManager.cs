using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PreparationManager : MonoBehaviour
{
    //

    //
    [SerializeField] private TextMeshProUGUI m_coinsText;
    [SerializeField] private TextMeshProUGUI m_highScoreText;
    [SerializeField] private AudioSource m_backButtonSound;
    [SerializeField] private AudioSource m_nextButtonSound;
    [SerializeField] private int m_highScore;
    [SerializeField] private int m_totalCoinsEarned;

    //
    private float m_backButtonSoundDuration = 0.2f;
    private float m_nextButtonSoundDuration = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        SetCoins();
        SetHighScore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetCoins()
    {
        m_totalCoinsEarned = PlayerPrefs.GetInt("Coins", 0);
        m_coinsText.text = m_totalCoinsEarned.ToString();
    }

    private void SetHighScore()
    {
        m_highScore = PlayerPrefs.GetInt("HighScore", 0);
        m_highScoreText.text = "High Score:\n" + m_highScore.ToString();
    }

    public void BackButton()
    {
        m_backButtonSound.PlayOneShot(m_backButtonSound.clip, 1f);
        StartCoroutine(BackButtonSound());
    }

    private IEnumerator BackButtonSound()
    {
        yield return new WaitForSeconds(m_backButtonSoundDuration);
        SceneManager.LoadScene(0);
    }

    public void NextButton()
    {
        m_nextButtonSound.PlayOneShot(m_nextButtonSound.clip, 1f);
        StartCoroutine(NextButtonSound());
    }

    private IEnumerator NextButtonSound()
    {
        yield return new WaitForSeconds(m_nextButtonSoundDuration);
        SceneManager.LoadScene(1);
    }
}
