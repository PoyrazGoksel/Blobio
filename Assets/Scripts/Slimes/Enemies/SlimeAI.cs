using System;
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
            StopAllBehaviourRoutines();
            StartWanderRoutine();
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            GUISkin guiSkin = GUI.skin;
            Vector3 currPos = transform.position;
            GizmosUtils.DrawText(guiSkin, _aiState.ToString(), currPos, GetStateColor(),
                (int)(6 / UnityEditor.HandleUtility.GetHandleSize(currPos)));
        }
#endif

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
            StopFleeRoutine();
            StopWanderRoutine();
            StopBaitChaseRoutine();
            StopEnemyChaseRoutine();
        }
        
        private void StartFleeRoutine(Slime fleeTarg)
        {
            StopAllBehaviourRoutines();
            
            _currFleeTarg = fleeTarg;
            _aiState = AIState.Fleeing;
            _fleeRoutine.StartCoroutine();
        }
        
        private void FleeRoutine()
        {
            if(_currFleeTarg == null)
            {
                StopAllBehaviourRoutines();
                StartWanderRoutine();
                
                return;
            }
            
            if(IsFleeTargAttacking(_currFleeTarg))
            {
                StopAllBehaviourRoutines();
                _aiPath.destination = (Trans.position - _currFleeTarg.Trans.position) + Trans.position;
                _aiPath.SearchPath();
            }
            
            //TODO: Continue checking enemies for attack behaviour;
            else
            {
                StartWanderRoutine();
            }
        }

        private bool IsFleeTargAttacking(Slime fleeTarg)
        {
            return IsPositionBetweenTwoAngles(Trans, fleeTarg.Trans.position, 20f);
        }

        private bool IsPositionBetweenTwoAngles
        (
            TransformEncapsulated myTrans,
            Vector3 position,
            float viewAngle
        )
        {
            Vector3 forward = myTrans.forward;

            // Calculate direction to position
            Vector3 dirToPos = position - myTrans.position;

            // Get the angle
            float angleToPos = Vector3.Angle(forward, dirToPos).ToEul();

            Debug.DrawLine(myTrans.position, position, Color.red,Time.deltaTime);
            
            return angleToPos.IsEulBtwn(-1f * viewAngle, viewAngle);
        }

        private void StopFleeRoutine()
        {
            _currFleeTarg = null;
            _fleeRoutine.StopCoroutine();
        }

        private void StartEnemyChaseRoutine(Slime currChaseTarg)
        {
            StopAllBehaviourRoutines();
            
            _currChaseTarg = currChaseTarg;
            _aiState = AIState.ChasingEnemy;
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
            
            _aiState = AIState.ChasingBait;
            _baitChaseRoutine.StartCoroutine();
        }

        private void StopBaitChaseRoutine() {_baitChaseRoutine.StopCoroutine();}

        private void StartWanderRoutine()
        {
            StopAllBehaviourRoutines();
            
            _isWandering = false;
            _aiState = AIState.Wandering;
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
            else if(CanBeEaten(enemySlime) && IsFleeTargAttacking(enemySlime))
            {
                StartFleeRoutine(enemySlime);
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
            SlimeEvents.EnemyDetected += OnEnemyDetected;
            SlimeEvents.SlimeCollision += OnSlimeCollision;
            SlimeEvents.BaitDetection += OnBaitDetection;
        }

        private void OnEnemyDetected(Slime enemySlime)
        {
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

        protected override void UnRegisterEvents()
        {
            base.UnRegisterEvents();
            SlimeEvents.BaitDetection -= OnBaitCollision;
            SlimeEvents.EnemyDetected -= OnEnemyDetected;
            SlimeEvents.SlimeCollision -= OnSlimeCollision;
            SlimeEvents.BaitCollision -= OnBaitDetection;
        }
    }
}