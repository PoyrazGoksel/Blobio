using System.Collections.Generic;
using Extensions.System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Extensions.Unity.MonoHelper
{
    public class UIPanel : UIBase
    {
        [ShowIf("@_manualReference == true")][SerializeField]
        protected List<UIButtonTMP> _uIButs = new();
        [ShowIf("@_manualReference == true")][SerializeField]
        protected List<UIButtonIMG> _uIButIMGs = new();
        [ShowIf("@_manualReference == true")][SerializeField]
        protected List<UITMP> _uITMPs = new();

        [ShowIf("@_manualReference == true")][SerializeField]
        protected List<UIIMG> _uIIMGs = new();

        [ShowIf("@_manualReference == true")][SerializeField]
        private List<Image> _myOtherIMGs = new();

        [ShowIf("@_manualReference == true")][SerializeField]
        private List<TextMeshProUGUI> _myOtherTMPs = new();

        [ShowIf("@_manualReference == true")][SerializeField]
        private List<RawImage> _myOtherRawIMGs = new();
        
        [SerializeField][UsedImplicitly]
        private bool _manualReference;

        private void OnValidate()
        {
            if (_manualReference) return;
            
            Clear();

            if (transform.TryGetComponent(out Image img))
            {
                _myOtherIMGs.Add(img);
            }

            if (transform.TryGetComponent(out TextMeshProUGUI tmp))
            {
                _myOtherTMPs.Add(tmp);
            }
            
            TryGetComponentInChildrenRecursive(transform);
        }

        private void TryGetComponentInChildrenRecursive(Transform trans)
        {
            for (int i = 0; i < trans.childCount; i ++)
            {
                Transform thisChild = trans.GetChild(i);

                if (thisChild.TryGetComponent(out UIBase uiBase))
                {
                    if (uiBase is UIButtonTMP uiBut)
                    {
                        _uIButs.Add(uiBut);
                    }
                    else if (uiBase is UIButtonIMG uiButIMG)
                    {
                        _uIButIMGs.Add(uiButIMG);
                    }
                    else if (uiBase is UITMP uitmp)
                    {
                        _uITMPs.Add(uitmp);
                    }
                    else if (uiBase is UIIMG uiimg)
                    {
                        _uIIMGs.Add(uiimg);
                    }
                }
                else
                {
                    if (thisChild.TryGetComponent(out Image img))
                    {
                        _myOtherIMGs.Add(img);
                    }
                    if (thisChild.TryGetComponent(out TextMeshProUGUI tmp))
                    {
                        _myOtherTMPs.Add(tmp);
                    }
                    if (thisChild.TryGetComponent(out RawImage rawImage))
                    {
                        _myOtherRawIMGs.Add(rawImage);
                    }
                    if (trans.GetChild(i).childCount > 0)
                    {
                        TryGetComponentInChildrenRecursive(trans.GetChild(i));
                    }
                }
            }
        }

        private void Clear()
        {
            _uIButs = new List<UIButtonTMP>();
            _uIButIMGs = new List<UIButtonIMG>();
            _uITMPs = new List<UITMP>();
            _uIIMGs = new List<UIIMG>();
            _myOtherIMGs = new List<Image>();
            _myOtherTMPs = new List<TextMeshProUGUI>();
        }

        [Button]
        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);
            
            SettingUIButsActive(isActive);
            SettingUITMPsActive(isActive);
            SettingUIImgActive(isActive);
            SettingUIButIMGsActive(isActive);
            _myOtherIMGs.DoToAll(oI => oI.enabled = isActive);
            _myOtherTMPs.DoToAll(oT => oT.enabled = isActive);
            _myOtherRawIMGs.DoToAll(oRI => oRI.enabled = isActive);
        }

        protected virtual void SettingUIImgActive(bool isActive) {_uIIMGs.DoToAll(uib => uib.SetActive(isActive));}

        protected virtual void SettingUITMPsActive(bool isActive) {_uITMPs.DoToAll(uib => uib.SetActive(isActive));}

        protected virtual void SettingUIButsActive(bool isActive) {_uIButs.DoToAll(uib => uib.SetActive(isActive));}

        protected virtual void SettingUIButIMGsActive(bool isActive) {_uIButIMGs.DoToAll(uib => uib.SetActive(isActive));}
        
        protected override void RegisterEvents()
        {
        }

        protected override void UnRegisterEvents()
        {
        }
    }
}