using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{


    public Dictionary<string, NetworkedPool> poolDictionnary = new Dictionary<string, NetworkedPool>();


    #region Singleton
    public static PoolManager Instance;

    protected void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDisable()
    {
        Instance = null;
    }

    #endregion

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            NetworkedPool pool = transform.GetChild(i).GetComponent<NetworkedPool>();
            poolDictionnary.Add(pool.poolname, pool);
        }
    }


}
