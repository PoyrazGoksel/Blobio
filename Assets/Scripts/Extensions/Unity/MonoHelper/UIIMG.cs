using System.Collections.Generic;
using Extensions.System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Extensions.Unity.MonoHelper
{
    public abstract class UIIMG : UIBase
    {
        [ShowIf("@_manualReference == true")][SerializeField]
        protected Image _myImg;

        [ShowIf("@_manualReference == true")][SerializeField]
        protected List<Image> _myOtherImages = new List<Image>();

        [ShowIf("@_manualReference == true")][SerializeField]
        protected List<TextMeshProUGUI> _myOtherTMPs = new List<TextMeshProUGUI>();

        [SerializeField][UsedImplicitly] 
        private bool _manualReference;

        private void OnValidate()
        {
            if(_manualReference) return;
            
            if (! _myImg)
            {
                if (transform.TryGetComponent(out Image myImg))
                {
                    _myImg = myImg;
                }
                else
                {
                    Debug.LogError("UIButtonTMP needs a TextMeshProUGUI in its children!");
                }
            }
        }

        public virtual void UpdateImage(Sprite sprite)
        {
            _myImg.sprite = sprite;
        }

        [Button(Name = "SetActive(bool isActive)", Style = ButtonStyle.Box, Expanded = true)]
        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);
            
            _myImg.enabled = isActive;
            _myOtherImages.DoToAll(oI => oI.enabled = isActive);
            _myOtherTMPs.DoToAll(oTmp => oTmp.enabled = isActive);
        }

        protected override void RegisterEvents()
        {
        }

        protected override void UnRegisterEvents()
        {
        }
    }
}