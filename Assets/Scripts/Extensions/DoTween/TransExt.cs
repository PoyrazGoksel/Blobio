using DG.Tweening;
using UnityEngine;

namespace Extensions.DoTween
{
    public static class TransExt
    {
        private const float ScaleInOutTimerDiv = 2f;
        private const int TweenInfiniteLoop = -1;

        public static Sequence DoScaleInOut(this Transform trans, float size, float time)
        {
            Sequence scaleSeq = DOTween.Sequence();
            scaleSeq.Append(trans.DOScale(Vector3.one * size, time / ScaleInOutTimerDiv));
            scaleSeq.Append(trans.DOScale(Vector3.one, time / ScaleInOutTimerDiv));
            return scaleSeq;
        }
        
        public static Sequence DoScaleInOut(this Transform trans, float size, float inTime, float outTime)
        {
            Sequence scaleSeq = DOTween.Sequence();
            scaleSeq.Append(trans.DOScale(Vector3.one * size, inTime / ScaleInOutTimerDiv));
            scaleSeq.Append(trans.DOScale(Vector3.one, outTime / ScaleInOutTimerDiv));
            return scaleSeq;
        }
        
        public static Sequence DoScaleInOut(this Transform trans, Vector3 size, float time)
        {
            Vector3 initScale = trans.lossyScale;
            
            Sequence scaleSeq = DOTween.Sequence();
            scaleSeq.Append(trans.DOScale(size, time / ScaleInOutTimerDiv));
            scaleSeq.Append(trans.DOScale(initScale, time / ScaleInOutTimerDiv));
            return scaleSeq;
        }
        
        public static Tween DoYoYo(this Transform trans, float size, float time)
        {
            Tween yoYoTween = trans.DOScale(Vector3.one * size, time);
            yoYoTween.SetLoops(TweenInfiniteLoop, LoopType.Yoyo);
            return yoYoTween;
        }
        
        public static Tween DoYoYo(this Transform trans, Vector3 size, float time)
        {
            Tween yoYoTween = trans.DOScale(size, time);
            yoYoTween.SetLoops(TweenInfiniteLoop, LoopType.Yoyo);
            return yoYoTween;
        }

    }
}