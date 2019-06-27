using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;

    private Transform _target;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Collider _collider;
    private Collider[] _ragdollColliders;
    private Rigidbody[] _ragdollRb;
    private Rigidbody _rb;
    
    private bool IsDead = false;

    private void Start()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _agent.Warp(transform.position);
        
        

        _target = PlayerManager.instance.player.transform;
        
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        _ragdollColliders = GetComponentsInChildren<Collider>();
        _ragdollRb = GetComponentsInChildren<Rigidbody>();

        DoRagdoll(false);
    }

    private void Update()
    {
        if (IsDead) return;
        
        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance <= lookRadius)
        {
            _agent.SetDestination(_target.position);
            FaceTarget();
            
            float step =  _agent.speed * Time.deltaTime;
            _animator.SetFloat("Speed", step);
            _animator.speed = _agent.speed;

            if (distance <= _agent.stoppingDistance)
            {
                Attack();
            }
            else
            {
                _animator.SetBool("Attack", false);
            }
        }
    }

    private void Attack()
    {
        _animator.SetFloat("Speed", 0);
        _animator.speed = 1f;
        _animator.SetBool("Attack", true);
    }

    private void FaceTarget()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    
    public void OnDamage()
    {
        //TODO Кривое отслеживание попадания по врагу
        
        /*
        _rb.isKinematic = true;
        _animator.speed = 1f;
        IsDead = true;
        _animator.SetFloat("Speed", 0);
        _animator.SetBool("IsDead", IsDead);
        _animator.Play("fallingback");
            
        Destroy( gameObject , 10f);
        return;
        */
        
        IsDead = true;
        //_animator.enabled = false;
        _agent.enabled = false;
        DoRagdoll(true);
        Destroy( gameObject , 10f);
    }
    
    public void DoRagdoll(bool isRagdoll)
    {
        foreach (var rb in _ragdollRb)
        {
            rb.isKinematic = !isRagdoll;
        }
        
        _animator.enabled = !isRagdoll;
    }
}
