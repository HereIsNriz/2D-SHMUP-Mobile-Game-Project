using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    //

    //
    [SerializeField] private float speed;

    //
    private Rigidbody2D projectileRb;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        projectileRb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= 6f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (gameManager.IsGameRunning)
        {
            MoveProjectile();
        }
    }

    private void MoveProjectile()
    {
        projectileRb.velocity = transform.up * speed * Time.deltaTime;
    }
}
