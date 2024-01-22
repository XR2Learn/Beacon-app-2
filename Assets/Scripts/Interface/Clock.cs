using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour {
	////Saving - Changed to public
	//[HideInInspector]
	private Text clock_numerical_text;

	public static string clock_time;

	//Saving - Changed to public
	[HideInInspector]
	public static int OldSecond;
	public static int SecondsIndex;

	public static bool IsTimeAccelerationEnabled;

	public static float speed_factor;

	public static bool IsClockRunning;

	int AutoPlayTimer = 0;

	const int AutoPlayerClockSpeed = 540;


	// Use this for initialization
	void Awake () {

		IsClockRunning = true;

		SecondsIndex = 0;

		clock_numerical_text = GetComponent<Text> ();
		clock_time = clock_numerical_text.text;

		OldSecond = DateTime.Now.Second;

		AutoPlayTimer = 0;

		IsTimeAccelerationEnabled = false;

		speed_factor = 10F;

	}


	// Update is called once per frame
	void Update () {

		AutoPlayTimer++;

		if (IsClockRunning) //&& SaveLoad.freezeUpdate == false) (Προσθήκη για να παγώνει ο χρόνος όταν εκτελείται το saving)
			if (IsTimeAccelerationEnabled) {
				SecondsIndex++;
				update_clock ();
				OldSecond = DateTime.Now.Second;
			}

	}

	void update_clock () {
		
		int MinutesIndex = SecondsIndex / 60;
		int HoursIndex = MinutesIndex / 60;

		clock_time = clock_numerical_text.text = string.Concat (visualized (HoursIndex % 24) + ":"
		+ visualized (MinutesIndex % 60) + ":" + visualized (SecondsIndex % 60));

	}


	public void enable_time_acceleration () {
		IsTimeAccelerationEnabled = true;
		speed_factor = 420F;
	}


	public void disable_time_acceleration () {
		IsTimeAccelerationEnabled = false;
		speed_factor = 10F;
	}
		

	string visualized (int _time) {
	
		if (_time < 10)
			return string.Concat ("0" + _time);
		else
			return string.Concat (_time);
	
	}



}
