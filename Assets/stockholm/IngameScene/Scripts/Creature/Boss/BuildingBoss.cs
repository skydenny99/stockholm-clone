using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBoss : MonoBehaviour, IBoss {
    public float HP;
    public int currentPhase = 0;
    protected int previousPhase = 0;
    public List<int> phaseStartHpList;

    public float patternInterval = 1f;
    protected float patternTimer = 0f;
    
    public BossAnimator _anim;

	// Use this for initialization
	protected void Start () {
        phaseStartHpList.Sort();
        previousPhase = currentPhase;
        Debug.Assert(_anim);
	}
	
	// Update is called once per frame
	protected void Update () {
        if (!_anim.doingPattern())
        {
            patternTimer += Time.deltaTime;
        }

        if (patternTimer > patternInterval)
        {
            if(!_anim.doingPattern())
            {
                _anim.animatePattern();
                patternTimer = 0;
            }
        }
    }

    public void getHit(float dmg)
    {
        HP -= dmg;
        _anim.getHit();
        changePhase();
    }

    void changePhase()
    {
        previousPhase = currentPhase;
        for(int i = 0; i < phaseStartHpList.Count; i++)
        {
            if(HP > phaseStartHpList[i])
            {
                currentPhase = i;
                break;
            }
            else
            {
                currentPhase = i + 1;
            }
        }
        if (currentPhase != previousPhase)
            _anim.changePhase();
    }
}
