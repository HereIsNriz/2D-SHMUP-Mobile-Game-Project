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
    [SerializeField] private GameObject m_weakEnemyPrefab;
    [SerializeField] private GameObject m_mediumEnemyPrefab;
    [SerializeField] private GameObject m_strongEnemyPrefab;
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
    private Queue<GameObject> m_weakEnemyPool = new Queue<GameObject>();
    private Queue<GameObject> m_mediumEnemyPool = new Queue<GameObject>();
    private Queue<GameObject> m_strongEnemyPool = new Queue<GameObject>();
    private Queue<GameObject> m_bossProjectilePool = new Queue<GameObject>();
    private float m_maxYPosition = 6f;
    private float m_maxXPosition = 1.5f;
    private float m_timeBeforeBossSpawn = 10f;
    private float m_delayBeforeBossSpawn = 5f;
    private bool m_hasReachNewHighScore;
    private bool m_isEnemySpawning;
    private int m_minRandomValue = 0;
    private int m_maxRandomValue = 3;

    private void Awake()
    {
        m_hasReachNewHighScore = false;
        m_newHighScoreText.gameObject.SetActive(false);
        for (int i = 0; i < m_projectilePoolSize; i++)
        {
            StoreProjectileIntoPool();
            StoreEnemyProjectileIntoPool();
            StoreBossProjectileIntoPool();
            StoreWeakEnemyIntoPool();
            StoreMediumEnemyIntoPool();
            StoreStrongEnemyIntoPool();
        }
        MakeBossExistFirst();
    }

    // Start is called before the first frame update
    void Start()
    {
        IsGameRunning = true;
        StartCoroutine(SpawnEnemies());
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
            if (!m_isEnemySpawning)
            {
                StartCoroutine(SpawnEnemies());
            }
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

    private IEnumerator WaitForSpawnBoss()
    {
        yield return new WaitForSeconds(m_timeBeforeBossSpawn);
        StartCoroutine(TimeToSpawnBoss());
        IsBossExist = true;
        m_isEnemySpawning = false;
    }

    public void WaitForBossToSpawn()
    {
        StartCoroutine(WaitForSpawnBoss());
    }

    public IEnumerator SpawnEnemies()
    {
        m_isEnemySpawning = true;
        while (IsGameRunning && !IsBossExist)
        {
            yield return new WaitForSeconds(m_enemySpawnDelay);

            int enemyIndex = Random.Range(m_minRandomValue, m_maxRandomValue);
            float randomXPosition = Random.Range(-m_maxXPosition, m_maxXPosition);
            Vector2 enemySpawnLocation = new Vector2(randomXPosition, m_maxYPosition);

            switch (enemyIndex)
            {
                case 0:
                    SpawnWeakEnemy(enemySpawnLocation, Quaternion.identity);
                    break;
                case 1:
                    SpawnMediumEnemy(enemySpawnLocation, Quaternion.identity);
                    break;
                case 2:
                    SpawnStrongEnemy(enemySpawnLocation, Quaternion.identity);
                    break;
            }
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

    // Weak Enemy Pool
    private GameObject StoreWeakEnemyIntoPool()
    {
        GameObject weakEnemy = Instantiate(m_weakEnemyPrefab);
        m_weakEnemyPrefab.gameObject.SetActive(false);
        m_weakEnemyPool.Enqueue(weakEnemy);

        return weakEnemy;
    }

    private GameObject SpawnWeakEnemy(Vector2 position, Quaternion rotation)
    {
        GameObject weakEnemy = m_weakEnemyPool.Count > 0 ? m_weakEnemyPool.Dequeue() : StoreWeakEnemyIntoPool();
        weakEnemy.gameObject.transform.SetPositionAndRotation(position, rotation);
        weakEnemy.gameObject.SetActive(true);

        return weakEnemy;
    }

    public void ReturnWeakEnemyToPool(GameObject weakEnemy)
    {
        weakEnemy.gameObject.SetActive(false);
        m_weakEnemyPool.Enqueue(weakEnemy);
    }

    // Medium Enemy Pool
    private GameObject StoreMediumEnemyIntoPool()
    {
        GameObject mediumEnemy = Instantiate(m_mediumEnemyPrefab);
        mediumEnemy.gameObject.SetActive(false);
        m_mediumEnemyPool.Enqueue(mediumEnemy);

        return mediumEnemy;
    }

    private GameObject SpawnMediumEnemy(Vector2 position, Quaternion rotation)
    {
        GameObject mediumEnemy = m_mediumEnemyPool.Count > 0 ? m_mediumEnemyPool.Dequeue() : StoreMediumEnemyIntoPool();
        mediumEnemy.gameObject.transform.SetPositionAndRotation(position, rotation);
        mediumEnemy.gameObject.SetActive(true);

        return mediumEnemy;
    }

    public void ReturnMediumEnemyToPool(GameObject mediumEnemy)
    {
        mediumEnemy.gameObject.SetActive(false);
        m_mediumEnemyPool.Enqueue(mediumEnemy);
    }

    // Strong Enemy Pool
    private GameObject StoreStrongEnemyIntoPool()
    {
        GameObject strongEnemy = Instantiate(m_strongEnemyPrefab);
        strongEnemy.gameObject.SetActive(false);
        m_strongEnemyPool.Enqueue(strongEnemy);

        return strongEnemy;
    }

    private GameObject SpawnStrongEnemy(Vector2 position, Quaternion rotation)
    {
        GameObject strongEnemy = m_strongEnemyPool.Count > 0 ? m_strongEnemyPool.Dequeue() : StoreStrongEnemyIntoPool();
        strongEnemy.gameObject.transform.SetPositionAndRotation(position, rotation);
        strongEnemy.gameObject.SetActive(true);

        return strongEnemy;
    }

    public void ReturnStrongEnemyToPool(GameObject strongEnemy)
    {
        strongEnemy.gameObject.SetActive(false);
        m_strongEnemyPool.Enqueue(strongEnemy);
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