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
    [SerializeField] private int m_highScore;
    [SerializeField] private int m_totalCoinsEarned;

    //

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
        SceneManager.LoadScene(0);
    }

    public void NextButton()
    {
        SceneManager.LoadScene(1);
    }
}
