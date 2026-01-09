using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //
    public bool IsGameRunning;

    //
    [SerializeField] private GameObject[] m_enemyPrefab;

    //
    private float m_enemySpawnDelay = 2f;
    private float m_maxYPosition = 6f;
    private float m_maxXPosition = 1.5f;

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
}
