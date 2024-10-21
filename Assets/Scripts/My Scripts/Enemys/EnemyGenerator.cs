using UnityEngine;
using System.Collections.Generic;

public class EnemyGenerator : MonoBehaviour
{
    public float encounterRate = 0.1f; // Encounter rate
    public List<Vector2> areaMinList; // List of area minimum coordinates
    public List<Vector2> areaMaxList; // List of area maximum coordinates

    void Update()
    {
        if (Random.value < encounterRate * Time.deltaTime)
        {
            GenerateEnemy();
        }
    }

    void GenerateEnemy()
    {
        int areaIndex = Random.Range(0, areaMinList.Count);
        float xPos = Random.Range(areaMinList[areaIndex].x, areaMaxList[areaIndex].x);
        float yPos = Random.Range(areaMinList[areaIndex].y, areaMaxList[areaIndex].y);
        Vector3 spawnPosition = new Vector3(xPos, yPos, 0);

        GameObject enemy = ObjectPool.instance.GetPooledObject();
        enemy.transform.position = spawnPosition;
        enemy.SetActive(true);
    }
}
