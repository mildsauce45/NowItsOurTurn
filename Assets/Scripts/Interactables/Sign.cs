using FirstWave.Core.Extensions;
using FirstWave.Core.GUI.Dialogs;
using UnityEngine;

public class Sign : Interactable
{
	public string SignText;
	public GameObject DialogPrefab;

	private float passedTime;
	private bool startTimer;
	private Dialog dialog;

	public float InputDelay = 0.5f;

	public override bool DisableCharacterMotor
	{
		get { return true; }
	}

	public override bool AllowInteraction
	{
		get { return !startTimer || passedTime > InputDelay; }
	}

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (startTimer)
		{
			passedTime += Time.deltaTime;
			if (passedTime > InputDelay)
				startTimer = false;
		}
	}

	public override void Interact()
	{
		if (dialog == null)
		{
			dialog = DialogPrefab.Instantiate().GetComponent<Dialog>();

			dialog.OnClosed += DialogClosed;
			dialog.GetComponent<Dialog>().StartConversation(SignText);
		}
	}

	void DialogClosed(string branch)
	{
		dialog.OnClosed -= DialogClosed;

		GameObject.Destroy(dialog);

		passedTime = 0f;
		startTimer = true;

		FindObjectOfType<InputManager>().DisableCharacterMotor = false;
	}
}
