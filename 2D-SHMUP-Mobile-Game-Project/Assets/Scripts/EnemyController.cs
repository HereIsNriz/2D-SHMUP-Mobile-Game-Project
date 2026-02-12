using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Property

    // SerializeField
    [SerializeField] private GameObject m_enemyProjectileOut;
    [SerializeField] private int m_lives;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_shootDelay = 1.2f;
    [SerializeField] private int m_enemyScore;
    [SerializeField] private bool m_isBoss;
    [SerializeField] private bool m_canShoot;
    [SerializeField] private bool m_isWeakEnemy;

    // Field
    private Rigidbody2D m_enemyRb;
    private GameManager m_gameManager;
    private PlayerController m_player;
    private float m_bottomScreen = -5f;
    private float m_bossMaxPosition = 3f;
    private float m_boosSlidingPosition = 1.3f;
    private float m_slidingSpeed = 80f;
    private int m_turningPoint = 1;
    private bool m_hasMoving = false;

    // Start is called before the first frame update
    private void Start()
    {
        m_enemyRb = GetComponent<Rigidbody2D>();
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_player = GameObject.Find("Player").GetComponent<PlayerController>();

        if (m_canShoot)
        {
            StartCoroutine(ShootEnemyProjectile());
        }

        if (m_isBoss)
        {
            StartCoroutine(ShootBossProjectile());
        }
    }

    // Update is called once per frame
    private void Update()
    {
        WeakEnemyBehaviour();
        MediumEnemyBehaviour();
        StrongEnemyBehaviour();
        BossBehaviour();
        if (transform.position.y <= m_bottomScreen)
        {
            Destroy(gameObject);
        }

        if (m_lives <= 0)
        {
            Destroy(gameObject);
            m_gameManager.PlayerScore += m_enemyScore;
        }
    }

    private void FixedUpdate()
    {
        if (m_gameManager.IsGameRunning)
        {
            MoveEnemy();
        }
    }

    private void MoveEnemy()
    {
        m_enemyRb.velocity = Vector2.down * m_speed * Time.deltaTime;

        if (transform.position.y <= m_bossMaxPosition && m_isBoss)
        {
            m_enemyRb.velocity = Vector2.down * 0;
            m_enemyRb.velocity = Vector2.right * m_slidingSpeed * m_turningPoint * Time.deltaTime;
        }
    }

    private void WeakEnemyBehaviour()
    {
        if (m_isWeakEnemy)
        {
            if (transform.position.y <= m_bottomScreen)
            {
                m_gameManager.ReturnWeakEnemyToPool(this.gameObject);
                m_player.Lives--;
            }

            if (m_lives <= 0)
            {
                m_gameManager.ReturnWeakEnemyToPool(this.gameObject);
                m_gameManager.PlayerScore += m_enemyScore;
            }
        }
    }
    private void MediumEnemyBehaviour()
    {

    }
    private void StrongEnemyBehaviour()
    {
        if (m_canShoot)
        {
            
        }
    }
    private void BossBehaviour()
    {
        if (m_isBoss)
        {
            if (m_lives <= 0)
            {
                m_gameManager.DeactivateBoss();
                m_gameManager.IsBossExist = false;
                m_gameManager.WaitForSpawnBoss();
            }

            if (transform.position.x >= m_boosSlidingPosition || transform.position.x <= -m_boosSlidingPosition)
            {
                if (!m_hasMoving)
                {
                    m_turningPoint *= -1;
                    m_hasMoving = true;
                }
            }
        }
    }

    private IEnumerator ShootEnemyProjectile()
    {
        while (m_gameManager.IsGameRunning)
        {
            yield return new WaitForSeconds(m_shootDelay);
            m_gameManager.ShootEnemyProjectile(m_enemyProjectileOut.transform.position, Quaternion.identity);
        }
    }

    private IEnumerator ShootBossProjectile()
    {
        while (m_gameManager.IsGameRunning)
        {
            yield return new WaitForSeconds(m_shootDelay);
            m_gameManager.ShootBossProjectile(m_enemyProjectileOut.transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            m_gameManager.ReturnProjectileToPool(collision.gameObject);
            m_lives--;
        }

        if (collision.gameObject.CompareTag("Projectile") && m_isWeakEnemy)
        {
            m_lives--;
        }

        if (m_isWeakEnemy && collision.gameObject.CompareTag("Player"))
        {
            m_gameManager.ReturnWeakEnemyToPool(this.gameObject);
            m_player.Lives--;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            m_player.Lives--;
        }

        if (m_isBoss && collision.gameObject.CompareTag("Sensor"))
        {
            m_hasMoving = false;
        }
    }
}
