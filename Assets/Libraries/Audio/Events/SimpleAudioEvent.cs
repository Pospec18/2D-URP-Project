using UnityEngine;
using Pospec.Common;
using Random = UnityEngine.Random;

namespace Pospec.Audio
{
	[CreateAssetMenu(menuName="Audio Events/Simple")]
	public class SimpleAudioEvent : AudioEvent
	{
		public AudioClip[] clips;

		public RangedFloat volume;

		[MinMaxRange(0, 2)]
		public RangedFloat pitch;

		public bool loop;

		public override void Play(AudioSource source)
		{
			if (clips.Length == 0) return;

			source.clip = clips[Random.Range(0, clips.Length)];
			source.volume = Random.Range(volume.minValue, volume.maxValue);
			source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
			source.loop = loop;
			source.Play();
		}
	}
}
