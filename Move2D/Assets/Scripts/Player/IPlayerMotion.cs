using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Move2D
{
	/// <summary>
	/// Use this interface to implement a new player movement module
	/// </summary>
	public interface IPlayerMotion
	{
		/// <summary>
		/// Generic move method to be called whenever you want the player to move
		/// </summary>
		bool Move ();

		/// <summary>
		/// Is the movement module activated ?
		/// </summary>
		bool IsActivated (int sliderValue);
	}
}
