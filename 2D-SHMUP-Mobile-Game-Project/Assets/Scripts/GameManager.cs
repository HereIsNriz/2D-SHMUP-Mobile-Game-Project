using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //
    public bool IsGameRunning;

    //
    [SerializeField] private GameObject[] m_enemyPrefab;
    [SerializeField] private GameObject m_projectilePrefab;
    [SerializeField] private int m_projectilePoolSize;

    //
    private Queue<GameObject> m_projectilePool = new Queue<GameObject>();
    [SerializeField] private float m_enemySpawnDelay = 2f;
    private float m_maxYPosition = 6f;
    private float m_maxXPosition = 1.5f;

    private void Awake()
    {
        for (int i = 0; i < m_projectilePoolSize; i++)
        {
            StoreProjectileIntoPool();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        IsGameRunning = true;
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator SpawnEnemy()
    {
        while (IsGameRunning)
        {
            yield return new WaitForSeconds(m_enemySpawnDelay);

            int enemyIndex = Random.Range(0, m_enemyPrefab.Length);
            float randomXPosition = Random.Range(-m_maxXPosition, m_maxXPosition);
            Vector2 enemySpawnLocation = new Vector2(randomXPosition, m_maxYPosition);

            Instantiate(m_enemyPrefab[enemyIndex], enemySpawnLocation, Quaternion.identity);
        }
    }

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
}
