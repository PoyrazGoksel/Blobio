using DG.Tweening;
using Events;
using UnityEngine;
using Utils;

namespace UI.MainMenu
{
    public class NewGameBTN : UIBTN
    {
        [SerializeField] private Transform _transform;

        private Sequence _clickSizeSeq;

        protected override void OnDisable()
        {
            base.OnDisable();

            if(_clickSizeSeq.IsActive()) _clickSizeSeq.Kill();
        }

        protected override void OnClick()
        {
            _transform.localScale = Vector3.one;

            _clickSizeSeq = DOTween.Sequence(); 
            
            Tween sizeIncTwn = _transform.DOScale(Vector3.one * 1.1f, 0.5f);
            sizeIncTwn.SetEase(Ease.OutElastic);
            
            Tween sizeDcrTwn = _transform.DOScale(Vector3.one, 0.5f);
            sizeDcrTwn.SetEase(Ease.OutElastic);


            _clickSizeSeq.Append(sizeIncTwn);
            _clickSizeSeq.Append(sizeDcrTwn);

            Tween secCounterTween = DOVirtual.Float
            (
                0,
                5f,
                5f,
                delegate(float e)
                {
                    Debug.LogWarning($"{e} = e");
                }
            );

            _clickSizeSeq.Append(secCounterTween);
            
            _clickSizeSeq.onComplete += delegate
            {
                MainMenuEvents.NewGameBTN?.Invoke();
            };
        }

        // private void SizeDown()
        // {
        //     if(_clickSizeTween.IsActive()) _clickSizeTween.Kill(true);
        //     
        //     _transform.localScale = Vector3.one * 1.1f;
        
        // }
    }
}