using UnityEngine;
using Valve.VR.InteractionSystem.Sample;

[RequireComponent(typeof( Collider ))]
public class ReloadZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            var weapon = other.GetComponent<PistolShootController>();
            weapon.ReloadWeapon();
        }
    }

}
