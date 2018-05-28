using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AgentSelector : MonoBehaviour
{

    #region Singleton
    public static AgentSelector Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion


    [System.Serializable]
    public class ButtonClass
    {
        public Image imageButton;
        //[HideInInspector]
        public bool clicked = false;
    }


    [HideInInspector]
    public List<GameObject> selectedAgents = new List<GameObject>();

    public List<ButtonClass> agentSelectButtons = new List<ButtonClass>();

    public List<Pool> selectedAgentPool = new List<Pool>();


    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    private void Update()
    {
        Debug.Log("selectedAgents.Count = " + selectedAgents.Count);
    }

    public void SelectAgent(GameObject agentToSelect)
    {
        
        if (selectedAgents.Contains(agentToSelect))
        {
            selectedAgents.Remove(agentToSelect);
        }
        else if (selectedAgents.Count < 3)
        {
            selectedAgents.Add(agentToSelect);
        }
      
    }

    public void ChangeButtonState(int button)
    {
        if (agentSelectButtons[button].clicked)
        {
            agentSelectButtons[button].imageButton.color = Color.white;
            agentSelectButtons[button].clicked = false;
        }
        else if(selectedAgents.Count < 3)
        {
            agentSelectButtons[button].imageButton.color = Color.red;
            agentSelectButtons[button].clicked = true;
        }

    }

    public void ConvertGoToPool()
    {
        foreach (GameObject Go in selectedAgents)
        {
            Pool poolObject = new Pool();
            poolObject.tag = Go.name;
            poolObject.prefab = Go;
            poolObject.size = 20;
            poolObject.willGrow = true;

            selectedAgentPool.Add(poolObject);
        }
        Debug.Log("After ConvertGoToPool selectedAgentPool count : " + selectedAgentPool.Count);
    }
}
