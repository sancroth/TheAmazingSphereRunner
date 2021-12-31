using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject catcherPrefab;
    public GameObject Player;
    public float spawnEnemyWindow = 1.5f;
    public float spawnCatcherWindow = 1.5f;
    private int m_NextEnemyPosition;
    private int m_NextCatcherPosition;
    private float m_SpawnPosZ;


    // Start is called before the first frame update
    void Start()
    {
        m_SpawnPosZ = Player.transform.position.x + 50f;
        InvokeRepeating(nameof(SpawnInteractables), 0f, spawnEnemyWindow);
    }

    private void SpawnInteractables()
    {
        (m_NextEnemyPosition, m_NextCatcherPosition) = CalculateNextPawnPoints();
        SpawnEnemy(EnemyType.Catcher, m_NextEnemyPosition);
        SpawnEnemy(EnemyType.Evader, m_NextCatcherPosition);
    }

    private void SpawnEnemy(EnemyType enemyType, int index )
    {

        Vector3 spawnPosition = new Vector3(
            AvailablePositions.FixedPositionsX[index],
            enemyPrefab.transform.position.y,
            m_SpawnPosZ);

        if (enemyType == EnemyType.Evader)
        {
            Instantiate(
                enemyPrefab,
                spawnPosition,
                enemyPrefab.transform.rotation);
        }
        else
        {
            Instantiate(
                catcherPrefab,
                spawnPosition,
                catcherPrefab.transform.rotation);
        }
    }

    private (int, int) CalculateNextPawnPoints()
    {
        int getRanNum1 = Random.Range(0, AvailablePositions.FixedPositionsX.Length);
        int getRanNum2 = Random.Range(0, AvailablePositions.FixedPositionsX.Length);
        while (getRanNum2 == getRanNum1)
        {
            getRanNum2 = Random.Range(0, AvailablePositions.FixedPositionsX.Length);
        }
        return (getRanNum1, getRanNum2);
    }
}
