using System;
using System.Collections;
using UnityEngine;

namespace GameJam.Enemies
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private float speed;
        [SerializeField] private Transform playerTarget;
        [SerializeField] private float minDistanceUntilAttack;
        [SerializeField] private GameObject projectile;
        [SerializeField] private float timeBetweenShots;
        [SerializeField] private float nextShotTime;
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private float waitTimeAtPoint;
        [SerializeField] private float distanceTolerance;
        
        private int currentPatrolPointIndex;
        private bool hasWaitedAtPoint = false;


        [System.Serializable]
        public enum EnemyType
        {
            Following,
            Shooting,
            Patroling
        }

        #region Basic Unity Methods

        private void Update()
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
                Instantiate(projectile, transform.position, Quaternion.identity);
                nextShotTime = Time.time + timeBetweenShots;
            }
            if (Vector2.Distance(transform.position, playerTarget.position) < minDistanceUntilAttack)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, -speed * Time.deltaTime);
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

        
    }

}
