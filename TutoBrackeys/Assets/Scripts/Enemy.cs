using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float startSpeed = 10f;
    public float startHealth = 100f;
    private float health;

    [HideInInspector]
    public float normalizedHealth;

    private EnemyHealthBar enemyHealthBar;

    public int value = 50;
    public GameObject deathEffect;

    [HideInInspector]
    public float speed;

    public float CDBetweenSpawns;
    public int price;

    private bool isDead = false;    

    public Alignement alignement;

    public Sprite image;

    //public Animator animator;

    //private void Awake()
    //{
    //    if (GetComponent<Animator>())
    //    {
    //        animator = GetComponent<Animator>();
    //    }
    //}


    private void OnEnable()
    {
        speed = startSpeed;
        health = startHealth;
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        isDead = false;
    }

    //private void Start()
    //{
    //    speed = startSpeed;
    //    health = startHealth;
    //    enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();


    //    //animator.SetBool("isWalking", true);

    //    //animator.Play("Armature|ArmatureAction.001");
    //}



    public void TakeDamage(float amount)
    {
        health -= amount;
        normalizedHealth = health / startHealth;
        enemyHealthBar.UpdateEnnemyHealth(normalizedHealth);

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Slow (float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    void Die()
    {
        isDead = true;

        GameObject deathEffectInst = MyObjectPooler.Instance.SpawnFromPool(deathEffect);
        deathEffectInst.transform.position = transform.position;
        deathEffectInst.transform.rotation = transform.rotation;
        deathEffectInst.SetActive(true);

        PlayerStats.Money += value;

        WaveSpawner.EnemiesAlive--;

        MyObjectPooler.Instance.ReturnToPool(gameObject);
    }

}
