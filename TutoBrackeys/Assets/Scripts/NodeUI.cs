using UnityEngine.UI;
using UnityEngine;

public class NodeUI : MonoBehaviour
{
    public GameObject UI;
    public Text upgradeCost;
    public Text upgradeText;
    public Button upgradeButton;
    public Text sellAmount;

    private Node target;

    public void SetTarget(Node _target)
    {
        target = _target;
        transform.position = target.GetBuildPosition();


        if (!target.isUpgraded)
        {

            upgradeCost.text = target.turretBlueprint.upgradeCost + "€";


            upgradeButton.interactable = true;
            upgradeText.enabled = true;
        }
        else
        {
            upgradeCost.text = "LVL MAX";
            upgradeButton.interactable = false;
            upgradeText.enabled = false;
        }


        sellAmount.text = target.turretBlueprint.GetSellAmount() + "€";

        UI.SetActive(true);
    }
	
    public void Hide()
    {
        UI.SetActive(false);
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.Instance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.Instance.DeselectNode();
    }
}
