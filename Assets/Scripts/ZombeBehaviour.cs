using System.Diagnostics.SymbolStore;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ZombeBehaviour : MonoBehaviour
{
    private Animator _animator;
    private float _moveSpeed = 1f;
    private float _rotateSpeed = 10;
    private Transform _target;
    private Collider _collider;
    private Rigidbody _rb;

    private bool IsDead = false;

    public void SetSpeed(float speed)
    {
        _moveSpeed = speed;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        _target = Player.instance.headCollider.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead) return;
        
        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, _target.position) < 1f)
        {
            _animator.SetFloat("Speed", 0);
            _animator.speed = 1f;
            _animator.SetBool("Attack", true);
            return;
        }
        else
        {
            _animator.SetBool("Attack", false);
        }
        
        // Move our position a step closer to the target.
        float step =  _moveSpeed * Time.deltaTime; // calculate distance to move
        Vector3 moveTo = Vector3.MoveTowards(transform.position, _target.position, step);
        transform.position = new Vector3(moveTo.x, 0, moveTo.z);
        
        Vector3 targetDir = _target.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, _rotateSpeed * Time.deltaTime, 0.0f);
        //Debug.DrawRay(transform.position, newDir, Color.red);

        // Move our position a step closer to the target.
        var newRotation = Quaternion.LookRotation(newDir);
        newRotation.x = 0;
        newRotation.z = 0;
        transform.rotation = newRotation;
        _animator.SetFloat("Speed", step);
        _animator.speed = _moveSpeed;
    }
    
    void OnCollisionEnter( Collision collision )
    {
        Bullet bullet = collision.collider.gameObject.GetComponent<Bullet>();
        bool hitByBullet = bullet != null;

        if (hitByBullet)
        {
            _rb.isKinematic = true;
            Physics.IgnoreCollision( _collider, collision.collider );
            _moveSpeed = 0;
            _animator.speed = 1f;
            IsDead = true;
            _animator.SetFloat("Speed", 0);
            _animator.SetBool("IsDead", IsDead);
            _animator.Play("fallingback");
            
            Destroy( gameObject , 10f);
            Destroy( bullet );
        }
    }

    public void OnDamage()
    {
        IsDead = true;
        _animator.enabled = false;
        _moveSpeed = 0;
        Destroy( gameObject , 3f);
        return;
        _rb.isKinematic = true;
        _moveSpeed = 0;
        _animator.speed = 1f;
        IsDead = true;
        _animator.SetFloat("Speed", 0);
        _animator.SetBool("IsDead", IsDead);
        _animator.Play("fallingback");
            
        Destroy( gameObject , 10f);
    }
}
