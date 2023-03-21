using System;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.AlarmSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class EnterTriggerComponent : MonoBehaviour
    {
        public event UnityAction<bool> Detection;

        private bool _isCrookEnter;

        private void Start()
        {
            var collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<Crook>(out Crook crook))
            {
                _isCrookEnter = true;
                Detection?.Invoke(_isCrookEnter);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<Crook>(out Crook crook))
            {
                _isCrookEnter = false;
                Detection?.Invoke(_isCrookEnter);
            }
        }
    }
}