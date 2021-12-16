using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour {

    public Collider2D[] cols;
    private Transform _transform;
    private Character _control;

    private float xSize;
    private float ySize;

	// Use this for initialization
	void Start () {
        _transform = transform;
        xSize = _transform.GetComponent<BoxCollider2D>().size.x;
        ySize = _transform.GetComponent<BoxCollider2D>().size.y;
        _control = _transform.parent.GetComponent<Character>();
        Debug.Assert(_control);
	}
	
	// Update is called once per frame
	void Update () {
        cols = Physics2D.OverlapAreaAll(_transform.position - new Vector3(xSize / 2, 0), _transform.position + new Vector3(xSize / 2, ySize/2));
        foreach (Collider2D hit in cols)
        {
            if (hit.tag.Equals("Ground"))
            {
                _control.setIsGrounded(true);
                _control.setCanJump(true);
                return;
            }
        }
        _control.setCanJump(false);
        _control.setIsGrounded(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.root.tag.Equals("Ground"))
        {
            _transform.root.GetComponentInChildren<CharacterAnimator>().resetAllTrigger();
        }
    }
    

    


}
