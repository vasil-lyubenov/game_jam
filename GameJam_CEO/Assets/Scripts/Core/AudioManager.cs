using System.Collections;
using UnityEngine;

namespace CEOGame.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] AudioSource ambientSource;
        [SerializeField] AudioSource sfxSource;

        [Header("Ambient")]
        [SerializeField] AudioClip ambientLoop;

        [Header("Random Office Noises")]
        [SerializeField] AudioClip[] officeNoises;
        [SerializeField, Range(8f, 25f)] float noiseIntervalMin = 8f;
        [SerializeField, Range(8f, 25f)] float noiseIntervalMax = 25f;

        [Header("UI SFX Clips")]
        [SerializeField] AudioClip approveClip;
        [SerializeField] AudioClip denyClip;
        [SerializeField] AudioClip cardPickupClip;
        [SerializeField] AudioClip companySheetClip;
        [SerializeField] AudioClip hrTipChimeClip;

        [Header("Volume")]
        [SerializeField, Range(0f, 1f)] float masterVolume = 1f;
        [SerializeField, Range(0f, 1f)] float ambientVolume = 0.4f;
        [SerializeField, Range(0f, 1f)] float sfxVolume = 1f;
        [SerializeField, Range(0f, 1f)] float noiseVolume = 0.5f;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        void Start()
        {
            if (ambientLoop != null && ambientSource != null)
            {
                ambientSource.clip = ambientLoop;
                ambientSource.loop = true;
                ambientSource.volume = masterVolume * ambientVolume;
                ambientSource.Play();
            }

            if (officeNoises != null && officeNoises.Length > 0)
                StartCoroutine(RandomNoiseCoroutine());
        }

        IEnumerator RandomNoiseCoroutine()
        {
            while (true)
            {
                float wait = Random.Range(noiseIntervalMin, noiseIntervalMax);
                yield return new WaitForSeconds(wait);

                var clip = officeNoises[Random.Range(0, officeNoises.Length)];
                if (clip != null && sfxSource != null)
                    sfxSource.PlayOneShot(clip, masterVolume * noiseVolume);
            }
        }

        public void PlayApprove() => PlaySFX(approveClip);
        public void PlayDeny() => PlaySFX(denyClip);
        public void PlayCardPickup() => PlaySFX(cardPickupClip);
        public void PlayCompanySheet() => PlaySFX(companySheetClip);
        public void PlayHRTipChime() => PlaySFX(hrTipChimeClip);

        void PlaySFX(AudioClip clip)
        {
            if (clip != null && sfxSource != null)
                sfxSource.PlayOneShot(clip, masterVolume * sfxVolume);
        }
    }
}
