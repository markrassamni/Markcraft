﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[SerializeField] private Transform target;
	[SerializeField] private float distance;
	[SerializeField] private float targetHeight;

	private float x=0;
	private float y=0;

	void Start () {
		
	}
	
	void LateUpdate () {
		if (!target)
			return;
		y = target.eulerAngles.y;

		Quaternion rotation = Quaternion.Euler(x,y,0);
		transform.rotation = rotation;

		transform.position = target.position - (rotation * Vector3.forward * distance + new Vector3(0,-targetHeight,0));
		
	}
}
