using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ProgressBar;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Move2D
{
	/// <summary>  
	///  This class manages the position of the sphere in relation to each of the other players
	/// </summary>  
	public class SpherePhysics: NetworkBehaviour
	{
		/// <summary>
		/// The list of players that influence the sphere
		/// </summary>
		[Tooltip ("The list of players that can influence the sphere")]
		public List<Player> players = new List<Player> ();
		public bool hasMoved { get; private set; }
		private Vector3 _previousPos = Vector3.zero;

		void OnEnable ()
		{
			Player.onPlayerDestroy += OnPlayerDestroy;
		}

		void OnDisable ()
		{
			Player.onPlayerDestroy -= OnPlayerDestroy;
		}

		void OnPlayerDestroy (Player player)
		{
			players.Remove (player);
		}

		public override void OnStartClient ()
		{
			players = FindPlayers ();
			this.transform.position = this.MovePosition (players, GetTotalMass (players));
			base.OnStartClient ();
		}

		public override void OnStartServer ()
		{
			players = FindPlayers ();
			this.transform.position = this.MovePosition (players, GetTotalMass (players));
			base.OnStartServer ();
		}

		void FixedUpdate ()
		{
			MovePosition (FindPlayers(), GetTotalMass (players));
		}

		void LateUpdate()
		{
			hasMoved = _previousPos != this.transform.position;
			_previousPos = this.transform.position;
		}

		// Find all the players currently in the scene
		List<Player> FindPlayers ()
		{
			var players = GameObject.FindGameObjectsWithTag ("Player");
			var res = new List<Player> ();

			foreach (var player in players) {
				res.Add (player.GetComponent<Player> ());
			}
			return res;
		}

		// Get the total mass of all players currently in the scene
		float GetTotalMass (List<Player> players)
		{
			if (players.Count == 0)
				return 1.0f;
			var sum = 0.0f;
			foreach (var player in players) {
				sum += player.mass;
			}
			return sum;
		}

		// Change the position of the sphere according to the players and their mass
		Vector2 MovePosition (List<Player> players, float sum)
		{
			// Don't move if the game is paused
			if (GameManager.singleton != null && !GameManager.singleton.isPlaying)
				return this.GetComponent<Rigidbody2D> ().position;
			Vector2 posTemp = Vector2.zero;
			if (players != null && players.Count >= 2) {
				foreach (var player in players)
					posTemp += (Vector2)player.gameObject.transform.position * (player.mass * 1.0f) / sum; 
				this.GetComponent<Rigidbody2D> ().MovePosition (posTemp);
			} else if (players != null && players.Count == 1) {
				this.GetComponent<Rigidbody2D> ().MovePosition ((Vector2)players [0].gameObject.transform.position);
			} else
				this.GetComponent<Rigidbody2D> ().MovePosition (new Vector2 (0, 0));
			return this.GetComponent<Rigidbody2D> ().position;
		}

		// Computing the distance between the sphere and a position as a purcentage
		public float XISquareCriterion (Vector3 position)
		{
			float criterion;
			int radiusCriterion = 20;

			criterion = Vector3.SqrMagnitude (this.transform.position - position) /
			Vector3.SqrMagnitude (new Vector3 (0, 0, radiusCriterion));

			return 100 * (1.0f - Mathf.Max (criterion, 0.01f));
		}
	} 
}