using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossAnimator : MonoBehaviour {

    public List<BossPhase> phaseList;
    public BossPattern currentPattern;
    public GameObject patternObject;
    public BuildingBoss _boss;
    protected Animator _anim;

    protected void Start()
    {
        _anim = transform.GetComponent<Animator>();
        phaseList = patternObject.GetComponents<BossPhase>().OrderBy(bp => bp.phaseNum).ToList();
        Debug.Assert(_anim);
        Debug.Assert(_boss);
    }
    public void getHit()
    {
        _anim.SetTrigger("Get Hit");
    }

    public void animatePattern()
    {
        currentPattern = phaseList[_boss.currentPhase].getRandomPattern();
        _anim.SetTrigger(currentPattern.getPatternName());
    }

    public void runPattern()
    {
        currentPattern.run();
    }

    public bool doingPattern()
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || _anim.GetCurrentAnimatorStateInfo(0).IsName("Get Hit"))
        {
            return false;
        }
        return true;
    }

    public void resetAllTrigger()
    {
        AnimatorControllerParameter[] param = _anim.parameters;
        for (int i = 0; i < param.Length; i++)
            if (param[i].type == AnimatorControllerParameterType.Trigger)
                _anim.ResetTrigger(Animator.StringToHash(param[i].name));
    }

    public void resetGetHitTrigger()
    {
        _anim.ResetTrigger("Get Hit");
    }

    public void changePhase()
    {
        _anim.SetTrigger("Change Phase");
    }

}
