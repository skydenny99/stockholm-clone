using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Condition : MonoBehaviour {

    public ConditionManager target { get; set; }
    public string conditionName = "Name";
    public string tooltip = "bulabula";
    public Sprite sprite;

    public List<Effect> effects;
    private List<Effect> realEffects;


    public delegate void ConditionDestroyed(Condition destroyed);

    public event ConditionDestroyed conditionDestroyed;

    void Awake()
    {
        Debug.Assert(effects.Count != 0);
        realEffects = new List<Effect>();
    }

    public void startAllEffects()
    {
        effects.ForEach(effect =>
        {
            Effect e = Instantiate(effect);
            e.transform.parent = transform;
            e.transform.localPosition = Vector2.zero;
            e.target = this.target.transform.root.GetComponentInChildren<ICreature>();
            e.destroyed += OnEffectDestroyed;
            e.startEffect();
        });
        /*foreach(Effect e in effects)
        {
            Debug.Log("how many");
            e.target = this.target.transform.root.GetComponentInChildren<ICreature>();
            e.destroyed += OnConditionDestroyed;
            e.startEffect();
        }*/
    }

    public void startSpecificEffect(Effect effect)
    {
        Effect temp = effects.Find(con => con.effectName.Equals(effect.effectName));
        Effect e = Instantiate(temp);
        e.transform.parent = transform;
        e.transform.localPosition = Vector2.zero;
        e.target = this.target.transform.root.GetComponentInChildren<ICreature>();
        e.destroyed += OnEffectDestroyed;
        e.startEffect();
    }
    
    public void stopAllEffects()
    {
        realEffects.ForEach(effect => effect.stopEffect());
        /*foreach(Effect e in effects)
        {
            e.stopEffect();
        }*/
    }

    public bool stopSpecificEffect(Effect e)
    {
        Effect temp = realEffects.Find(con => con.effectName.Equals(e.effectName));
        if (temp == null)
        {
            return false;
        }
        temp.stopEffect();
        return true;
    }

    public void resetAllEffects()
    {
        realEffects.ForEach(effect => effect.initializeCount());
    }

    private void OnEffectDestroyed(Effect destroyedEffect)
    {
        destroyedEffect.destroyed -= OnEffectDestroyed;
        DestroyEffect(destroyedEffect);
    }

    public void DestroyEffect(Effect c)
    {
        realEffects.Remove(c);
        Destroy(c.gameObject);
        if (realEffects.Count == 0)
            conditionDestroyed.Invoke(this);
    }

    void OnDestroy()
    {
        stopAllEffects();
    }
}
