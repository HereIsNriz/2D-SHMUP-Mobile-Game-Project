using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    //

    //
    [SerializeField] private float m_speed;
    [SerializeField] private bool m_isEnemyProjectile;

    //
    private Rigidbody2D m_projectileRb;
    private GameManager m_gameManager;
    private PlayerController m_player;
    private float m_projectileBoundary = 6f;

    // Start is called before the first frame update
    void Start()
    {
        m_projectileRb = GetComponent<Rigidbody2D>();
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= m_projectileBoundary)
        {
            m_gameManager.ReturnProjectileToPool(this.gameObject);
        }

        if (transform.position.y <= -m_projectileBoundary)
        {
            if (m_isEnemyProjectile)
            {
                m_gameManager.ReturnEnemyProjectileToPool(this.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_gameManager.IsGameRunning)
        {
            if (m_isEnemyProjectile)
            {
                MoveProjectileDown();
            }
            else
            {
                MoveProjectileUp();
            }
        }
    }

    private void MoveProjectileUp()
    {
        m_projectileRb.velocity = transform.up * m_speed * Time.deltaTime;
    }

    private void MoveProjectileDown()
    {
        m_projectileRb.velocity = Vector2.down * m_speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_isEnemyProjectile)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                m_gameManager.ReturnEnemyProjectileToPool(this.gameObject);
                m_player.Lives--;
            }
        }
    }
}
