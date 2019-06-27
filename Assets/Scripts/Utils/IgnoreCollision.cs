using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    public Collider IgnoreCollider;
    
    void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), IgnoreCollider);
    }

}
