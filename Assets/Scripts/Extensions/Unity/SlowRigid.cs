using System;
using UnityEngine;

namespace Extensions.Unity
{
	public class SlowRigid
	{
		private readonly float _timeSlowFactor;
		private readonly float _timeSlowTimeOut;
		private readonly RoutineHelper _updateRoutine;
		private readonly Rigidbody _myRigidbody;
		private readonly Animator _myAnimator;

		private float _timeSlowTimer;
		private bool _shouldHaveGravity = true;
		private Vector3 _beforeTimeSlowRotation;
		private Vector3 _beforeTimeSlowVelocity;
	
		public SlowRigid(MonoBehaviour updatingMono, Rigidbody myRigidbody, float timeSlowFactor, float timeSlowTimeOut)
		{
			_myRigidbody = myRigidbody;
			_timeSlowFactor = timeSlowFactor;
			_timeSlowTimeOut = timeSlowTimeOut;
		
			_updateRoutine = new RoutineHelper
			(
				updatingMono,
				new WaitForFixedUpdate(),
				UpdateRigid,
				() => true
			);

			SetRigidSettings();
		
			_updateRoutine.StartCoroutine();
		}

		public SlowRigid(MonoBehaviour updatingMono, Rigidbody myRigidbody, Animator myAnimator, float timeSlowFactor, float timeSlowTimeOut)
		{
			_myRigidbody = myRigidbody;
			_myAnimator = myAnimator;
			_timeSlowFactor = timeSlowFactor;
			_timeSlowTimeOut = timeSlowTimeOut;

			_updateRoutine = new RoutineHelper
			(
				updatingMono,
				new WaitForFixedUpdate(),
				UpdateRigid,
				() => true
			);

			SetRigidSettings();
		}

		public void SetPaused(bool isPaused)
		{
			_updateRoutine.SetPaused(isPaused);
		}
		
		private void SetRigidSettings()
		{
			_shouldHaveGravity = _myRigidbody.useGravity;
			
			_myRigidbody.useGravity = false;
			
			_beforeTimeSlowVelocity = _myRigidbody.velocity;
			
			if (float.IsNaN(_beforeTimeSlowVelocity.x)) _beforeTimeSlowVelocity = Vector3.zero;

			_beforeTimeSlowRotation = _myRigidbody.angularVelocity;
			
			if (float.IsNaN(_beforeTimeSlowRotation.x)) _beforeTimeSlowRotation = Vector3.zero;
			
			_myRigidbody.velocity = _beforeTimeSlowVelocity * _timeSlowFactor;
			
			_myRigidbody.angularVelocity = _beforeTimeSlowRotation * _timeSlowFactor;
		}

		private void ResetTimeScale()
		{
			_updateRoutine.StopCoroutine();
		
			_myRigidbody.velocity = new Vector3
			(_beforeTimeSlowVelocity.x, _beforeTimeSlowVelocity.y, _beforeTimeSlowVelocity.z);

			_myRigidbody.angularVelocity = new Vector3
			(_beforeTimeSlowRotation.x, _beforeTimeSlowRotation.y, _beforeTimeSlowRotation.z);
		
			_myRigidbody.useGravity = _shouldHaveGravity;
		}

		private void UpdateRigid()
		{
			_timeSlowTimer += Time.deltaTime;

			if (_timeSlowTimer > _timeSlowTimeOut) ResetTimeScale();

			if (!Vector3IsEqual(_myRigidbody.velocity / _timeSlowFactor, _beforeTimeSlowVelocity))
			{
				_beforeTimeSlowVelocity +=
				(_myRigidbody.velocity / _timeSlowFactor - _beforeTimeSlowVelocity) * _timeSlowFactor;
			
				_myRigidbody.velocity = _beforeTimeSlowVelocity * _timeSlowFactor;
			}
		
			if (!Vector3IsEqual(_myRigidbody.angularVelocity / _timeSlowFactor, _beforeTimeSlowRotation))
			{
				_beforeTimeSlowRotation +=
				(_myRigidbody.angularVelocity / _timeSlowFactor - _beforeTimeSlowRotation) *
				(_timeSlowFactor * 0.5f);

				_myRigidbody.angularVelocity = _beforeTimeSlowRotation * _timeSlowFactor;
			}
		
			_myRigidbody.AddForce(Physics.gravity * (_timeSlowFactor * _myRigidbody.mass));
		
			if (_myAnimator) _myAnimator.speed = _timeSlowFactor * 40;
		}

		private static bool Vector3IsEqual(Vector3 firstVector, Vector3 secondVector)
		{
			return (firstVector - secondVector).sqrMagnitude <= 0.001f;
		}
	}
}