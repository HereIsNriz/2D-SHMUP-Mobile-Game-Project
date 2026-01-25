using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PreparationManager : MonoBehaviour
{
    //
    public int HighScore;
    public int Coins;

    //
    [SerializeField] private TextMeshProUGUI m_coinsText;
    [SerializeField] private TextMeshProUGUI m_highScoreText;

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
        m_coinsText.text = Coins.ToString();
    }

    private void SetHighScore()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        m_highScoreText.text = "High Score:\n" + HighScore.ToString();
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
