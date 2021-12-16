using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Effect : MonoBehaviour {

    //TODO : ICreature를 MoveAbleCreature로 변경
    public ICreature target { get; set; }
    public string effectName = "Name";

    public int conditionLevel = 1;
    public bool isBuff = true;
    public bool isInfinity = false;
    public float basicDurationTime = 0f;
    public float durationTimeByLevelRate = 1f;
    private float finalDurationTime = 0f;

    public bool isTicking = true;
    public float tickInterval = 1f;
    private int tickCount = 0;
    private int currentCount = 0;

    public delegate void EffectDestroyed(Effect destroyed);

    public event EffectDestroyed destroyed;

    protected Coroutine c;
    protected bool coroutineStarted = false;
    
    public void startEffect()
    {
        if (isInfinity)
        {
            applyEffect();
        }
        else
        {
            if (isTicking)
            {
                tickCount = (int)(finalDurationTime / tickInterval);
            }
            else
            {
                tickCount = 1;
                tickInterval = finalDurationTime;
            }
            if (gameObject.active)
                c = StartCoroutine(effectCoroutine());
            else
                coroutineStarted = true;
        }
    }

    public IEnumerator effectCoroutine()
    {
        for (currentCount = 0; currentCount < tickCount; currentCount++)
        {
            applyEffect();
            yield return new WaitForSeconds(tickInterval);
        }
        if (!isTicking)
            revokeEffect();
        
        if(destroyed != null)
            destroyed.Invoke(this); 
    }
    public void stopEffect()
    {
        if(!isInfinity)
        {
            if(c != null)
                StopCoroutine(c);
        }
        else
        {
            revokeEffect();
        }
        //문제
        if(destroyed != null)
            destroyed.Invoke(this);
    }

    public abstract void applyEffect();

    public abstract void revokeEffect();

    protected virtual void Awake()
    {
        finalDurationTime = basicDurationTime + conditionLevel * durationTimeByLevelRate;
    }

	protected virtual void Start()
    {
        if (coroutineStarted)
            if (c == null)
                c = StartCoroutine(effectCoroutine());
    }

    public void initializeCount()
    {
        currentCount = 0;
    }
}
