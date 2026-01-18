using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //

    //
    [SerializeField] private GameObject m_enemyProjectileOut;
    [SerializeField] protected int m_lives;
    [SerializeField] protected float m_speed;
    [SerializeField] protected bool m_isBoss;
    [SerializeField] protected bool m_canShoot;

    //
    protected Rigidbody2D m_enemyRb;
    protected GameManager m_gameManager;
    protected PlayerController m_player;
    protected float m_bottomScreen = -5f;
    protected float m_bossMaxPosition = 3f;
    protected float m_shootDelay = 1.2f;

    // Start is called before the first frame update
    protected void Start()
    {
        m_enemyRb = GetComponent<Rigidbody2D>();
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_player = GameObject.Find("Player").GetComponent<PlayerController>();

        if (m_canShoot)
        {
            StartCoroutine(ShootEnemyProjectile());
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        if (transform.position.y <= m_bottomScreen)
        {
            Destroy(gameObject);
            m_player.Lives--;
        }

        if (m_lives <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected void FixedUpdate()
    {
        if (m_gameManager.IsGameRunning)
        {
            MoveEnemy();
        }
    }

    protected void MoveEnemy()
    {
        m_enemyRb.velocity = Vector2.down * m_speed * Time.deltaTime;

        if (transform.position.y <= m_bossMaxPosition && m_isBoss)
        {
            m_enemyRb.velocity = Vector3.zero;
        }
    }

    protected IEnumerator ShootEnemyProjectile()
    {
        while (m_gameManager.IsGameRunning)
        {
            yield return new WaitForSeconds(m_shootDelay);

            m_gameManager.ShootEnemyProjectile(m_enemyProjectileOut.transform.position, Quaternion.identity);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            m_gameManager.ReturnProjectileToPool(collision.gameObject);
            m_lives--;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            m_player.Lives--;
        }
    }
}
