using System;
using UnityEngine;

namespace FirstWave.StateMachine.Unity
{
	public abstract class State<TTrigger>
	{
		public GameObject Owner { get; private set; }

		public State(GameObject owner)
		{
			if (owner == null)
				throw new ArgumentException("Owner cannot be null");

			Owner = owner;
		}

		public abstract void Update();
		public abstract TTrigger GetTrigger();

		public virtual void OnEnter()
		{
		}

		public virtual void OnExit()
		{
		}
	}
}
