using System.Collections.Generic;
using Extensions.System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Extensions.Unity.MonoHelper
{
    public abstract class UIButtonIMG : UIBase
    {
        [ShowIf("@_manualReference == true")][SerializeField]
        protected Button _myButton;

        [ShowIf("@_manualReference == true")][SerializeField]
        protected Image _myImage;

        [ShowIf("@_manualReference == true")][SerializeField]
        protected List<Image> _myOtherImages = new List<Image>();

        [ShowIf("@_manualReference == true")][SerializeField]
        protected List<TextMeshProUGUI> _myOtherTMPs = new List<TextMeshProUGUI>();

        [SerializeField][UsedImplicitly] 
        private bool _manualReference;

        private void OnValidate()
        {
            if(_manualReference) return;

            if (Application.isPlaying) return;

            if (! _myButton)
            {
                if (TryGetComponent(out Button myBut))
                {
                    _myButton = myBut;
                }
                else
                {
                    Debug.LogError("UIButtonTMP needs a button on its GameObject");
                }
            }

            if (! _myImage)
            {
                if (TryGetComponent(out Image myImg))
                {
                    _myImage = myImg;
                }
                else
                {
                    Debug.LogError("UIButtonTMP needs a Image on its GameObject");
                }
            }
        }

        [Button(Name = "SetActive(bool isActive)", Style = ButtonStyle.Box, Expanded = true)]
        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);
            
            _myButton.enabled = isActive;

            if (_myImage) _myImage.enabled = isActive;
            _myOtherImages.DoToAll(oI => oI.enabled = isActive);
            _myOtherTMPs.DoToAll(oTmp => oTmp.enabled = isActive);
        }

        protected override void RegisterEvents()
        {
            _myButton.onClick.AddListener(OnClick);
        }

        protected abstract void OnClick();

        protected override void UnRegisterEvents()
        {
            _myButton.onClick.RemoveListener(OnClick);
        }
    }
}