//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Demonstrates how to create a simple interactable object
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
using Valve.VR.Extras;

namespace Valve.VR.InteractionSystem.Sample
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
	public class PistolShootController : MonoBehaviour
	{
		public Bullet BulletPrefab;
		public Transform BulletStartPosition;
		public GameObject ObjectSpawner;
		public GameObject WeaponBody;
		public ParticleSystem MuzzleFlash;
		public WFX_LightFlicker WFX_LightFlicker;
		public SoundPlayOneshot reloadSound;
		public SoundPlayOneshot cantShootSound;
	    
        private Vector3 oldPosition;
		private Quaternion oldRotation;

		private float attachTime;

		private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ( ~Hand.AttachmentFlags.SnapOnAttach ) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);

        private Interactable interactable;

        private InteractableObject _interactableObject;
        
        public int MAX_AMMO = 15;
        public int BULLET_SPEED = 80;
        [SerializeField]
        private Text _ammoText;

        private int _currentAmmo;

        private Quaternion _defaultRotation;
        
		//-------------------------------------------------
		void Awake()
		{
            _ammoText.gameObject.SetActive(false);
            
            interactable = this.GetComponent<Interactable>();
            _interactableObject = this.GetComponent<InteractableObject>();
            _currentAmmo = MAX_AMMO;
            _ammoText.text = _currentAmmo.ToString();
            
            _defaultRotation = WeaponBody.transform.localRotation;
		}
		
        private bool lastHovering = false;


        private float _recoilRecoverSpeed = 10f;
        private float _recoilAmount = 5f;
        
        private void Update()
        {
	        if (!_interactableObject.IsGrabbing) return;
	        
	        bool stateDown = SteamVR_Input.GetStateDown("GrabPinch", SteamVR_Input_Sources.RightHand);
	        if (stateDown)
	        {
		        FireBullet();
	        }
	        //TODO Добавить помимо поворота при отдаче и смещение оружия
	        WeaponBody.transform.localRotation = Quaternion.Lerp(WeaponBody.transform.localRotation, _defaultRotation, _recoilRecoverSpeed * Time.deltaTime);
	        
	        
            if (interactable.isHovering != lastHovering) //save on the .tostrings a bit
            {
                lastHovering = interactable.isHovering;
            }
        }
        
        //-------------------------------------------------
        private void FireBullet()
        {
	        if (!CheckCanShoot())
	        {
		        cantShootSound.Play();
		        return;
	        }
	        
	        _currentAmmo--;
	        UpdateAmmo();
	        
	        MuzzleFlash.Play();
	        WFX_LightFlicker.Play();
	        
	        Bullet bullet = Instantiate(BulletPrefab, gameObject.transform);
	        bullet.SetSpeed(BULLET_SPEED);
	        bullet.transform.SetPositionAndRotation(BulletStartPosition.position, BulletStartPosition.rotation);
	        
	        Physics.IgnoreCollision( bullet.GetComponent<Collider>(), Player.instance.headCollider );
	        Physics.IgnoreCollision( bullet.GetComponent<Collider>(), gameObject.GetComponent<Collider>() );
	        
	        bullet.transform.SetParent(ObjectSpawner.transform);
	        
	        bullet.BulletReleased(transform);
	        
	        GunRecoil();
        }

        private void GunRecoil()
        {
	        WeaponBody.transform.Rotate(Vector3.forward * _recoilAmount, Space.Self);

        }

        private void UpdateAmmo()
        {
	        _currentAmmo = _currentAmmo >= 0 ? _currentAmmo : 0;
	        _ammoText.text = _currentAmmo.ToString();
        }

        private bool CheckCanShoot()
        {
	        return _currentAmmo > 0;
        }

        public void ReloadWeapon()
        {
	        if(_currentAmmo == MAX_AMMO) return;
	        
	        reloadSound.Play();
	        _currentAmmo = MAX_AMMO;
	        UpdateAmmo();
        }

		public void ShowAmmo(bool isGrabbing)
		{
			_ammoText.gameObject.SetActive(isGrabbing);
		}
	}
}
