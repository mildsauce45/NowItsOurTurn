using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : Singleton<InputManager>
{
	public string[] KeysToMap;

	private IDictionary<string, bool> prevState;
	private IDictionary<string, bool> currentState;

    public bool DisableCharacterMotor = false;

	void Awake()
	{
		prevState = new Dictionary<string, bool>();
		currentState = new Dictionary<string, bool>();
	}

	// Update is called once per frame
	void Update()
	{
		// Move the existing current state back into the previous state collection
		foreach (var key in currentState.Keys)
			prevState[key] = currentState[key];

		currentState.Clear();

		// Now handle each key we care about
		currentState.Add("Interact", Input.GetButton("Interact"));
        currentState.Add("Cancel", Input.GetButton("Cancel"));
		currentState.Add("Menu", Input.GetButton("Menu"));
	}

	public bool KeyReleased(string key)
	{
		return prevState.ContainsKey(key) && prevState[key] && currentState.ContainsKey(key) && !currentState[key];
	}

	public bool KeyPressed(string key)
	{
		return prevState.ContainsKey(key) && !prevState[key] && currentState.ContainsKey(key) && currentState[key];
	}

	public bool KeyDown(string key)
	{
		return currentState.ContainsKey(key) && !currentState[key];
	}

	public bool VerticalAxisUp()
	{
		return Input.GetAxisRaw("Vertical") > 0;
	}

	public bool VerticalAxisDown()
	{
		return Input.GetAxisRaw("Vertical") < 0;
	}

	public bool HorizontalAxisLeft()
	{
		return Input.GetAxisRaw("Horizontal") < 0;
	}

	public bool HorizontalAxisRight()
	{
		return Input.GetAxisRaw("Horizontal") > 0;
	}

	public void Flush()
	{
		prevState.Clear();
		currentState.Clear();
	}

	public void FlushKey(string key)
	{
		if (prevState.ContainsKey(key))
			prevState.Remove(key);

		if (currentState.ContainsKey(key))
			currentState.Remove(key);
	}
}
