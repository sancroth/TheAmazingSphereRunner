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
    private int m_TargetPosX = 1;
    private float m_LastPosX;
    private float m_Timer = 0f;
    private float m_Percent;
    private float m_PosDiff;
    private bool m_BallMoving = false;
    private int m_MovingDirection;
    private bool m_BufferMoveSet = false;
    private int m_BufferMoveDirection;

    private const int DIRECTION_LEFT = -1;
    private const int DIRECTION_RIGHT = 1;

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
        if ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.A)))
        {
            if (!m_BallMoving)
            {
                if ((Input.GetKeyDown(KeyCode.D)))
                {
                    PrepareMove(DIRECTION_RIGHT);
                }
                else if ((Input.GetKeyDown(KeyCode.A)))
                {
                    PrepareMove(DIRECTION_LEFT);
                }
            }
            else
            {
                //only buffer at 1/2 of the move time
                if(m_Timer >= timeToMove / 2)
                {
                    if ((Input.GetKeyDown(KeyCode.D)))
                    {
                        StoreBufferedMove(DIRECTION_RIGHT);

                    }
                    else if ((Input.GetKeyDown(KeyCode.A)))
                    {
                        StoreBufferedMove(DIRECTION_LEFT);
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

        m_HorizontalPosition = m_LastPosX + m_PosDiff * m_Percent;

        transform.position = new Vector3(
            m_HorizontalPosition,
            transform.position.y,
            transform.position.z);

        if (m_Timer >= timeToMove)
        {
            // fix minor calculation mistake if any
            if(transform.position.x != AvailablePositions.FixedPositionsX[m_TargetPosX])
            {
                transform.position = new Vector3(
                    AvailablePositions.FixedPositionsX[m_TargetPosX],
                    transform.position.y,
                    transform.position.z);
            }
            m_Timer = 0f;
            m_BallMoving = false;
            if (m_BufferMoveSet)
            {
                PrepareMove(m_BufferMoveDirection);
                // release buffer
                m_BufferMoveSet = !m_BufferMoveSet;
            }   
        }
    }

    public void PrepareMove(int direction)
    {
        if (direction==DIRECTION_RIGHT)
        {
            if (m_TargetPosX != AvailablePositions.FixedPositionsX.Length-1)
            {
                m_TargetPosX++;
                m_MovingDirection = direction;
                m_BallMoving = true;
                m_LastPosX = transform.position.x;
                m_PosDiff = CalculateNextTargetX();
            }
        }
        else if (direction== DIRECTION_LEFT)
        {
            if (m_TargetPosX != 0)
            {
                m_TargetPosX--;
                m_MovingDirection = direction;
                m_BallMoving = true;
                m_LastPosX = transform.position.x;
                m_PosDiff = CalculateNextTargetX();
            }
        }
    }

    public void StoreBufferedMove(int direction)
    {
        if (direction == DIRECTION_RIGHT)
        {
            m_BufferMoveSet = true;
            m_BufferMoveDirection = direction;
        }
        else if (direction == DIRECTION_LEFT)
        {
            m_BufferMoveSet = true;
            m_BufferMoveDirection = direction;
        }
    }

    public float CalculateNextTargetX()
    {
        float PosDiff;

        if (transform.position.x > 0)
        {
            if (m_MovingDirection == 1)
            {
                PosDiff = AvailablePositions.FixedPositionsX[m_TargetPosX] - transform.position.x;
            }
            else
            {
                PosDiff = transform.position.x * -1f;
            }
        }
        else if (transform.position.x < 0)
        {
            if (m_MovingDirection == -1)
            {
                PosDiff = AvailablePositions.FixedPositionsX[m_TargetPosX] - transform.position.x;
            }
            else
            {
                PosDiff = transform.position.x * -1f;
            }
        }
        else
        {
            PosDiff = AvailablePositions.FixedPositionsX[m_TargetPosX];
        }
        return PosDiff;
    }
}
