﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Move2D
{
	/// <summary>
	/// An interactable pickup, gives points to the players when the sphere collides with it
	/// </summary>
	public class Pickup : NetworkBehaviour, IEnterInteractable
	{
		public delegate void PickupHandler ();
		public static event PickupHandler onPickupEnter;
		/// <summary>
		/// The number of points given to the players when the sphere collides with the pickup.
		/// </summary>
		[Tooltip ("The number of points given to the players when the sphere collides with the pickup.")]
		public int scoreValue = 1;

		#region IInteractable implementation

		[Server]
		public void OnEnterEffect (SphereCDM sphere)
		{
			this.gameObject.SetActive (false);
			GameManager.singleton.AddToScore (scoreValue);
			if (GameManager.singleton.GetCurrentLevel ().sphereVisibility == Level.SphereVisibility.FadeAfterStartLevel ||
			   GameManager.singleton.GetCurrentLevel ().sphereVisibility == Level.SphereVisibility.Invisible) {
				sphere.Blink ();
			}
			RpcDisable ();
			RpcPickupEvent ();
		}

		#endregion

		[ClientRpc]
		public void RpcDisable ()
		{
			this.gameObject.SetActive (false);
		}

		[ClientRpc]
		public void RpcPickupEvent ()
		{
			if (onPickupEnter != null)
				onPickupEnter ();
		}
	}
}
