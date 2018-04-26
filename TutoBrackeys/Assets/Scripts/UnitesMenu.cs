using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitesMenu : MonoBehaviour
{
    public Transform spawnPoint;

    public List<Image> agentButtonsImageList;
    public List<Button> agentButtonsList;
    public List<Text> agentButtonsTextsList;

    private List<int> priceList = new List<int>(5);
    private List<float> CDList = new List<float>(5);



    private void Awake()
    {

        for (int i = 0; i < AgentSelector.Instance.selectedAgents.Count; i++)
        {
            agentButtonsImageList[i].sprite = AgentSelector.Instance.selectedAgents[i].GetComponent<Enemy>().image;
            CDList.Add(AgentSelector.Instance.selectedAgents[i].GetComponent<Enemy>().CDBetweenSpawns);
            priceList.Add(AgentSelector.Instance.selectedAgents[i].GetComponent<Enemy>().price);
        }

    }


    private void Update()
    {
        int i = 0;

        foreach (Button button in agentButtonsList)
        {
            if (!button.interactable)
            {
                CDList[i] -= Time.deltaTime;
                agentButtonsTextsList[i].text = string.Format("{0:00.00}", CDList[i]);

                if (CDList[i] <= 0)
                {
                    CDList[i] = AgentSelector.Instance.selectedAgents[i].GetComponent<Enemy>().CDBetweenSpawns;
                    agentButtonsTextsList[i].text = null;
                    //if (priceList[i] <= PlayerStats.Money)
                    //{
                    //    button.interactable = true;
                    //}
                }
            }
            i++;
        }
    }


    public void SpawnUnite(int indexUniteToSpawn)
    {
        MyObjectPooler.Instance.SpawnFromPoolAt(AgentSelector.Instance.selectedAgents[indexUniteToSpawn], spawnPoint.transform.position, spawnPoint.transform.rotation);
        agentButtonsList[indexUniteToSpawn].interactable = false;

        PlayerStats.Money -= priceList[indexUniteToSpawn];
    }
	
}
