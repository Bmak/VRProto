using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class VR_AvatarController : MonoBehaviour
{
    public Transform Head;

    public int AvatarHeight = 100;
    // Start is called before the first frame update
    void Start()
    {
        Head = Player.instance.headCollider.transform;
    }

    void FixedUpdate()
    {
        float distanceFromFloor = Vector3.Dot( Head.localPosition, Vector3.up );
        transform.localPosition = Head.localPosition - 0.5f * distanceFromFloor * Vector3.up;
    }
}
