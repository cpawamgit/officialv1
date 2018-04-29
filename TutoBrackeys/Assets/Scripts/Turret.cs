using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    protected Transform target;
    protected IDamageable targetEnemy;
    private SphereCollider sphereCollider;
    

    [Header("General")]

    public float range = 15f;
    public Alignement alignement;

    [Header("Use Bullets (default)")]
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    private float fireCountdown = 0f;
    

    [Header("Unity Setup Fields")]

    public string enemyTag = "Enemy";

    public Transform partToRotate;
    public float turnSpeed = 10f;

    public Transform firePoint;

    protected List<GameObject> enemies;
    protected List<GameObject> allies;




    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = range;
        enemies = new List<GameObject>();
        allies = new List<GameObject>();
    }



    void Start()
    {
       InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }


        LockOnTarget();

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f/fireRate;
            }

            fireCountdown -= Time.deltaTime;
    }


    protected void LockOnTarget()
    {
        Vector3 dir = (target.position - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }


    void Shoot ()
    {
        GameObject bulletGO = MyObjectPooler.Instance.SpawnFromPool(bulletPrefab);
        bulletGO.transform.position = firePoint.position;
        bulletGO.transform.rotation = firePoint.rotation;
        bulletGO.SetActive(true);

        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
            bullet.Seek(target);

    }



    protected void UpdateTarget()
    {

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (!enemies[i].activeSelf)
            enemies.RemoveAt(i);
        }

        for (int i = allies.Count - 1; i >= 0; i--)
        {
            if (!allies[i].activeSelf)
                allies.RemoveAt(i);
        }


        if (target != null)
        {
            if (!target.gameObject.activeSelf)
            {
                target = null;
                targetEnemy = null;
            }
        }

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

     

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }   
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<IDamageable>();
        }
        else
        {
            target = null;
        }

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<IDamageable>() == null)
            return;

        if (collider.GetComponent<IDamageable>().GetAlignement() == null)
        {
            Debug.Log("Target dont have an alignement");
            return;
        }

        if (collider.GetComponent<IDamageable>().GetAlignement() == alignement)
            allies.Add(collider.gameObject);
        else
            enemies.Add(collider.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemies.Contains(other.gameObject))
            enemies.Remove(other.gameObject);

        if (allies.Contains(other.gameObject))
            allies.Remove(other.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

  
}
