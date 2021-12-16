using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    [SerializeField]
    private List<Condition> conditionList_;

    // Use this for initialization
    void Start()
    {
        conditionList_ = new List<Condition>();
    }

    public void AddCondition(Condition c)
    {
        Condition temp = conditionList_.Find(con => con.conditionName.Equals(c.conditionName));
        if (temp != null)
        {
            temp.resetAllEffects();
            Destroy(c.gameObject);
            return;
        }
        c.transform.parent = transform;
        c.target = this;
        c.transform.localPosition = Vector3.zero;
        c.conditionDestroyed += DeleteCondition;
        conditionList_.Add(c);
        c.startAllEffects();
    }

    public void DeleteCondition(Condition c)
    {
        Condition temp = conditionList_.Find(con => con.conditionName.Equals(c.conditionName));
        if (temp == null)
        {
            return;
        }
        temp.stopAllEffects();
        conditionList_.Remove(temp);
        Destroy(temp.gameObject);
    }


}
