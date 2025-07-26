using Pospec.Saving;
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Pospec.VolumeSettings
{
    public class VolumeSettings : MonoBehaviour
    {
        [Tooltip("Expose parameters '" + masterVolumeName + "' , '" + musicVolumeName + "' , '" + soundVolumeName + "' and '" + dialogueVolumeName + "' on AudioMixer volume tabs")]
        [SerializeField] private AudioMixer audioMixer;

        [SerializeField] private VolumeSetter masterSetter;
        [SerializeField] private VolumeSetter musicSetter;
        [SerializeField] private VolumeSetter soundSetter;
        [SerializeField] private VolumeSetter dialogueSetter;

        private const string masterVolumeName = "MasterVolume";
        private const string musicVolumeName = "MusicVolume";
        private const string soundVolumeName = "SoundVolume";
        private const string dialogueVolumeName = "DialogueVolume";
        private const string saveKey = "AudioVolumes";

        private static AudioSettingsData _data;
        private static AudioSettingsData Data
        {
            get
            {
                if (_data == null)
                {
                    if (!SaveManager.Load(saveKey, out _data))
                    {
                        _data = new AudioSettingsData(1);
                    }
                }
                return _data;
            }
        }

        private void Start()
        {
            try
            {
                SetMasterVolume(Data.MasterVolume);
                SetMusicVolume(Data.MusicVolume);
                SetSoundVolume(Data.SoundVolume);
                SetDialogueVolume(Data.DialogueVolume);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while applying settings: " + ex.Message);
            }
        }

        private void OnEnable()
        {
            if (masterSetter != null)
            {
                masterSetter.UpdateUI(Data.MasterVolume);
                masterSetter.onChanged += SetMasterVolume;
            }
            if (musicSetter != null)
            {
                musicSetter.UpdateUI(Data.MasterVolume);
                musicSetter.onChanged += SetMusicVolume;

            }
            if (soundSetter != null)
            {
                soundSetter.UpdateUI(Data.MasterVolume);
                soundSetter.onChanged += SetSoundVolume;
            }
            if (dialogueSetter != null)
            {
                dialogueSetter.UpdateUI(Data.MasterVolume);
                dialogueSetter.onChanged += SetDialogueVolume;
            }
        }

        private void OnDisable()
        {
            if (masterSetter != null)
                masterSetter.onChanged -= SetMasterVolume;
            if (musicSetter != null)
                musicSetter.onChanged -= SetMusicVolume;
            if (soundSetter != null)
                soundSetter.onChanged -= SetSoundVolume;
            if (dialogueSetter != null)
                dialogueSetter.onChanged -= SetDialogueVolume;
        }

        public void SetMasterVolume(float volume)
        {
            audioMixer.SetFloat(masterVolumeName, SliderToMixer(volume));
            Data.MasterVolume = volume;
            ValueChanged();
        }

        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat(musicVolumeName, SliderToMixer(volume));
            Data.MusicVolume = volume;
            ValueChanged();
        }

        public void SetSoundVolume(float volume)
        {
            audioMixer.SetFloat(soundVolumeName, SliderToMixer(volume));
            Data.SoundVolume = volume;
            ValueChanged();
        }

        public void SetDialogueVolume(float volume)
        {
            audioMixer.SetFloat(dialogueVolumeName, SliderToMixer(volume));
            Data.DialogueVolume = volume;
            ValueChanged();
        }

        private static float SliderToMixer(float sliderVal) => Mathf.Log10(sliderVal) * 20;

        private void ValueChanged()
        {
            SaveManager.SaveShared(_data, saveKey);
        }

        private class AudioSettingsData : SaveData
        {
            public float MasterVolume = 1;
            public float MusicVolume = 1;
            public float SoundVolume = 1;
            public float DialogueVolume = 1;

            public AudioSettingsData(int version) : base(version)
            {
            }
        }
    }
}
