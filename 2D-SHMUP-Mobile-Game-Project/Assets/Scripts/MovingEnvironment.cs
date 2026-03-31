using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnvironment : MonoBehaviour
{
    //

    //
    [SerializeField] private float m_speed;
    [SerializeField] private bool m_isDroppedItem;

    //
    private Rigidbody2D m_envinronmentRb;
    private GameManager m_gameManager;
    private Vector2 m_startPosition;
    private float m_resetPosition = -2f;
    private float m_bottomScreen = -5.5f;

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
        if (!m_isDroppedItem)
        {
            ResetPosition();
        }
        else
        {
            SpinDroppedItem();
            ReturnDroppedItemIntoTheirPool();
        }
    }

    private void FixedUpdate()
    {
        MoveAndStopTheEnvironment();
    }

    private void ResetPosition()
    {
        if (transform.position.y <= m_resetPosition)
        {
            transform.position = m_startPosition;
        }
    }

    private void MoveGroundAndFloor()
    {
        m_envinronmentRb.velocity = Vector2.down * m_speed * Time.deltaTime;
    }

    private void MoveAndStopTheEnvironment()
    {
        if (m_gameManager.IsBossExist)
        {
            m_envinronmentRb.velocity = Vector3.zero;
        }
        else
        {
            if (m_gameManager.IsGameRunning)
            {
                MoveGroundAndFloor();
            }
            else
            {
                m_envinronmentRb.velocity = Vector3.zero;
            }
        }
    }

    private void SpinDroppedItem()
    {
        transform.Rotate(Vector2.up * Time.deltaTime * m_speed * 3);
    }

    private void ReturnDroppedItemIntoTheirPool()
    {
        if (transform.position.y <= m_bottomScreen)
        {
            if (gameObject.tag == "Coin")
            {
                m_gameManager.ReturnCoinToPool(this.gameObject);
            }

            if (gameObject.tag == "Heal")
            {
                m_gameManager.ReturnHealthRegenToPool(this.gameObject);
            }
        }
    }
}
