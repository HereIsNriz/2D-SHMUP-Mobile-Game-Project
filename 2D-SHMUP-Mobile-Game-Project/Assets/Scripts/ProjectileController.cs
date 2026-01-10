using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    //

    //
    [SerializeField] private float m_speed;

    //
    private Rigidbody2D m_projectileRb;
    private GameManager m_gameManager;

    // Start is called before the first frame update
    void Start()
    {
        m_projectileRb = GetComponent<Rigidbody2D>();
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= 6f)
        {
            m_gameManager.ReturnProjectileToPool(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (m_gameManager.IsGameRunning)
        {
            MoveProjectile();
        }
    }

    private void MoveProjectile()
    {
        m_projectileRb.velocity = transform.up * m_speed * Time.deltaTime;
    }
}
