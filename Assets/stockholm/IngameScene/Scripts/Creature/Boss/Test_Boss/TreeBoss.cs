using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBoss : BuildingBoss {
    public GameObject player;
    public float maxRoomWidth = 25f;
    private TreeBossAnimator treeAnim_;
    
    new void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform.root.gameObject;
        treeAnim_ = (TreeBossAnimator)_anim;
    }

    new void Update()
    {
        if (!_anim.doingPattern())
        {
            patternTimer += Time.deltaTime;
        }

        if (patternTimer > patternInterval)
        {
            if (!_anim.doingPattern())
            {
                calculateAttackPos();
                _anim.animatePattern();
                patternTimer = 0;
            }
        }
    }
    void calculateAttackPos()
    {
        int pos = 0;
        float dist = player.transform.position.x - transform.position.x;
        float jointWidth = maxRoomWidth / 12;
        int charPos = (int)(dist / jointWidth);
        if (dist > 0)
            pos += 6;
        else if (dist < 0)
            pos += 5;
        pos += charPos;
        treeAnim_.setAttackPos(pos);
    }
}
