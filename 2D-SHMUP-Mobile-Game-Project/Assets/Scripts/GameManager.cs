using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //
    public bool IsGameRunning;
    public bool IsBossExist;
    public int PlayerScore;

    //
    [SerializeField] private GameObject[] m_enemyPrefab;
    [SerializeField] private GameObject m_bossPrefab;
    [SerializeField] private GameObject m_projectilePrefab;
    [SerializeField] private GameObject m_enemyProjectilePrefab;
    [SerializeField] private GameObject m_bossProjectilePrefab;
    [SerializeField] private GameObject m_gameOverPanel;
    [SerializeField] private GameObject m_statsPanel;
    [SerializeField] private GameObject m_shadowPanel;
    [SerializeField] private GameObject m_shadowPanel2;
    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private TextMeshProUGUI m_newHighScoreText;
    [SerializeField] private int m_projectilePoolSize;
    [SerializeField] private float m_enemySpawnDelay = 2f;

    //
    private GameObject m_boss;
    private Queue<GameObject> m_projectilePool = new Queue<GameObject>();
    private Queue<GameObject> m_enemyProjectilePool = new Queue<GameObject>();
    private Queue<GameObject> m_bossProjectilePool = new Queue<GameObject>();
    private float m_maxYPosition = 6f;
    private float m_maxXPosition = 1.5f;
    private float m_timeBeforeBossSpawn = 10f;
    private float m_delayBeforeBossSpawn = 5f;
    private bool m_hasReachNewHighScore;

    private void Awake()
    {
        m_hasReachNewHighScore = false;
        m_newHighScoreText.gameObject.SetActive(false);
        for (int i = 0; i < m_projectilePoolSize; i++)
        {
            StoreProjectileIntoPool();
            StoreEnemyProjectileIntoPool();
        }
        MakeBossExistFirst();
    }

    // Start is called before the first frame update
    void Start()
    {
        IsGameRunning = true;
        StartCoroutine(SpawnEnemy());
        StartCoroutine(WaitForSpawnBoss());
    }

    // Update is called once per frame
    void Update()
    {
        SetScore();
        if (IsBossExist)
        {
            m_shadowPanel.gameObject.SetActive(false);
            m_shadowPanel2.gameObject.SetActive(false);
            m_scoreText.gameObject.SetActive(false);
        }
        else
        {
            m_shadowPanel.gameObject.SetActive(true);
            m_shadowPanel2.gameObject.SetActive(true);
            m_scoreText.gameObject.SetActive(true);
        }
    }

    private void SetScore()
    {
        if (PlayerScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", PlayerScore);
            m_hasReachNewHighScore = true;
        }
        m_scoreText.text = "Score:\n" +  PlayerScore.ToString();
    }

    public void GameOver()
    {
        IsGameRunning = false;
        m_gameOverPanel.SetActive(true);
    }

    public void NextButton()
    {
        m_gameOverPanel.SetActive(false);
        if (m_hasReachNewHighScore)
        {
            m_newHighScoreText.gameObject.SetActive(true);
        }
        m_statsPanel.SetActive(true);
    }

    public void BackToMenuButton()
    {
        m_statsPanel.SetActive(false);
        SceneManager.LoadScene(0);
    }

    private IEnumerator TimeToSpawnBoss()
    {
        yield return new WaitForSeconds(m_delayBeforeBossSpawn);
        Vector2 bossSpawnLocation = new Vector2(0, m_maxYPosition);
        SpawnBoss(bossSpawnLocation, Quaternion.identity);
    }

    public IEnumerator WaitForSpawnBoss()
    {
        yield return new WaitForSeconds(m_timeBeforeBossSpawn);
        StartCoroutine(TimeToSpawnBoss());
        IsBossExist = true;
    }

    private IEnumerator SpawnEnemy()
    {
        while (IsGameRunning && !IsBossExist)
        {
            yield return new WaitForSeconds(m_enemySpawnDelay);

            int enemyIndex = Random.Range(0, m_enemyPrefab.Length);
            float randomXPosition = Random.Range(-m_maxXPosition, m_maxXPosition);
            Vector2 enemySpawnLocation = new Vector2(randomXPosition, m_maxYPosition);

            Instantiate(m_enemyPrefab[enemyIndex], enemySpawnLocation, Quaternion.identity);
        }
    }

    // Player Projectile Pool
    private GameObject StoreProjectileIntoPool()
    {
        GameObject projectile = Instantiate(m_projectilePrefab);
        projectile.gameObject.SetActive(false);
        m_projectilePool.Enqueue(projectile);

        return projectile;
    }

    public GameObject ShootProjectile(Vector2 position, Quaternion rotation)
    {
        GameObject projectile = m_projectilePool.Count > 0 ? m_projectilePool.Dequeue() : StoreProjectileIntoPool();
        projectile.gameObject.transform.SetPositionAndRotation(position, rotation);
        projectile.gameObject.SetActive(true);

        return projectile;
    }

    public void ReturnProjectileToPool(GameObject projectile)
    {
        projectile.gameObject.SetActive(false);
        m_projectilePool.Enqueue(projectile);
    }

    // Enemy Projectile Pool
    private GameObject StoreEnemyProjectileIntoPool()
    {
        GameObject enemyProjectile = Instantiate(m_enemyProjectilePrefab);
        enemyProjectile.gameObject.SetActive(false);
        m_enemyProjectilePool.Enqueue(enemyProjectile);

        return enemyProjectile;
    }

    public GameObject ShootEnemyProjectile(Vector2 position, Quaternion rotation)
    {
        GameObject enemyProjectile = m_enemyProjectilePool.Count > 0 ? m_enemyProjectilePool.Dequeue() : StoreEnemyProjectileIntoPool();
        enemyProjectile.gameObject.transform.SetPositionAndRotation(position, rotation);
        enemyProjectile.gameObject.SetActive(true);

        return enemyProjectile;
    }

    public void ReturnEnemyProjectileToPool(GameObject enemyProjectile)
    {
        enemyProjectile.gameObject.SetActive(false);
        m_enemyProjectilePool.Enqueue(enemyProjectile);
    }

    // Boss Pool
    private GameObject MakeBossExistFirst()
    {
        m_boss = Instantiate(m_bossPrefab);
        m_boss.gameObject.SetActive(false);

        return m_boss;
    }

    private GameObject SpawnBoss(Vector2 position, Quaternion rotation)
    {
        m_boss.gameObject.transform.SetPositionAndRotation(position, rotation);
        m_boss.gameObject.SetActive(true);

        return m_boss;
    }

    public void DeactivateBoss()
    {
        m_boss.gameObject.SetActive(false);
    }

    // Boss Projectile Pool
    private GameObject StoreBossProjectileIntoPool()
    {
        GameObject bossProjectile = Instantiate(m_bossProjectilePrefab);
        bossProjectile.gameObject.SetActive(false);
        m_bossProjectilePool.Enqueue(bossProjectile);

        return bossProjectile;
    }

    public GameObject ShootBossProjectile(Vector2 position, Quaternion rotation)
    {
        GameObject bossProjectile = m_bossProjectilePool.Count > 0 ? m_bossProjectilePool.Dequeue() : StoreBossProjectileIntoPool();
        bossProjectile.transform.SetPositionAndRotation(position, rotation);
        bossProjectile.gameObject.SetActive(true);

        return bossProjectile;
    }

    public void ReturnBossProjectileToPool(GameObject bossProjectile)
    {
        bossProjectile.gameObject.SetActive(false);
        m_bossProjectilePool.Enqueue(bossProjectile);
    }
}