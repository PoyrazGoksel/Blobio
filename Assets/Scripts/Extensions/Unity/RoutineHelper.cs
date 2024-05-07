using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Extensions.Unity
{
    public class RoutineHelper
    {
        private readonly Action _invokeFunc;
        private readonly YieldInstruction _wait;
        private Coroutine _myRoutine;
        private readonly MonoBehaviour _myInvokingMono;
        private readonly Func<bool> _whileCond;
        private bool _isStopped;
        public bool IsStopped => _isStopped;
        public event UnityAction OnEnded;

        private bool _isPaused;

        public RoutineHelper
        (MonoBehaviour invokingBehaviour, YieldInstruction wait, Action invokeFunc, Func<bool> whileCondition = null)
        {
            whileCondition ??= () => true;

            _myInvokingMono = invokingBehaviour;
            _wait = wait;
            _invokeFunc = invokeFunc;
            _whileCond = whileCondition;
        }

        public void SetPaused(bool isPaused)
        {
            _isPaused = isPaused;
        }

        public void StartCoroutine()
        {
            _isStopped = false;
            if (_myRoutine == null)
            {
                _myRoutine = _myInvokingMono.StartCoroutine(InvokingRoutine());
            }
        }

        public void StopCoroutine()
        {
            _isStopped = true;
            if (_myRoutine != null)
            {
                _myInvokingMono.StopCoroutine(_myRoutine);
                _myRoutine = null;
                OnEnded?.Invoke();
            }
        }

        private IEnumerator InvokingRoutine()
        {
            while (_whileCond() && _isStopped == false)
            {
                if (_isPaused == false)
                {
                    _invokeFunc?.Invoke();
                }
                yield return _wait;
            }
            
            OnEnded?.Invoke();
        }
    }
    
    public class RoutineHelper<T>
    {
        private readonly Action _invokeFunc;
        private readonly YieldInstruction _wait;
        private Coroutine _myRoutine;
        private readonly MonoBehaviour _myInvokingMono;
        private readonly Func<bool> _whileCond;
        private readonly T _updateType;
        private readonly Action<T> _updateCallback;

        public event UnityAction OnEnded;
        // int test = 1;
        //     
        // RoutineHelper<int> exampleRoutine = new RoutineHelper<int>
        // (
        //     this,
        //     new WaitForSeconds(1f),
        //     delegate { Debug.LogWarning("test" + test); },
        //     delegate { return test < 10; },
        //     test,
        //     delegate(int i) { test += 1; i = test; }
        // );
        //     
        // exampleRoutine.StartCoroutine();
        public RoutineHelper
        (MonoBehaviour invokingBehaviour, YieldInstruction wait, Action invokeFunc, Func<bool> whileCondition, T updateType, Action<T> updateCallback
        )
        {
            _myInvokingMono = invokingBehaviour;
            _wait = wait;
            _invokeFunc = invokeFunc;
            _whileCond = whileCondition;
            _updateType = updateType;
            _updateCallback = updateCallback;
        }

        public void StartCoroutine()
        {
            if (_myRoutine == null)
            {
                _myRoutine = _myInvokingMono.StartCoroutine(InvokingRoutine());
            }
        }

        public void StopCoroutine()
        {
            if (_myRoutine != null)
            {
                _myInvokingMono.StopCoroutine(_myRoutine);
                _myRoutine = null;
                OnEnded?.Invoke();
            }
        }

        private IEnumerator InvokingRoutine()
        {
            while (_whileCond())
            {
                _updateCallback?.Invoke(_updateType);
                _invokeFunc?.Invoke();
                yield return _wait;
            }
            OnEnded?.Invoke();
        }
    }
}