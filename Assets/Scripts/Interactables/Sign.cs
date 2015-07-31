using UnityEngine;
using System.Collections;

public class Sign : Interactable {
	
    public string SignText;
    public GameObject DialogPrefab;

	private float passedTime;
	private bool startTimer;
    private GameObject dialog;

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
	void Start () {		
	}
	
	// Update is called once per frame
	void Update () {
		if (startTimer) {
			passedTime += Time.deltaTime;
			if (passedTime > InputDelay)
				startTimer = false;
		}
	}

	public override void Interact() {
        if (dialog == null)
        {
            dialog = GameObject.Instantiate(DialogPrefab, Vector3.zero, Quaternion.identity) as GameObject;

            Dialog.OnClosed += DialogClosed;
            dialog.GetComponent<Dialog>().StartConversation(SignText);
        }
	}

	void DialogClosed() {
		Dialog.OnClosed -= DialogClosed;

        GameObject.Destroy(dialog);

		passedTime = 0f;
		startTimer = true;

        FindObjectOfType<InputManager>().DisableCharacterMotor = false;
	}
}
