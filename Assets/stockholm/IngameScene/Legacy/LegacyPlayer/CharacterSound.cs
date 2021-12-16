using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSound : MonoBehaviour {
    public enum FloorType
    {
        grass
    }

    public FloorType currentFloor = FloorType.grass;
    public AudioSource source;
    public GameObject cAnimator;
    private CharacterAnimator _anim;

    public AudioClip[] grasses;
    
    public float walkingVol = 3f;
    public float footstepInterval = 0.1f;
    private float walkTimer = 0f;
    

	// Use this for initialization
	void Start () {
        walkTimer += footstepInterval;
        source = gameObject.GetComponent<AudioSource>();
        _anim = cAnimator.GetComponent<CharacterAnimator>();
        Debug.Assert(source);
        Debug.Assert(_anim);
	}
	
	// Update is called once per frame
	void Update () {
        walkTimer += Time.deltaTime;
        if (_anim.isAnimating(CharacterAnimator.CurrentPlayerAnimate.Walking) && walkTimer > footstepInterval)
        {
            WalkingSound();
            walkTimer = 0f;
        }
        
	}

    void WalkingSound()
    {
        AudioClip clip = null;
        switch(currentFloor)
        {
            case FloorType.grass:
                clip = grasses[Random.Range(0, grasses.Length - 1)];
                break;
        }
        source.PlayOneShot(clip);

    }

    public void setFlooerType(FloorType type)
    {
        currentFloor = type;
    }
}
