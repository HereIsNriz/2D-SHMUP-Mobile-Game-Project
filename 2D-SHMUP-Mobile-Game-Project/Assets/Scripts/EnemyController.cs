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
    [SerializeField] private bool m_isMediumEnemy;

    // Field
    private Rigidbody2D m_enemyRb;
    private GameManager m_gameManager;
    private PlayerController m_player;
    private float m_bottomScreen = -5f;
    private float m_bossMaxPosition = 3f;
    private float m_boosSlidingPosition = 1.3f;
    private float m_slidingSpeed = 80f;
    private int m_turningPoint = 1;
    private int m_startingLives;
    private bool m_hasMoving = false;
    private bool m_isStrongEnemyShooting;
    private bool m_isBossShooting;

    // Start is called before the first frame update
    private void Start()
    {
        m_startingLives = m_lives;
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

        if (m_canShoot && !m_isStrongEnemyShooting && gameObject.activeInHierarchy)
        {
            StartCoroutine(ShootEnemyProjectile());
        }

        if (m_isBoss && !m_isBossShooting && gameObject.activeInHierarchy)
        {
            StartCoroutine(ShootBossProjectile());
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
            }

            if (m_lives <= 0)
            {
                m_lives = m_startingLives;
                m_gameManager.ReturnWeakEnemyToPool(this.gameObject);
                m_gameManager.PlayerScore += m_enemyScore;
            }
        }
    }
    private void MediumEnemyBehaviour()
    {
        if (m_isMediumEnemy)
        {
            if (transform.position.y <= m_bottomScreen)
            {
                m_gameManager.ReturnMediumEnemyToPool(this.gameObject);
            }

            if (m_lives <= 0)
            {
                m_lives = m_startingLives;
                m_gameManager.ReturnMediumEnemyToPool(this.gameObject);
                m_gameManager.PlayerScore += m_enemyScore;
            }
        }
    }
    private void StrongEnemyBehaviour()
    {
        if (m_canShoot)
        {
            if (transform.position.y <= m_bottomScreen)
            {
                m_gameManager.ReturnStrongEnemyToPool(this.gameObject);
            }

            if (m_lives <= 0)
            {
                m_lives = m_startingLives;
                m_gameManager.ReturnStrongEnemyToPool(this.gameObject);
                m_gameManager.PlayerScore += m_enemyScore;
                m_isStrongEnemyShooting = false;
            }
        }
    }
    private void BossBehaviour()
    {
        if (m_isBoss)
        {
            if (m_lives <= 0)
            {
                m_lives = m_startingLives;
                m_gameManager.DeactivateBoss();
                m_gameManager.IsBossExist = false;
                m_gameManager.WaitForBossToSpawn();
                m_gameManager.PlayerScore += m_enemyScore;
                m_isBossShooting = false;
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
        m_isStrongEnemyShooting = true;
        while (m_gameManager.IsGameRunning)
        {
            yield return new WaitForSeconds(m_shootDelay);
            m_gameManager.ShootEnemyProjectile(m_enemyProjectileOut.transform.position, Quaternion.identity);
        }
    }

    private IEnumerator ShootBossProjectile()
    {
        m_isBossShooting = true;
        while (m_gameManager.IsGameRunning)
        {
            yield return new WaitForSeconds(m_shootDelay);
            m_gameManager.ShootBossProjectile(m_enemyProjectileOut.transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && !m_isWeakEnemy)
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

        if (m_isMediumEnemy && collision.gameObject.CompareTag("Player"))
        {
            m_gameManager.ReturnMediumEnemyToPool(this.gameObject);
            m_player.Lives--;
        }

        if (m_canShoot && collision.gameObject.CompareTag("Player"))
        {
            m_gameManager.ReturnStrongEnemyToPool(this.gameObject);
            m_player.Lives--;
        }

        if (m_isBoss && collision.gameObject.CompareTag("Player"))
        {
            m_player.Lives--;
        }

        if (m_isBoss && collision.gameObject.CompareTag("Sensor"))
        {
            m_hasMoving = false;
        }
    }
}
