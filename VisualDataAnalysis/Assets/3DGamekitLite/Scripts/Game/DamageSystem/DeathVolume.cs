using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gamekit3D
{
    [RequireComponent(typeof(Collider))]
    public class DeathVolume : MonoBehaviour
    {
        public new AudioSource audio;

        public UnityEvent OnDeath;

        void OnTriggerEnter(Collider other)
        {
            var pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                OnDeath.Invoke();
                pc.Die(new Damageable.DamageMessage());
            }
            if (audio != null)
            {
                audio.transform.position = other.transform.position;
                if (!audio.isPlaying)
                    audio.Play();
            }
        }

        void Reset()
        {
            if (LayerMask.LayerToName(gameObject.layer) == "Default")
                gameObject.layer = LayerMask.NameToLayer("Environment");
            var c = GetComponent<Collider>();
            if (c != null)
                c.isTrigger = true;
        }

    }
}
