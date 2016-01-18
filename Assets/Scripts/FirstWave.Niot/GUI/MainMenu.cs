using FirstWave.Core.GUI;
using FirstWave.Niot.GUI.MenuState;
using FirstWave.StateMachine;
using FirstWave.StateMachine.Unity;
using UnityEngine;
using System.Linq;
using Assets.Scripts.GUI.MenuState;

namespace FristWave.Niot.GUI
{
	public class MainMenu : MonoBehaviour
	{
		public BorderTextures textures;
		public FontProperties fontProperties;

		private MonoBehaviour _initiator;
		public MonoBehaviour initiator
		{
			get { return _initiator; }
			set
			{
				_initiator = value;
				if (_initiator)
					_initiator.enabled = false;
			}
		}

		private StateMachine<State<MainMenuTrigger>, MainMenuTrigger> stateMachine;

		void Start()
		{
			var topMenuState = new TopMenuState(transform.FindChild("TopMenuUI").gameObject, textures, fontProperties);
			var statusState = new StatusState(transform.FindChild("StatusUI").gameObject, textures, fontProperties);

			stateMachine = new StateMachine<State<MainMenuTrigger>, MainMenuTrigger>(topMenuState);

			stateMachine
				.Configure(topMenuState)
				.OnEntry(trans => topMenuState.OnEnter())
				.OnExit(trans => topMenuState.OnExit())
				.Permit(MainMenuTrigger.StatusSelected, statusState)
				.Ignore(MainMenuTrigger.MenuExited);

			stateMachine
				.Configure(statusState)
				.OnEntry(trans => statusState.OnEnter())
				.OnExit(trans => statusState.OnExit())
				.Permit(MainMenuTrigger.ActionCanceled, topMenuState);

			topMenuState.OnEnter();
		}

		void Update()
		{
			if (stateMachine.State == null)
				return;

			stateMachine.State.Update();

			var trigger = stateMachine.State.GetTrigger();
			if (trigger == MainMenuTrigger.MenuExited)
			{
				initiator.enabled = true;

				Destroy(gameObject);
			}

			if (trigger != MainMenuTrigger.Nothing)
			{
				Debug.Log(trigger);
				stateMachine.Fire(trigger);
			}
		}
	}
}
