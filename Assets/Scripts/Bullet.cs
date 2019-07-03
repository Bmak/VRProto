using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Bullet : MonoBehaviour
{
    public PhysicMaterial targetPhysMaterial;
    public GameObject bulletView;
    public ParticleSystem hitEffect;
    
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

	
    private Transform _weapon;
    //-------------------------------------------------
    public void BulletReleased(Transform weapon)
    {
	    _weapon = weapon;
	    inFlight = true;
	    
        airReleaseSound.Play();
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit))
        {
	        SpawnParticles(hitEffect, hit.point);
	        
	        Transform t = hit.transform;
	        while (t.parent != null)
	        {
		        var z = t.parent.GetComponent<EnemyController>();
		        if (z != null)
		        {
			        z.OnDamage();
			        var rb = hit.rigidbody;
			        rb.AddForce(-hit.normal * 100f, ForceMode.Impulse);

			        hitTargetSound.Play();

			        break;
		        }
		        t = t.parent.transform;
	        }
	        //bulletView.SetActive(false);
	        Destroy( gameObject, 2f );
	        Physics.IgnoreCollision( arrowHeadRB, hit.collider );
	        return;
        }
        
        Destroy( gameObject, 10 );
    }

    private bool bParticlesSpawned = false;
    private void SpawnParticles(ParticleSystem particlePrefab, Vector3 position)
    {
	    // Don't do this twice
	    if ( bParticlesSpawned )
	    {
		    return;
	    }

	    bParticlesSpawned = true;

	    if ( particlePrefab != null )
	    {
		    ParticleSystem particleObject = Instantiate( particlePrefab, position, transform.rotation ) as ParticleSystem;
		    particleObject.Play();
		    Destroy( particleObject, 2f );
	    }
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
