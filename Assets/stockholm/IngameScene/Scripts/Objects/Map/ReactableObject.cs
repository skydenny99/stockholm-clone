using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReactableObject : MapObject, IReactable
{

    public string reactableTileName = "tEMMIE";
    public string reactableTileTooltip = "hOI";
    public bool isReadyToReact = true;
    public bool onlyAffectPlayer = false;
    public Collider2D[] cols;

    public bool selfCheckStateToReact = false;
    public float checkInterval = 0.1f;
    public Transform sensor;
    protected Vector2 sensorCenter;
    protected float sensorWidth;
    protected float sensorHeight;

    protected WaitForSeconds delay_;
    protected Coroutine checkConditionCoroutine_;
    protected Creature target_;


    public IEnumerator CheckConditionToReact()
    {
        while (true)
        {
            Vector2 temp = new Vector2(sensorWidth / 2, -sensorHeight / 2);
            cols = Physics2D.OverlapAreaAll(sensorCenter - temp, sensorCenter + temp);
            foreach (Collider2D col in cols)
            {
                if (isReadyToReact)
                {

                    target_ = null;
                    if (onlyAffectPlayer)
                    {
                        if (col.CompareTag("Player"))
                        {
                            target_ = col.GetComponent<Creature>();
                        }
                    }
                    else
                        target_ = col.GetComponent<Creature>();


                    if (target_ != null)
                        React(col);

                    if (col.CompareTag("Player")&&onlyAffectPlayer)
                    {
                        break;
                    }
                }
            }

            yield return delay_;
        }
    }


    public abstract void React(Collider2D col);

    protected void Start()
    {
        if (checkInterval < 0.03f)
            delay_ = null;
        else
            delay_ = new WaitForSeconds(checkInterval);

        if (selfCheckStateToReact)
            sensor = transform;

        sensorCenter = sensor.position;
        Vector2 box = sensor.GetComponent<BoxCollider2D>().size;
        sensorWidth = box.x;
        sensorHeight = box.y;


        checkConditionCoroutine_ = StartCoroutine(CheckConditionToReact());
    }

}
