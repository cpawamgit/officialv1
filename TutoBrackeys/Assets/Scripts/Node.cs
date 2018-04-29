using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.AI;

public class Node : MonoBehaviour {

    public Color hoverColor;
    public Vector3 offset;
    public Color notEnoughMoneyColor;
    public Transform spawnPointForPeones;
    public GameObject peones;

    public Transform positionTestForPeon;

    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded = false;

    private Renderer rend;
    private Color startColor;

    BuildManager buildManager;

    void Start ()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.Instance;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + offset;
    }


    void BuildTurret(TurretBlueprint blueprint)
    {
        if (PlayerStats.Instance.Money < blueprint.cost)
        {
            Debug.Log("Not enough money !!!!");
            return;
        }

        PlayerStats.Instance.ChangeMoney(-blueprint.cost);

        GameObject _peones = MyObjectPooler.Instance.SpawnFromPoolAt(peones, spawnPointForPeones.position, spawnPointForPeones.rotation);
        _peones.GetComponent<Peons>().GoBuildATower(blueprint, GetBuildPosition(), this);

        turretBlueprint = blueprint;
    }

    public void UpgradeTurret()
    {
        if (PlayerStats.Instance.Money < turretBlueprint.upgradeCost)
        {
            Debug.Log("Not enough money to upgrade !!!");
            return;
        }

        PlayerStats.Instance.ChangeMoney(-turretBlueprint.upgradeCost);

        //Get rid of the old turret
        MyObjectPooler.Instance.ReturnToPool(turret);

        //Build a new one
        GameObject _turret = MyObjectPooler.Instance.SpawnFromPool(turretBlueprint.upgradedPrefab);
        _turret.transform.position = GetBuildPosition();
        _turret.transform.rotation = Quaternion.identity;
        _turret.SetActive(true);


        turret = _turret;

        GameObject effect = MyObjectPooler.Instance.SpawnFromPool(buildManager.buildEffect);
        effect.transform.position = GetBuildPosition();
        effect.transform.rotation = Quaternion.identity;
        effect.SetActive(true);


        isUpgraded = true;

        Debug.Log("Turret upgraded !");
    }

    public void SellTurret()
    {
        PlayerStats.Instance.ChangeMoney(turretBlueprint.GetSellAmount());

        GameObject _selleffect = MyObjectPooler.Instance.SpawnFromPool(buildManager.sellEffect);
        _selleffect.transform.position = GetBuildPosition();
        _selleffect.transform.rotation = Quaternion.identity;
        _selleffect.SetActive(true);

        MyObjectPooler.Instance.ReturnToPool(turret);
        turret = null;
        turretBlueprint = null;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (!buildManager.CanBuild)
            return;


        BuildTurret(buildManager.GetTurretToBuild());
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildManager.CanBuild)
            return;

        if (buildManager.HasMoney)
            rend.material.color = hoverColor;
        else
            rend.material.color = notEnoughMoneyColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    
       
}
