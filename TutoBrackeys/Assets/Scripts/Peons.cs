using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Peons : MonoBehaviour, IDamageable
{
    private NavMeshAgent navMeshAgent;
    private Vector3 buildPosition;
    private bool building = false;
    private bool onTheWay = false;
    private float constructionSpeed;
    private int peonsAlive;
    public float lifePerPeon = 25;
    private float peonActualLife;
    private float constructedPoints;
    private TurretBlueprint blueprint;
    private Node nodeDestination;
    private GameObject _buildingTower;
    private int peonToDisable = 0;
    private float speed;

    public List<GameObject> peons;
    public GameObject buildingTower;
    public Alignement alignement;
    public float startSpeed;

    public GameObject speedEffect;
    public GameObject healEffect;


    private Dictionary<string, GameObject> effectDictionnary;


    private void OnEnable()
    {
        speed = startSpeed;
        navMeshAgent = GetComponent<NavMeshAgent>();
        building = false;
        onTheWay = false;
        constructionSpeed = 0.25f;
        peonsAlive = 4;
        constructedPoints = 0f;
        peonActualLife = lifePerPeon;

        navMeshAgent.enabled = false;
        navMeshAgent.enabled = true;
        navMeshAgent.speed = startSpeed;

        effectDictionnary = new Dictionary<string, GameObject>();
        effectDictionnary.Add("speedEffect", speedEffect);
        effectDictionnary.Add("healEffect", healEffect);


        for (int i = 0; i < peons.Count; i++)
        {
            if (!peons[i].activeSelf)
                peons[i].SetActive(true);
        }
    }

    private void Update()
    {
        if (onTheWay)
        {
            if (navMeshAgent.remainingDistance <= Mathf.Epsilon)
            {
                transform.LookAt(buildPosition);
                navMeshAgent.isStopped = true;
                BuildATower();
                onTheWay = false;
            }
        }

        if (building)
        {
            if (constructedPoints >= blueprint.constructionPoints)
            {
                MyObjectPooler.Instance.ReturnToPool(_buildingTower);
                GameObject turretBuild = MyObjectPooler.Instance.SpawnFromPoolAt(blueprint.prefab, buildPosition, Quaternion.identity);
                MyObjectPooler.Instance.SpawnFromPoolAt(BuildManager.Instance.buildEffect, buildPosition, Quaternion.identity);

                Debug.Log("Tower built");

                building = false;
                MyObjectPooler.Instance.ReturnToPool(gameObject);

                nodeDestination.turret = turretBuild;
            }
            else
            {
                constructedPoints += constructionSpeed * peonsAlive * Time.deltaTime;
            }

        }
    }

    public void GoBuildATower(TurretBlueprint _blueprint, Vector3 _buildPosition, Node _nodeDestination)
    {
        buildPosition = _buildPosition;
        blueprint = _blueprint;
        nodeDestination = _nodeDestination;
        onTheWay = true;

        navMeshAgent.destination = buildPosition;
    }


    private void BuildATower()
    {
        _buildingTower = MyObjectPooler.Instance.SpawnFromPoolAt(buildingTower, buildPosition, Quaternion.identity);
        building = true;
    }

    public void TakeDamage(float amount)
    {
        peonActualLife -= amount;

        if (peonActualLife <= 0)
        {
            peonsAlive -= 1;
            peonActualLife = lifePerPeon;
            peons[peonToDisable].SetActive(false);
            peonToDisable++;

            if (peonsAlive <=0)
                MyObjectPooler.Instance.ReturnToPool(gameObject);
        }
    }

    public Alignement GetAlignement()
    {
        return alignement;
    }

    public void ModifySpeed(float multiplier)
    {
        speed = startSpeed * multiplier;
        navMeshAgent.speed = speed;
    }

    public void Heal(float amount)
    {
        if (peonActualLife + amount <= lifePerPeon)
            peonActualLife += amount;
        else
            peonActualLife = lifePerPeon;

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
