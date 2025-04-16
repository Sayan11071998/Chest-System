using ChestSystem.Sound;
using UnityEngine;
using System;
using ChestSystem.Events;

namespace ChestSystem.Audio
{
    public class SoundService
    {
        private SoundScriptableObject soundScriptableObject;
        private AudioSource audioEffects;
        private AudioSource backgroundMusic;

        public SoundService(SoundScriptableObject soundScriptableObject, AudioSource audioEffectSource, AudioSource bgMusicSource)
        {
            this.soundScriptableObject = soundScriptableObject;
            audioEffects = audioEffectSource;
            backgroundMusic = bgMusicSource;
            PlaybackgroundMusic(SoundType.BACKGROUND_MUSIC_MAIN, true);

            RegisterSoundEventListeners();
        }

        private void RegisterSoundEventListeners()
        {
            if (EventService.Instance == null) return;

            EventService.Instance.OnChestSpawned.AddListener(chest => PlaySoundEffects(SoundType.CHEST_CLICK));
            EventService.Instance.OnChestUnlockStarted.AddListener(chest => PlaySoundEffects(SoundType.CHEST_UNLOCK_START));
            EventService.Instance.OnChestUnlockCompleted.AddListener(chest => PlaySoundEffects(SoundType.CHEST_UNLOCK_COMPLETE));
            EventService.Instance.OnChestCollected.AddListener((chest, coins, gems) => PlaySoundEffects(SoundType.CHEST_COLLECT));
            EventService.Instance.OnMaxSlotsIncreased.AddListener(slots => PlaySoundEffects(SoundType.ADD_SLOT));

            EventService.Instance.OnUIButtonClick.AddListener(() => PlaySoundEffects(SoundType.UI_BUTTON_CLICK));
            EventService.Instance.OnNotificationShow.AddListener(() => PlaySoundEffects(SoundType.NOTIFICATION_SHOW));
            EventService.Instance.OnNotificationClose.AddListener(() => PlaySoundEffects(SoundType.NOTIFICATION_CLOSE));
            EventService.Instance.OnGemsSpend.AddListener(() => PlaySoundEffects(SoundType.GEMS_SPEND));
        }

        public void UnregisterSoundEventListeners()
        {
            if (EventService.Instance == null) return;

            EventService.Instance.OnChestSpawned.RemoveListener(chest => PlaySoundEffects(SoundType.CHEST_CLICK));
            EventService.Instance.OnChestUnlockStarted.RemoveListener(chest => PlaySoundEffects(SoundType.CHEST_UNLOCK_START));
            EventService.Instance.OnChestUnlockCompleted.RemoveListener(chest => PlaySoundEffects(SoundType.CHEST_UNLOCK_COMPLETE));
            EventService.Instance.OnChestCollected.RemoveListener((chest, coins, gems) => PlaySoundEffects(SoundType.CHEST_COLLECT));
            EventService.Instance.OnMaxSlotsIncreased.RemoveListener(slots => PlaySoundEffects(SoundType.ADD_SLOT));

            EventService.Instance.OnUIButtonClick.RemoveListener(() => PlaySoundEffects(SoundType.UI_BUTTON_CLICK));
            EventService.Instance.OnNotificationShow.RemoveListener(() => PlaySoundEffects(SoundType.NOTIFICATION_SHOW));
            EventService.Instance.OnNotificationClose.RemoveListener(() => PlaySoundEffects(SoundType.NOTIFICATION_CLOSE));
            EventService.Instance.OnGemsSpend.RemoveListener(() => PlaySoundEffects(SoundType.GEMS_SPEND));
        }

        public void PlaySoundEffects(SoundType soundType, bool loopSound = false)
        {
            AudioClip clip = GetSoundClip(soundType);
            if (clip != null)
            {
                audioEffects.loop = loopSound;
                audioEffects.clip = clip;
                audioEffects.PlayOneShot(clip);
            }
            else
                Debug.LogError("No Audio Clip selected.");
        }

        private void PlaybackgroundMusic(SoundType soundType, bool loopSound = false)
        {
            AudioClip clip = GetSoundClip(soundType);
            if (clip != null)
            {
                backgroundMusic.loop = loopSound;
                backgroundMusic.clip = clip;
                backgroundMusic.Play();
            }
            else
                Debug.LogError("No Audio Clip selected.");
        }

        private AudioClip GetSoundClip(SoundType soundType)
        {
            Sounds sound = Array.Find(soundScriptableObject.audioList, item => item.soundType == soundType);
            return sound.audio;
        }
    }
}