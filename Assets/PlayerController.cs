using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //public HudManager hudManager;
    public float timeToMove;

    private Stats m_Stats;
    private float m_HorizontalPosition;
    private float[] m_FixedPositionsX = new float[] { -2f, 0f, 2f };
    private int m_TargetPosX = 1;
    private float m_Timer = 0f;
    private float m_Percent;
    private float m_PosDiff;
    private bool m_BallMoving = false;
    private int m_MovingDirection;


    private void Awake()
    {
        m_Stats = GetComponent<Stats>();
        //hudManager.UpdateHealthText(m_Stats.health);
    }

    private void Update()
    {
        if (m_Stats.health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (!m_BallMoving)
        {
            if ((Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.A)))
            {
                m_BallMoving = true;
                if ((Input.GetKey(KeyCode.D)))
                {
                    if (m_TargetPosX != 2)
                    {
                        m_TargetPosX++;
                        m_MovingDirection = 1;
                    }
                }
                else if ((Input.GetKey(KeyCode.A)))
                {
                    if (m_TargetPosX != 0)
                    {
                        m_TargetPosX--;
                        m_MovingDirection = -1;
                    }
                }
            }
        }

        //float playerSize = transform.localScale.x / 2;
        if (m_Timer < timeToMove && m_BallMoving)
        {
            MovePlayer();
        }
    }

    public void ReceiveDamage()
    {
        m_Stats.UpdateHealth(10);
        //hudManager.UpdateHealthText(m_Stats.health);
    }

    public void MovePlayer() 
    {
        m_Timer += Time.deltaTime;
        m_Percent = m_Timer / timeToMove;

        if (transform.position.x > 0)
        {
            if (m_MovingDirection == 1)
            {
                m_PosDiff = m_FixedPositionsX[m_TargetPosX] - transform.position.x;
            }
            else
            {
                m_PosDiff = transform.position.x * -1f;
            }
        }
        else if (transform.position.x < 0)
        {
            if (m_MovingDirection == -1)
            {
                m_PosDiff = m_FixedPositionsX[m_TargetPosX] - transform.position.x;
            }
            else
            {
                m_PosDiff = transform.position.x * -1f;
            }
        }
        else
        {
            m_PosDiff = m_FixedPositionsX[m_TargetPosX];
        }
        m_HorizontalPosition = transform.position.x + m_PosDiff * m_Percent;
        Debug.Log("Position to add:");
        Debug.Log(m_PosDiff * m_Percent);
        Debug.Log("Current pos after addition:");
        Debug.Log(m_HorizontalPosition);

        transform.position = new Vector3(
            m_HorizontalPosition,
            transform.position.y,
            transform.position.z);

        Debug.Log(m_Timer);

        if (transform.position.x == m_FixedPositionsX[m_TargetPosX] && m_Timer >= timeToMove)
        {
            m_Timer = 0f;
            m_BallMoving = false;
        }
    }
}
