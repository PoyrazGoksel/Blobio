using Sirenix.OdinInspector;
using UnityEngine;

namespace Extensions.Unity.MonoHelper
{
    public class UIParticleCam : UIBase
    {
        [SerializeField] protected Camera _myCam;
        [SerializeField] protected ParticleSystem _myParticleSystem;

        protected ParticleSystem.MainModule MyParticleMainMod;

        protected virtual void Awake()
        {
            MyParticleMainMod = _myParticleSystem.main;
            MyParticleMainMod.playOnAwake = false;
        }

        [Button]
        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);
            
            _myCam.enabled = isActive;

            if (isActive)
            {
                _myParticleSystem.Play();
            }
            else
            {
                _myParticleSystem.Stop();
            }
        }

        protected override void RegisterEvents()
        {
        }

        protected override void UnRegisterEvents()
        {
        }
    }
}