using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionModule : MonoBehaviour,IModule {
    public List<Condition> conditionList;
    private Dictionary<string, Condition> conditionDictionary;

    private bool isRunning = false;

    public void DestroyModule()
    {
        Destroy(this);
    }

    public bool InitModule()
    {
        conditionDictionary = new Dictionary<string, Condition>();
        foreach(Condition c in  conditionList)
        {
            conditionDictionary.Add(c.conditionName, c);
        }
        return true;
    }

    public bool RunModule()
    {
        isRunning = true;
        return true;
    }

    public void StopModule()
    {
        isRunning = false;
    }

    public Condition getCondtionByName(string s)
    {
        Condition c = null;
        if(conditionDictionary.TryGetValue(s, out c))
        {
            if (c != null)
                return c;
        }

        return null;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
