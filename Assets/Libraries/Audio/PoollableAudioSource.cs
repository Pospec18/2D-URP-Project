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
        [SerializeField] private AudioSource source;
        private IObjectPool<PoollableAudioSource> pool;
        
        public PoollableAudioSource Setup(IObjectPool<PoollableAudioSource> pool)
        {
            this.pool = pool;
            return this;
        }

        public void Play(AudioEvent audioEvent)
        {
            source.spatialBlend = 0;
            audioEvent.Play(source);
            if (!source.loop)
                StartCoroutine(ReturnToPool(source.clip.length));
        }

        public void PlayFrom(AudioEvent audioEvent, Vector3 pos)
        {
            source.spatialBlend = 1;
            transform.position = pos;
            audioEvent.Play(source);
            if (!source.loop)
                StartCoroutine(ReturnToPool(source.clip.length));
        }

        private IEnumerator ReturnToPool(float clipLength)
        {
            yield return new WaitForSeconds(clipLength);
            if (pool != null)
                pool.Release(this);
        }
    }
}
