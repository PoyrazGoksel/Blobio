using System;
using System.Collections.Generic;
using System.Linq;
using Datas;
using Extensions.Unity;
using Pathfinding;
using UnityEngine;
using WorldObjects;
using Random = UnityEngine.Random;

namespace Slimes.Enemies
{
    public class SlimeAI : Slime
    {
        private const float DestReachedDist = 1.2f;
        private const float GridMax = 49f;
        private const float ViewAngle = 20f;
        [SerializeField] private AIPath _aiPath;
        [SerializeField] private SphereCollider _baitCollisionDetector;
        [SerializeField] private AIState _aiState;
        private RoutineHelper _baitChaseRoutine;
        private Bait _currBait;
        private Vector3 _currSeekPos;
        private RoutineHelper _enemyChaseRoutine;
        private RoutineHelper _fleeRoutine;
        private RoutineHelper _wanderRoutine;
        private bool _isWandering;
        private Slime _currFleeTarg;
        private Slime _currChaseTarg;
        private List<Slime> _currEnemies = new();
        private bool _isFleeing;
        private Ray _rightAngleRay;
        private Ray _leftAngleRay;

        protected override void Awake()
        {
            base.Awake();
            _wanderRoutine = new RoutineHelper(this, null, SeekRoutineUpdate);

            _baitChaseRoutine = new RoutineHelper
            (this, new WaitForFixedUpdate(), BaitChaseRoutineUpdate);

            _enemyChaseRoutine = new RoutineHelper(this, null, EnemyChaseRoutineUpdate);
            _fleeRoutine = new RoutineHelper(this, null, FleeRoutine);
        }

        private void Start()
        {
            _fleeRoutine.StartCoroutine();
            StopAllBehaviourRoutines();
            StartWanderRoutine();
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 forward = Trans.forward;
            
            float rightAngle = ViewAngle;
            float leftAngle = -1f * ViewAngle;

            Quaternion leftAngleLineRot = Quaternion.Euler(0f, leftAngle, 0f);
            Quaternion rightAngleLineRot = Quaternion.Euler(0f, rightAngle, 0f);

            Vector3 leftAnglePos = leftAngleLineRot * forward;
            Vector3 rightAnglePos = rightAngleLineRot * forward;
            
            _leftAngleRay = new (transform.position, leftAnglePos);
            _rightAngleRay = new (transform.position, rightAnglePos);
            
            GUISkin guiSkin = GUI.skin;
            Vector3 currPos = transform.position;
            GizmosUtils.DrawText(guiSkin, _aiState.ToString(), currPos, GetStateColor(),
                (int)(6 / UnityEditor.HandleUtility.GetHandleSize(currPos)));
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(Trans.position, Trans.position + (Trans.forward * 10f));
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Trans.position, Trans.position + _rightAngleRay.direction * 10f);
            Gizmos.DrawLine(Trans.position, Trans.position + _leftAngleRay.direction * 10f);
        }
#endif
        private void SetAIState(AIState aiState)
        {
            if(_isFleeing) return;
            
            _aiState = aiState;
        }

        private void BaitDetected(Bait bait)
        {
            if(_currFleeTarg == null && _currChaseTarg == null)
            {
                _currBait = bait;

                StartBaitChaseRoutine();   
            }
        }

        private void StopAllBehaviourRoutines()
        {
            StopWanderRoutine();
            StopBaitChaseRoutine();
            StopEnemyChaseRoutine();
        }
        
        private void FleeRoutine()
        {
            if(_currEnemies.Count == 0) return;

            _currEnemies = _currEnemies.Where(e => e != null).ToList();
            
            _currEnemies = _currEnemies.OrderBy(e => Vector3.Distance(Trans.position, e.Trans.position)).ToList();
            
            foreach(Slime slime in _currEnemies)
            {
                if(CanBeEaten(slime) && IsFleeTargAttacking(slime))
                {
                    SetAIState(AIState.Fleeing);
                    _currFleeTarg = slime;
                    Vector3 fleeDirection = (Trans.position - _currFleeTarg.Trans.position).normalized;
                    _aiPath.destination = fleeDirection + Trans.position;
                    _aiPath.SearchPath();
                    _isFleeing = true;
                    return;
                }
            }

            if(_isFleeing) // TODO: when these were not added it was working
            {
                _isFleeing = false;
                StopAllBehaviourRoutines(); // TODO: when these were not added it was working 
                StartWanderRoutine();// TODO: when these were not added it was working
            }
        }

        private bool IsFleeTargAttacking(Slime fleeTarg)
        {
            return IsPositionBetweenTwoAngles(fleeTarg.Trans, Trans.position, ViewAngle);
        }

        private bool IsPositionBetweenTwoAngles
        (
            TransformEncapsulated myTrans,
            Vector3 otherPos,
            float viewAngle
        )
        {
            Vector3 forward = myTrans.forward;
            
            Vector3 dirToPos = otherPos - myTrans.position;
            
            Debug.DrawLine(Trans.position, otherPos, Color.red, Time.deltaTime);
            float angleToPos = Vector3.Angle(forward, dirToPos).ToEul();

            float rightAngle = viewAngle;
            float leftAngle = -1f * viewAngle;

            return angleToPos.IsEulBtwn(leftAngle, rightAngle);
        }

