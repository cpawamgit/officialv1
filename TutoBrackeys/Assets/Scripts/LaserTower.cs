using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Turret {

    [Header("Use Laser")]

    public bool useLaser = false;

    public int damageOverTime = 30;
    public float slowAmount = 0.5f;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;


    private void OnEnable()
    {   
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }


    private void Update()
    {
        if (target == null)
        {
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                impactEffect.Stop();
                impactLight.enabled = false;
            }
            return;

        }

        base.LockOnTarget();

      
        Laser();
    

     }


    void Laser()
    {
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.ModifySpeed(slowAmount);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    private void OnTriggerExit(Collider other)
    {
        enemies.Remove(other.gameObject);
    }

}
