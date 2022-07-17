using System;
using System.Collections;
using UnityEngine;

namespace GameJam.Enemies
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private float speed;
        [SerializeField] public Transform playerTarget;
        [SerializeField] private float minDistanceUntilAttack;
        [SerializeField] private GameObject projectile;
        [SerializeField] private float timeBetweenShots;
        [SerializeField] private float nextShotTime;
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private float waitTimeAtPoint;
        [SerializeField] private float distanceTolerance;
        [SerializeField] private Health _health;
        [SerializeField] private GameObject _bullet;
        
        [SerializeField] private float _collisionDistance=0.5f;
        [SerializeField] private LayerMask _collisionMask;
        
        private int currentPatrolPointIndex;
        private bool hasWaitedAtPoint = false;

        private bool _alive=true;

        [System.Serializable]
        public enum EnemyType
        {
            Following,
            Shooting,
            Patroling
        }

        #region Basic Unity Methods

        private void Awake()
        {
            _health.SubscribeToDeath(OnDeath);
        }

        private void OnDisable()
        {
            _health.UnSubscribeToDeath(OnDeath);
        }

        private void OnEnable()
        {
            _health.SubscribeToDeath(OnDeath);
        }

        private void Update()
        {
            if (_alive)
            {
                if (enemyType == EnemyType.Following)
                {
                    FollowPlayer();
                }

                if (enemyType == EnemyType.Shooting)
                {
                    RetreatAndShoot();
                }

                if (enemyType == EnemyType.Patroling)
                {
                    Patrol();
                }
            }
        }

        #endregion
       

        #region Main Methods

        private void Patrol()
        {
            if (Vector2.Distance(transform.position, patrolPoints[currentPatrolPointIndex].position) > distanceTolerance)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolPointIndex].position, speed * Time.deltaTime);
            }
            else if(!hasWaitedAtPoint)
            {
                hasWaitedAtPoint = true;
                StartCoroutine(WaitAtPoint());
            }
        }

        private IEnumerator WaitAtPoint()
        {
            yield return new WaitForSeconds(waitTimeAtPoint);
            if (currentPatrolPointIndex + 1 < patrolPoints.Length)
            {
                currentPatrolPointIndex++;
            }
            else
            {
                currentPatrolPointIndex = 0;
            }
            hasWaitedAtPoint = false;
        }

        private void RetreatAndShoot()
        {
            if (Time.time > nextShotTime)
            {
                GameObject bulletGameobject = Instantiate(_bullet);
                bulletGameobject.transform.position = transform.position;
                bulletGameobject.GetComponent<Bullet>().Direction = (playerTarget.position - transform.position).normalized;
                nextShotTime = Time.time + timeBetweenShots;
            }
            if (Vector2.Distance(transform.position, playerTarget.position) < minDistanceUntilAttack)
            {
                Vector2 _direction = (transform.position - playerTarget.position).normalized;
                //transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, -speed * Time.deltaTime);
                _direction = CollisionCheck(_direction);
                transform.Translate(_direction*(speed*Time.deltaTime));
            }
        }

        private void FollowPlayer()
        {
            if (Vector2.Distance(transform.position, playerTarget.position) > minDistanceUntilAttack)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, speed * Time.deltaTime);
            }
            else
            {
                //Attack player
            }
        }

        #endregion


        private void OnDeath()
        {
            _alive = false;
        }
        
        private Vector2 CollisionCheck(Vector2 move)
        {
            if (move.x>0 && Physics2D.Raycast(transform.position, Vector2.right, _collisionDistance, _collisionMask))
            {
                move.x = 0;
            }
            if (move.x<0 && Physics2D.Raycast(transform.position, Vector2.left, _collisionDistance, _collisionMask))
            {
                move.x = 0;
            }
            if (move.y>0 && Physics2D.Raycast(transform.position, Vector2.up, _collisionDistance, _collisionMask))
            {
                move.y = 0;
            }
            if (move.y<0 && Physics2D.Raycast(transform.position, Vector2.down, _collisionDistance, _collisionMask))
            {
                move.y = 0;
            }

            return move;
        }
        
    }
    
   

}