        private void StartEnemyChaseRoutine(Slime currChaseTarg)
        {
            StopAllBehaviourRoutines();
            
            _currChaseTarg = currChaseTarg;
            SetAIState(AIState.ChasingEnemy);
            _enemyChaseRoutine.StartCoroutine();
        }

        private Color GetStateColor()
        {
            return _aiState switch
            {
                AIState.Null => Color.gray,
                AIState.ChasingBait => Color.green,
                AIState.ChasingEnemy => Color.red,
                AIState.Fleeing => Color.blue,
                AIState.Wandering => Color.white,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private void EnemyChaseRoutineUpdate()
        {
            //TODO: While chasing if cant eat stop chasing (if enemy has eaten any baits while chasing)
            if(CanEat(_currChaseTarg) == false)
            {
                StopAllBehaviourRoutines();
                StartWanderRoutine();

                return;
            }
            
            if(_currChaseTarg == null)
            {
                StopAllBehaviourRoutines();
                StartWanderRoutine();

                return;
            }
            
            _aiPath.destination = _currChaseTarg.Trans.position;
            _aiPath.SearchPath();
        }

        private void TryEatEnemy(Slime otherSlime)
        {
            if(_isFleeing) return;
            
            if(otherSlime == this) return;
            
            if(CanEat(otherSlime))
            {
                otherSlime.Eaten();
                
                StopAllBehaviourRoutines();
                StartWanderRoutine();   
            }
        }
        
        private void StopEnemyChaseRoutine()
        {
            _currChaseTarg = null;
            _enemyChaseRoutine.StopCoroutine();
        }

        private void BaitChaseRoutineUpdate()
        {
            if(_currBait == null)
            {
                StopAllBehaviourRoutines();
                StartWanderRoutine();

                return;
            }

            _aiPath.destination = _currBait.TransformEncapsulated.position;
            _aiPath.SearchPath();
        }

        private void StartBaitChaseRoutine()
        {
            StopAllBehaviourRoutines();
            
            SetAIState(AIState.ChasingBait);
            _baitChaseRoutine.StartCoroutine();
        }

        private void StopBaitChaseRoutine() {_baitChaseRoutine.StopCoroutine();}

        private void StartWanderRoutine()
        {
            StopAllBehaviourRoutines();
            
            _isWandering = false;
            SetAIState(AIState.Wandering);
            _wanderRoutine.StartCoroutine();
        }

        private void SeekRoutineUpdate()
        {
            if(_isWandering == false
                || Vector3.Distance(transform.position, _currSeekPos) < DestReachedDist)
                SetRandWanderPos();
        }

        private void SetRandWanderPos()
        {
            _isWandering = true;
            _currSeekPos = GetRandWanderPos();

            _aiPath.destination = _currSeekPos;
            _aiPath.SearchPath();
        }

        private Vector3 GetRandWanderPos()
        {
            Vector3 returnPos = transform.position;
            float sphereDetectorRadius = _baitCollisionDetector.radius;
            float sphereDetectorRadiusNegative = -1f * sphereDetectorRadius;

            returnPos.x -= Random.Range(sphereDetectorRadiusNegative, sphereDetectorRadius);
            returnPos.z -= Random.Range(sphereDetectorRadiusNegative, sphereDetectorRadius);

            returnPos.x = Mathf.Clamp(returnPos.x, -GridMax, GridMax);
            returnPos.z = Mathf.Clamp(returnPos.z, -GridMax, GridMax);

            return returnPos;
        }

        private void StopWanderRoutine()
        {
            _wanderRoutine.StopCoroutine();
            _isWandering = false;
        }

        protected override void Pause() {_aiPath.isStopped = true;}

        protected override void UnPause() {_aiPath.isStopped = false;}

        private void DecideCombatTactics(Slime enemySlime)
        {
            if(CanEat(enemySlime))
            {
                StartEnemyChaseRoutine(enemySlime);
            }
        }

        private bool CanEat(Slime enemySlime)
        {
            if(Score > enemySlime.Score + EatScoreDiff)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanBeEaten(Slime enemySlime)
        {
            if(Score < enemySlime.Score - EatScoreDiff)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            SlimeEvents.BaitCollision += OnBaitCollision;
            SlimeEvents.SlimeCollision += OnSlimeCollision;
            SlimeEvents.BaitDetection += OnBaitDetection;
            SlimeEvents.EnemyDetected += OnEnemyDetected;
            SlimeEvents.EnemyLost += OnEnemyLost;
        }

        private void OnEnemyDetected(Slime enemySlime)
        {
            _currEnemies.Add(enemySlime);
            DecideCombatTactics(enemySlime);
        }

        private void OnSlimeCollision(Slime otherSlime)
        {
            TryEatEnemy(otherSlime);
        }

        private void OnBaitDetection(Bait bait)
        {
            BaitDetected(bait);
        }

        private void OnEnemyLost(Slime lostEnemy)
        {
            _currEnemies.Remove(lostEnemy);
        }

        protected override void UnRegisterEvents()
        {
            base.UnRegisterEvents();
            SlimeEvents.BaitDetection -= OnBaitCollision;
            SlimeEvents.SlimeCollision -= OnSlimeCollision;
            SlimeEvents.BaitCollision -= OnBaitDetection;
            SlimeEvents.EnemyDetected -= OnEnemyDetected;
            SlimeEvents.EnemyLost -= OnEnemyLost;
        }
    }
}