using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReactable {
    
    IEnumerator CheckConditionToReact();
    void React(Collider2D col);

}
