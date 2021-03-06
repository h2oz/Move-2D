﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProgressBar;

namespace Move2D
{
	/// <summary>
	/// Class to be used to compute the distance between the Sphere and the PointFollow into a radial progress
	/// </summary>
	[RequireComponent (typeof(ProgressRadialBehaviour))]
	public class ProgressBarUI : MonoBehaviour
	{
		private bool _active;
		private GameObject _pointFollow;
		private GameObject _sphereCDM;

		void OnEnable ()
		{
			GameManager.onLevelStarted += OnLevelStarted;
		}

		void OnDisable ()
		{
			GameManager.onLevelStarted -= OnLevelStarted;
		}

		void OnLevelStarted ()
		{
			_active = true;
			this.transform.parent.gameObject.GetComponent<CanvasGroup> ().alpha = 1;
			/*
			_active = (GameManager.singleton.GetCurrentLevel ().spawnMotionPointFollow
			&& GameManager.singleton.GetCurrentLevel ().sphereVisibility == Level.SphereVisibility.Visible);
			if (_active) {
				this.transform.parent.gameObject.GetComponent<CanvasGroup> ().alpha = 1;
				_pointFollow = GameObject.FindGameObjectWithTag ("PointFollow");
				_sphereCDM = GameObject.FindGameObjectWithTag ("SphereCDM");
			} else {
				this.transform.parent.gameObject.GetComponent<CanvasGroup> ().alpha = 0;
			}b
			*/
		}

		void Update ()
		{
			if (_active) {
				this.GetComponent<ProgressRadialBehaviour> ().Value = Mathf.Lerp(
					this.GetComponent<ProgressRadialBehaviour> ().Value,
					LevelManager.singleton.scorePrerequisiteProgress * 100.0f,
					0.5f);
				//var criterion = _sphereCDM.GetComponent<SpherePhysics> ().XISquareCriterion (_pointFollow.transform.position);
				//this.GetComponent<ProgressRadialBehaviour> ().Value = criterion;
			}
		}
	}
}