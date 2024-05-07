using System.Collections.Generic;
using Extensions.System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Extensions.Unity.MonoHelper
{
    public abstract class UITMP : UIBase
    {
        [ShowIf("@_manualReference == true")][SerializeField] 
        protected TextMeshProUGUI _myTMP;

        [ShowIf("@_manualReference == true")][SerializeField]
        protected List<Image> _myOtherImages = new List<Image>();

        [ShowIf("@_manualReference == true")][SerializeField]
        protected List<TextMeshProUGUI> _myOtherTMPs = new List<TextMeshProUGUI>();

        [SerializeField][UsedImplicitly] 
        private bool _manualReference;

        private void OnValidate()
        {
            if(_manualReference) return;
            
            if (! _myTMP)
            {
                if (transform.TryGetComponent(out TextMeshProUGUI myTmp))
                {
                    _myTMP = myTmp;
                }
                else
                {
                    Debug.LogError("UIButtonTMP needs a TextMeshProUGUI in its children!");
                }
            }
        }

        public virtual void UpdateText(string text)
        {
            _myTMP.text = text;
        }

        [Button(Name = "SetActive(bool isActive)", Style = ButtonStyle.Box, Expanded = true)]
        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);
            
            _myTMP.enabled = isActive;
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