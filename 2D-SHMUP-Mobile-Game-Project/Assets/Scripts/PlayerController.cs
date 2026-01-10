using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //
    public int Lives;

    //
    [SerializeField] private GameObject m_playerProjectile;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_shootDelay = 1f;

    //
    private Rigidbody2D m_playerRb;
    private GameManager m_gameManager;
    private GameObject m_projectileOut;
    private Vector3 m_touchPosition;
    private bool m_isScreenTouched;
    private float m_xPosition = 1.8f;
    private float m_minYPosition = -3.5f;
    private float m_maxYPosition = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        m_playerRb = GetComponent<Rigidbody2D>();
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_projectileOut = GameObject.Find("Projectile Out");
        m_isScreenTouched = false;
        StartCoroutine(ShootProjectile());
    }

    // Update is called once per frame
    void Update()
    {
        if (m_gameManager.IsGameRunning)
        {
            DetectTouchInput();
            MovePlayer();
        }
    }

    private void DetectTouchInput()
    {
        m_touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_touchPosition.z = 0;

        if (Input.GetMouseButtonDown(0))
        {
            m_isScreenTouched = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_isScreenTouched = false;
        }
    }

    private void MovePlayer()
    {
        if (Input.GetMouseButton(0) && m_isScreenTouched)
        {
            transform.position = Vector2.MoveTowards(m_playerRb.position, m_touchPosition, m_speed * Time.deltaTime);

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -m_xPosition, m_xPosition),
                Mathf.Clamp(transform.position.y, m_minYPosition, m_maxYPosition),
                0);
        }
    }

    private IEnumerator ShootProjectile()
    {
        while (m_gameManager.IsGameRunning)
        {
            yield return new WaitForSeconds(m_shootDelay);

            m_gameManager.ShootProjectile(m_projectileOut.transform.position, Quaternion.identity);
        }
    }
}
