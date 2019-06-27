using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject player;

    void Awake()
    {
        instance = this;
    }
}
