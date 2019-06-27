using UnityEngine;

public class TestRagdoll : MonoBehaviour
{
    private Collider _mainCollider;
    private Collider[] _ragdollColliders;
    private Animator _animator;
    private Rigidbody _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        _mainCollider = GetComponent<Collider>();
        _ragdollColliders = GetComponentsInChildren<Collider>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        DoRagdoll(false);
    }

    public void DoRagdoll(bool isRagdoll)
    {
        foreach (Collider col in _ragdollColliders)
        {
            col.enabled = isRagdoll;
        }
Debug.Log("TEST");
        //_mainCollider.enabled = !isRagdoll;
        //_rb.useGravity = !isRagdoll;
        _animator.enabled = !isRagdoll;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DoRagdoll(true);
        }
    }
}
