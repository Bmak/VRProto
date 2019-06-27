using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Bullet : MonoBehaviour
{
    public PhysicMaterial targetPhysMaterial;
    public GameObject bulletView;
    
    public SoundPlayOneshot airReleaseSound;
    public SoundPlayOneshot hitTargetSound;
    public PlaySound hitGroundSound;
    
    
    private Collider arrowHeadRB;
    private bool inFlight;

    private float _speed;

    //-------------------------------------------------
    void Awake()
    {
	    //BULLET_SPEED = Random.Range(2f, 50f);
        arrowHeadRB = gameObject.GetComponent<Collider>();
        //Physics.IgnoreCollision( shaftRB.GetComponent<Collider>(), Player.instance.headCollider );
    }

    public void SetSpeed(float speed)
    {
	    _speed = speed;
    }

    private void Update()
    {
	    if (inFlight)
	    {
		    transform.Translate(Time.deltaTime * _speed, 0, 0);
	    }
    }

	
    private List<RaycastHit> _enemyHits = new List<RaycastHit>();

    private Transform _weapon;
    //-------------------------------------------------
    public void BulletReleased(Transform weapon)
    {
	    _weapon = weapon;
	    inFlight = true;
	    
        airReleaseSound.Play();
        
        _enemyHits.Clear();
        RaycastHit[] hits = Physics.RaycastAll( transform.position,  transform.right, 500f);
        foreach (RaycastHit hit in hits)
        {
	        Debug.DrawRay(transform.position, transform.right, Color.red, 100f);
	        //Debug.LogWarning("HIT " + hit.collider.gameObject.tag);
	        //Debug.LogWarning("HIT " + hit.collider.gameObject.name);
	        //Debug.LogWarning(hit.transform.name);
	        bool hitBalloon = hit.collider.gameObject.GetComponent<Balloon>() != null;
	        bool hitTarget = hit.collider.gameObject.GetComponent<ExplosionWobble>() != null;

	        /*
	        if ( hitTarget || hitBalloon )
	        {
		        hit.collider.gameObject.SendMessageUpwards( "ApplyDamage", SendMessageOptions.DontRequireReceiver );
		        gameObject.SendMessage( "HasAppliedDamage", SendMessageOptions.DontRequireReceiver );
		        hitTargetSound.Play();
		        bulletView.SetActive(false);
	        }

	        if ( hitBalloon )
	        {
		        Physics.IgnoreCollision( arrowHeadRB, hit.collider );
	        }
			*/
	        
	        //bool hitEnemy = hit.collider.gameObject.GetComponent<ZombeBehaviour>() != null;
	        bool hitEnemy = hit.collider.gameObject.tag == "Enemy";
	        if (hitEnemy)
	        {
		        _enemyHits.Add(hit);
	        }
			
	        Destroy( gameObject , 5f);
        }

        RaycastHit? validateHit = null;
        var distance = 1000f;
        foreach (RaycastHit hit in _enemyHits)
        {
	        validateHit = hit;
	        break;
	        float dist = Vector3.Distance(hit.transform.position, transform.position);
	        if (dist < distance)
	        {
		        distance = dist;
		        validateHit = hit;
	        }
	        
        }

        if (validateHit != null)
        {
	        Transform t = validateHit.Value.transform;
	        while (t.parent != null)
	        {
		        var z = t.parent.GetComponent<EnemyController>();
		        if (z != null)
		        {
			        z.OnDamage();
			        var rb = validateHit.Value.rigidbody;
			        rb.AddForce(-validateHit.Value.normal * 100f, ForceMode.Impulse);

			        break;
		        }
		        t = t.parent.transform;
	        }
	        //validateHit.Value.collider.gameObject.GetComponent<ZombeBehaviour>().OnDamage();
	        bulletView.SetActive(false);
	        hitTargetSound.Play();
	        Physics.IgnoreCollision( arrowHeadRB, validateHit.Value.collider );
	        _enemyHits.Clear();
        }

        // Check if arrow is shot inside or too close to an object
        /*
        RaycastHit[] hits = Physics.SphereCastAll( transform.position, 0.01f, transform.forward, 0.80f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore );
        foreach ( RaycastHit hit in hits )
        {
            if ( hit.collider.gameObject != gameObject
                 && hit.collider.gameObject != arrowHeadRB.gameObject
                 && hit.collider != Player.instance.headCollider
                 && hit.transform.name != "PistolPrefab")
            {
	            Debug.LogWarning("DESTROYED " + hit.transform.name);
                Destroy( gameObject );
                return;
            }
        }
		*/
        //SetCollisionMode(CollisionDetectionMode.ContinuousDynamic);

        Destroy( gameObject, 10 );
    }
    
    protected void SetCollisionMode(CollisionDetectionMode newMode, bool force = false)
    {
        Rigidbody[] rigidBodies = this.GetComponentsInChildren<Rigidbody>();
        for (int rigidBodyIndex = 0; rigidBodyIndex < rigidBodies.Length; rigidBodyIndex++)
        {
            if (rigidBodies[rigidBodyIndex].isKinematic == false || force)
                rigidBodies[rigidBodyIndex].collisionDetectionMode = newMode;
        }
    }
    
    //-------------------------------------------------
	void OnCollisionEnter( Collision collision )
	{
		return;
		if ( inFlight )
		{
			//Rigidbody rb = GetComponent<Rigidbody>();
			//float rbSpeed = rb.velocity.sqrMagnitude;
			bool hitBalloon = collision.collider.gameObject.GetComponent<Balloon>() != null;
			bool hitTarget = collision.collider.gameObject.GetComponent<ExplosionWobble>() != null;


			// Only play hit sounds if we're moving quickly
			//if ( rbSpeed > 0.1f )
			{
				//hitGroundSound.Play();
			}

			// Only count collisions with good speed so that arrows on the ground can't deal damage
			// always pop balloons
			if (/* rbSpeed > 0.1f*/ hitTarget || hitBalloon )
			{
				collision.collider.gameObject.SendMessageUpwards( "ApplyDamage", SendMessageOptions.DontRequireReceiver );
				gameObject.SendMessage( "HasAppliedDamage", SendMessageOptions.DontRequireReceiver );
				hitTargetSound.Play();
			}

			if ( hitBalloon )
			{
				Physics.IgnoreCollision( arrowHeadRB, collision.collider );
			}

			// Player Collision Check (self hit)
			if ( Player.instance && collision.collider == Player.instance.headCollider )
			{
				Player.instance.PlayerShotSelf();
			}
			
			Destroy( gameObject );
		}
	}
}
