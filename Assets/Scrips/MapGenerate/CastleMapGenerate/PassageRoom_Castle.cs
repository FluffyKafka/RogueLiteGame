using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassageRoom_Castle : Room_Castle
{
    public float difficulty;
    public override void GenerateRoom(MapGenerater_Castle _generater)
    {
        base.GenerateRoom(_generater);

        float currentEnemyDifficulty = 0;
        while (currentEnemyDifficulty < difficulty)
        {
            Vector2 randomPosition = flatPositions[UnityEngine.Random.Range(0, flatPositions.Count)];
            randomPosition = new Vector2(randomPosition.x, randomPosition.y + generater.enemyYOffeset);
            GameObject randomEnemy = generater.enemyPrefabList[UnityEngine.Random.Range(0, generater.enemyPrefabList.Count)];

            Enemy newEnemy =
                Instantiate(randomEnemy, randomPosition, Quaternion.identity).GetComponent<Enemy>();
            currentEnemyDifficulty += newEnemy.difficulty;
        }
    }
}
