﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

	private bool isJumping = false;
	private bool isPunching = false;
	private bool isBuilding = false;
	public bool IsJumping{
		get{
			return isJumping;
		} set{
			isJumping = value;
		}
	}
	public bool IsPunching{
		get{
			return isPunching;
		} set{
			isPunching = value;
		}
	}
	public bool IsBuilding{
		get{
			return isBuilding;
		} set{
			isBuilding = value;
		}
	}

	// public void JumpButtonPressed(){
	// 	isJumping = true;
	// }
	// public void PunchButtonPressed(){
	// 	isPunching = true;
	// }
	// public void BuildButtonPressed(){
	// 	isBuilding = true;
	// }



}
