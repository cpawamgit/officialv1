using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour, IDamageable
{

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
    public GameObject speedEffect;
    public GameObject healEffect;

    private Dictionary<string, GameObject> effectDictionnary;
  

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
        return alignement;
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
