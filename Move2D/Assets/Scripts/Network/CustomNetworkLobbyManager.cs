﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.NetworkSystem;

namespace Move2D
{
	public enum NetworkErrorMessage {
		ServerDisconnected,
		NotEnoughPlayers,
		ClientLeft,
		StopServer,
		None,
	}

	/// <summary>
	/// Custom NetworkLobby class
	/// </summary>
	public class CustomNetworkLobbyManager : LobbyManager
	{
		public delegate void NetworkHandler ();

		public delegate void NetworkConnectionHandler (NetworkConnection conn);

		public delegate void SceneLoadedHandler (GameObject lobbyPlayer, GameObject gamePlayer);

		/// <summary>
		/// Event called on client when a client disconnect
		/// </summary>
		public static event NetworkConnectionHandler onClientDisconnect;
		/// <summary>
		/// Event called on server when a client disconnect
		/// </summary>
		public static event NetworkConnectionHandler onServerDisconnect;
		/// <summary>
		/// Event called on client when a client connects
		/// </summary>
		public static event NetworkConnectionHandler onClientConnect;
		/// <summary>
		/// Event called on server when a client connects
		/// </summary>
		public static event NetworkConnectionHandler onServerConnect;
		/// <summary>
		/// Occurs when the scene changed on the server.
		/// </summary>
		public static event NetworkHandler onServerSceneChanged;
		/// <summary>
		/// Occurs when the scene changed on the client
		/// </summary>
		public static event NetworkConnectionHandler onClientSceneChanged;

		public NetworkServerSimple webGLServer;

		public NetworkErrorMessage errorMessage = NetworkErrorMessage.None;

		public static int secondServerHostId = 0;

		/// <summary>
		/// Called when the host is stopped. Destroy the gameManager, display an error message and reset the default lobby scene
		/// </summary>
		public override void OnStopHost ()
		{
			if (!_isMatchmaking && GameManager.singleton != null) {
				errorMessage = NetworkErrorMessage.StopServer;
				GameObject.Destroy (GameManager.singleton.gameObject);
				NetworkServer.Destroy (GameManager.singleton.gameObject);
				infoPanel.Display (errorMessage.ToMessageString(), "OK", null);
				CustomNetworkLobbyManager.networkSceneName = this.onlineScene;
			}
			base.OnStopHost ();
		}

		/// <summary>
		/// Called when the client is stopped. Destroy the gameManager, display an error message and reset the default lobby scene
		/// </summary>
		public override void OnStopClient ()
		{
			if (!_isMatchmaking && GameManager.singleton != null) {
				GameObject.Destroy (GameManager.singleton.gameObject);
				NetworkServer.Destroy (GameManager.singleton.gameObject);
				infoPanel.Display (errorMessage.ToMessageString(), "OK", null);
				CustomNetworkLobbyManager.networkSceneName = this.onlineScene;
			}
			base.OnStopClient ();
		}

		/// <summary>
		/// Called when the server is stopped. Destroy the gameManager, display an error message and reset the default lobby scene
		/// </summary>
		public override void OnStopServer ()
		{
			if (!_isMatchmaking && GameManager.singleton != null) {
				GameObject.Destroy (GameManager.singleton.gameObject);
				NetworkServer.Destroy (GameManager.singleton.gameObject);
				// LobbyManager bug fix
				foreach (var playerInfo in GameObject.FindObjectsOfType<LobbyPlayer> ()) {
					GameObject.Destroy (playerInfo.gameObject);
				}
				errorMessage = NetworkErrorMessage.StopServer;
				infoPanel.Display (errorMessage.ToMessageString(), "OK", null);
				CustomNetworkLobbyManager.networkSceneName = this.onlineScene;
			}
			this.GetComponent<WebGLCustomServer> ().Stop ();
		
			base.OnStopServer ();
		}

		public override void OnStartServer ()
		{
			base.OnStartServer ();

			if (!useWebSockets)
				this.GetComponent<WebGLCustomServer> ().Initialize ();
		}

		/// <summary>
		/// Called when the server scene is changed
		/// </summary>
		public override void OnServerSceneChanged (string sceneName)
		{
			base.OnServerSceneChanged (sceneName);
			if (onServerSceneChanged != null)
				onServerSceneChanged ();
		}

		/// <summary>
		/// Called when the client scene is changed
		/// </summary>
		public override void OnClientSceneChanged (NetworkConnection conn)
		{
			base.OnClientSceneChanged (conn);
			if (onClientSceneChanged != null)
				onClientSceneChanged (conn);
		}

		public override bool OnLobbyServerSceneLoadedForPlayer (GameObject lobbyPlayer, GameObject gamePlayer)
		{
			gamePlayer.GetComponent<Renderer> ().material.color = lobbyPlayer.GetComponent<LobbyPlayer> ().playerColor;
			gamePlayer.GetComponent<Player> ().playerName = lobbyPlayer.GetComponent<LobbyPlayer> ().playerName;
			gamePlayer.GetComponent<Player> ().color = lobbyPlayer.GetComponent<LobbyPlayer> ().playerColor;
			gamePlayer.GetComponent<Player> ().playerInfo.playerControllerId = lobbyPlayer.GetComponent<LobbyPlayer> ().playerControllerId;
			return true;
		}

		public override void OnServerDisconnect (NetworkConnection conn)
		{
			Debug.Log ("Server Disconnect");
			base.OnServerDisconnect (conn);
			if (onServerDisconnect != null)
				onServerDisconnect (conn);
		}

		public override void OnServerConnect (NetworkConnection conn)
		{
			base.OnServerConnect (conn);
			if (onServerConnect != null)
				onServerConnect (conn);
		}

		public override void OnClientDisconnect (NetworkConnection conn)
		{
			Debug.Log ("Client Disconnect");
			base.OnClientDisconnect (conn);
			if (onClientDisconnect != null)
				onClientDisconnect (conn);
		}

		public override void OnClientConnect (NetworkConnection conn)
		{
			base.OnClientConnect (conn);
			if (onClientConnect != null)
				onClientConnect (conn);
		}
	}
}