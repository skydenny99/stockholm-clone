using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossPhase : MonoBehaviour {

    public int phaseNum = 0;
    public List<Component> patternComponentList;
    public List<BossPattern> phasePatternList = new List<BossPattern>();

    void Start()
    {
        phasePatternList = patternComponentList.Cast<BossPattern>().ToList();
        Debug.Log(phaseNum + "phase count : " + phasePatternList.Count);
    }

    public BossPattern getRandomPattern()
    {
        return phasePatternList[Random.Range(0, phasePatternList.Count)];
    }
}
