using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnvironment : MonoBehaviour
{
    //

    //
    [SerializeField] private float m_speed;
    //
    private Rigidbody2D m_envinronmentRb;
    private GameManager m_gameManager;
    private Vector2 m_startPosition;
    private float m_resetPosition = -2f;

    // Start is called before the first frame update
    void Start()
    {
        m_envinronmentRb = GetComponent<Rigidbody2D>();
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= m_resetPosition)
        {
            transform.position = m_startPosition;
        }
    }

    private void FixedUpdate()
    {
        if (m_gameManager.IsGameRunning)
        {
            MoveGroundAndFloor();
        }
    }

    private void MoveGroundAndFloor()
    {
        m_envinronmentRb.velocity = Vector2.down * m_speed * Time.deltaTime;
    }
}
