using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class Unite : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    public int value = 50;
    public float CDBetweenSpawns;
    public int price;


    public float startSpeed = 10f;
    public float startHealth = 100f;
    [HideInInspector]
    public float normalizedHealth;

    [Header("Effects")]
    public Sprite image;
    public GameObject speedEffect;
    public GameObject healEffect;
    public GameObject deathEffect;

    [Header("Setup")]
    public Targetter targetter;


    [HideInInspector]
    public float speed;


    private bool isDead = false;
    private float fireCountdown = 0f;
    private float health;
    protected float resistance = 0f;

    private EnemyHealthBar enemyHealthBar;


    private Dictionary<string, GameObject> effectDictionnary;



    protected virtual void Awake()
    {

    }
   

    private void OnEnable()
    {
        speed = startSpeed;
        health = startHealth;
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        isDead = false;
        normalizedHealth = health / startHealth;
        enemyHealthBar.UpdateEnnemyHealth(normalizedHealth);

        effectDictionnary = new Dictionary<string, GameObject>();
        effectDictionnary.Add("speedEffect", speedEffect);
        effectDictionnary.Add("healEffect", healEffect);
    }


    protected virtual void Update()
    {
        if (targetter.target == null)
        {
            return;
        }


        targetter.LockOnTarget();

        if (fireCountdown <= 0f)
        {
            Attack();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }




    protected  virtual void Attack()
    {
        GameObject bulletGO = MyObjectPooler.Instance.SpawnFromPool(bulletPrefab);
        bulletGO.transform.position = targetter.firePoint.position;
        bulletGO.transform.rotation = targetter.firePoint.rotation;
        bulletGO.SetActive(true);

        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
            bullet.Seek(targetter.target);

    }




    public virtual void TakeDamage(float amount)
    {
        float effectivDamage = amount * (1 - resistance);
        health -= effectivDamage;
        normalizedHealth = health / startHealth;
        enemyHealthBar.UpdateEnnemyHealth(normalizedHealth);

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void ModifySpeed (float multiplier)
    {
        speed = startSpeed * multiplier;
    }

    void Die()
    {
        isDead = true;

        GameObject deathEffectInst = MyObjectPooler.Instance.SpawnFromPool(deathEffect);
        deathEffectInst.transform.position = transform.position;
        deathEffectInst.transform.rotation = transform.rotation;
        deathEffectInst.SetActive(true);

        PlayerStats.Instance.ChangeMoney(value);

        WaveSpawner.EnemiesAlive--;

        MyObjectPooler.Instance.ReturnToPool(gameObject);
    }

    public Alignement GetAlignement()
    {
        return targetter.alignement;
    }


    public void Heal(float amount)
    {
        if (health + amount <= startHealth)
            health += amount;
        else
            health = startHealth;

        normalizedHealth = health / startHealth;
        enemyHealthBar.UpdateEnnemyHealth(normalizedHealth);
    }

    public void TurnOnOffEffects(string effect, bool stateToTurn)
    {
        if (effectDictionnary[effect] == null)
        {
            Debug.Log("No effect with name " + effect);
            return;
        }

        if (stateToTurn)
            effectDictionnary[effect].SetActive(true);
        else
            effectDictionnary[effect].SetActive(false);

    }

}
