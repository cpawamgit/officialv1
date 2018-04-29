using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{

    private Transform target;
    private int wavePointIndex = 0;
 

    private Enemy enemy;


    void OnEnable()
    {
        enemy = GetComponent<Enemy>();

        target = Waypoints.points[0];

        wavePointIndex = 0;

}

void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }

        //enemy.speed = enemy.startSpeed;
    }


    void GetNextWaypoint()
    {
        if (wavePointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            return;
        }

        wavePointIndex++;
        target = Waypoints.points[wavePointIndex];
    }


    void EndPath()
    {
        PlayerStats.DecreaseLife();
        WaveSpawner.EnemiesAlive--;
        MyObjectPooler.Instance.ReturnToPool(gameObject);
    }
}
