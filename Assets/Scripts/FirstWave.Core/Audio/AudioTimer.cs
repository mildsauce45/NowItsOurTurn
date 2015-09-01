using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstWave.Core;
using UnityEngine;

namespace FirstWave.Core.Audio
{
	public class AudioTimer : ITimer
	{
		private AudioSource audioSource;
		private bool enabled;

		public bool IsComplete
		{
			get { return enabled && !audioSource.isPlaying; }
		}

		public bool Enabled
		{
			get { return enabled; }
		}

		public AudioTimer(AudioSource audioSource)
		{
			this.audioSource = audioSource;
		}

		public void SetAudioClip(AudioClip clip)
		{
			if (Enabled || audioSource.isPlaying)
				Stop();

			audioSource.clip = clip;
		}

		public void Start()
		{
			if (!enabled)
			{
				enabled = true;
				audioSource.Play();
			}
		}

		public void Update()
		{
			// Pretty sure we can just ignore this method, the audio source is going to update itself
		}

		public void Stop()
		{
			enabled = false;

			audioSource.Stop();
		}
	}
}
