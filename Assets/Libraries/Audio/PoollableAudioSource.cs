using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Pospec.Audio
{
    /// <summary>
    /// AudioSource that works with AudioSourcePool for reusable AudioSources.
    /// When audio is played, return's itself to the pool.
    /// </summary>
    public class PoollableAudioSource : MonoBehaviour
    {
        private IObjectPool<PoollableAudioSource> pool; [SerializeField] private AudioSource source; public PoollableAudioSource Setup(IObjectPool<PoollableAudioSource> pool)
        {
            this.pool = pool;
            return this;
        }

        public void Play(AudioEvent audioEvent)
        {
            source.spatialBlend = 0;
            audioEvent.Play(source);
            StartCoroutine(ReturnToPool());
        }

        public void PlayFrom(AudioEvent audioEvent, Vector3 pos)
        {
            source.spatialBlend = 1;
            transform.position = pos;
            audioEvent.Play(source);
            StartCoroutine(ReturnToPool());
        }

        private IEnumerator ReturnToPool()
        {
            yield return new WaitUntil(() => !source.isPlaying);
            if (pool != null)
                pool.Release(this);
        }
    }
}
