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
        
        private void Start() {StartWanderRoutine();}

        protected override void IncreaseSize(int size)
        {
            base.IncreaseSize(size);
            //_baitCollisionDetector.radius += initBaitDetectorSize + NewSizeOffset;
        }

        private void BaitDetected(Bait bait)
        {
            _currBait = bait;

            StopAllBehaviourRoutines();
            StartBaitChaseRoutine();
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
            _currFleeTarg = fleeTarg;
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
                _aiPath.canMove = true;
                _aiPath.destination = (_currFleeTarg.Trans.position - Trans.position) + Trans.position;
            }
            else
            {
                StopAllBehaviourRoutines();
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
            float angleToPos = Vector3.Angle(forward, dirToPos);

            // Check if the angle to the position falls between the two provided angles
            // Assuming that angle1 is less than or equal to angle2
            return -1f * viewAngle <= angleToPos && angleToPos <= viewAngle;
        }
        
        private void StopFleeRoutine()
        {
            _currFleeTarg = null;
            _fleeRoutine.StopCoroutine();
        }

        private void StartEnemyChaseRoutine(Slime currChaseTarg)
        {
            _currChaseTarg = currChaseTarg;
            _enemyChaseRoutine.StartCoroutine();
        }
        
        private void EnemyChaseRoutineUpdate()
        {
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

        private void StartBaitChaseRoutine() {_baitChaseRoutine.StartCoroutine();}

        private void StopBaitChaseRoutine() {_baitChaseRoutine.StopCoroutine();}

        private void StartWanderRoutine()
        {
            _isWandering = false;
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
                StopAllBehaviourRoutines();
                StartEnemyChaseRoutine(enemySlime);
            }
            else if(CanBeEaten(enemySlime) && IsFleeTargAttacking(enemySlime))
            {
                StopAllBehaviourRoutines();
                StartFleeRoutine(enemySlime);
            }
            else
            {
                
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

        protected override void OnBaitCollision(Bait colBait)
        {
            base.OnBaitCollision(colBait);
            
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