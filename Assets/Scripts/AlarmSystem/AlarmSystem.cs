using System;
using System.Collections;
using Scripts.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.AlarmSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class AlarmSystem : MonoBehaviour
    {
        [SerializeField] private EnterTriggerComponent _coverageArea;
        
        [Header("Alarm sound")]
        [SerializeField] private AudioClip _alarmSound;
        [Tooltip("in seconds")] [SerializeField] private float _timeToMax = 1f;

        [Header("Lamp signal")] 
        [SerializeField] private SpriteRenderer _lamp;
        [SerializeField] private Sprite _spriteLampOff;
        [SerializeField] private Sprite _spriteLampOn;

        private AudioSource _audioSource;
        private Coroutine _currentCoroutine;
        private float _currentVolume = 0;
        private float _delayForAlarmSound;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = _currentVolume;
            _audioSource.clip = _alarmSound;
            _lamp.sprite = _spriteLampOff;
            _delayForAlarmSound = 1f / _timeToMax;
        }

        private void OnEnable()
        {
            _coverageArea.Detection += OnDetection;
        }

        private void OnDetection(bool isCrookEnter)
        {
            if (isCrookEnter) 
                StartAlarm();
            else
                StopAlarm();
        }

        private void StartAlarm()
        {
            ConsoleTools.LogError("Жулик вошел!");
            _lamp.sprite = _spriteLampOn;
            
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            
            _currentCoroutine = StartCoroutine(TurnUpVolume());
        }

        private void StopAlarm()
        {
            ConsoleTools.LogSuccess("Жулик ушел!");
            _lamp.sprite = _spriteLampOff;
            
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            
            _currentCoroutine = StartCoroutine(TurnDownVolume());
        }

        private IEnumerator TurnUpVolume()
        {
            _audioSource.Play();
            var waitForNextFrame = new WaitForNextFrameUnit();

            while (_audioSource.volume != 1)
            {
                _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, 1, _delayForAlarmSound * Time.deltaTime);
                yield return waitForNextFrame;
            }
        }

        private IEnumerator TurnDownVolume()
        {
            var waitForNextFrame = new WaitForNextFrameUnit();

            while (_audioSource.volume != 0)
            {
                _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, 0, _delayForAlarmSound * Time.deltaTime);
                yield return waitForNextFrame;
            }
            
            _audioSource.Stop();
        }

        private void OnDisable()
        {
            _coverageArea.Detection -= OnDetection;
        }
    }
}