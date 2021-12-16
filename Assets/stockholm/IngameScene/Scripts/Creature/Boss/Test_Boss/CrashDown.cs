using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrashDown : MonoBehaviour, BossPattern {
    public string patternName = "Crash Down";

    public GameObject leftArm;
    public GameObject rightArm;
    public float armWidth = 2;
    public float damage = 30;
    public float knockBack = 5;
    private GameObject currentPatternArm_;
    private GameObject player_;

    void Start()
    {
        player_ = GameObject.FindGameObjectWithTag("Player").transform.root.gameObject;
    }

    public string getPatternName()
    {
        return patternName;
    }

    public void run()
    {
        if (leftArm.transform.position.y > rightArm.transform.position.y)
            currentPatternArm_ = leftArm;
        else
            currentPatternArm_ = rightArm;

        if (Mathf.Abs(player_.transform.position.x - currentPatternArm_.transform.position.x) < armWidth)
        {
            player_.GetComponentInChildren<ILegacyCreature>().getDamaged(currentPatternArm_.transform, damage, knockBack);
        }
    }
    
    
}
