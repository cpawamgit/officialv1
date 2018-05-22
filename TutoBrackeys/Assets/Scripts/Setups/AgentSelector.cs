using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AgentSelector : MonoBehaviour
{

    #region Singleton
    public static AgentSelector Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion


    [HideInInspector]
    public List<GameObject> selectedAgents;


    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SelectAgent(GameObject agentToSelect)
    {
        if (!selectedAgents.Contains(agentToSelect))
        {
            selectedAgents.Add(agentToSelect);
        }
        else
        {
            selectedAgents.Remove(agentToSelect);
        }
    }
}
