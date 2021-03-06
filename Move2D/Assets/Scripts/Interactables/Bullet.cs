using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Move2D
{
	/// <summary>
	/// A Bullet. Is fired by the BulletBuilder and goes in the direction set at its initialization.
	/// </summary>
	public class Bullet: NetworkBehaviour, IEnterInteractable
	{
		/// <summary>
		/// The number of points the players loses when the sphere collides with the pickup.
		/// </summary>
		public int scoreValue = 1;

		/// <summary>
		/// The bullet direction
		/// </summary>
		[SyncVar] public Vector3 direction;

		/// <summary>
		/// The bullet velocity
		/// </summary>
		public float velocity = 2.0f;

		/// <summary>
		/// The duration until the bullet is destroyed
		/// </summary>
		public float bulletLifeSpan = 5.0f;

		/// <summary>
		/// Time until the bullet starts moving
		/// </summary>
		public float timeUntilStart = 2.0f;

		private bool _moveEnabled = false;

		#region IInteractable implementation

		[Server]
		public void OnEnterEffect (SphereCDM sphere)
		{
			GameManager.singleton.AddToScore (-scoreValue);
			this.velocity = 0.0f;
			if (GameManager.singleton.invisibleSphere)
				sphere.Damage ();
			NetworkManager.Destroy (this.gameObject);
		}

		#endregion

		void OnAnimationEnd ()
		{
			if (isServer)
				NetworkServer.Destroy (this.gameObject);
		}

		void Start ()
		{
			Destroy (gameObject, bulletLifeSpan);
			/*Timing.CallDelayed (timeUntilStart, delegate {
				_moveEnabled = true;
			});*/
		}

		/// <summary>
		/// Move this instance.
		/// </summary>
		void Move ()
		{
			this.transform.position += this.direction * this.velocity * Time.fixedDeltaTime;
		}

		void FixedUpdate ()
		{
			if (_moveEnabled)
				Move ();
		}
	}
}