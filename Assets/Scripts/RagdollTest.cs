using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollTest : MonoBehaviour
{
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.blue);
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray2.origin, ray2.direction, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            
            RaycastHit[] hits = Physics.RaycastAll( ray, 1000f);
            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                Debug.LogWarning(objectHit.gameObject.tag);
                if(objectHit.gameObject.tag == "Enemy")
                {
                    _animator.enabled = false;
                }
                // Do something with the object that was hit by the raycast.
            }
        }
    }
}
