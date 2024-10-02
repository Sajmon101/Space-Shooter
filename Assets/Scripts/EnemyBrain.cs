using GDL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GDL
{
    public class EnemyBrain : MonoBehaviour
    {
        public Transform player;
        private EnemyReferences enemyReferences;

        private float pathUpdateDeadLine;

        private float shootingDistance;

        [SerializeField] float sightRange = 5f;

        [SerializeField] LayerMask whatIsPlayer;
        [SerializeField] LayerMask obstacleLayer;
        public bool isChasing = false;

        [Header("Patrolling")]
        public bool isPatrolling;
        public GameObject pointA;
        public GameObject pointB;
        private float patrolSpeed = 4;
        private bool whereGo;

        public bool IsAlive = true;

        private void Awake()
        {
            enemyReferences = GetComponent<EnemyReferences>();
            pointA.transform.parent = null;
            pointB.transform.parent = null;
        }

        void Start()
        {
            shootingDistance = enemyReferences.navMeshAgent.stoppingDistance;    
            enemyReferences.navMeshAgent.stoppingDistance = 0;    
        }

        void Update()
        {
            if (!IsAlive)
            {
                return;
            }

            if (isPatrolling)
            {
                patrolSpeed = (enemyReferences.navMeshAgent.desiredVelocity.sqrMagnitude/15);
                if (whereGo)
                {
                    LookAtTarget(pointA.transform);
                    UpdatePath(pointA.transform);
                    enemyReferences.animator.SetFloat("speed", patrolSpeed);
                    if (Vector3.Distance(pointA.transform.position, transform.position)<1) whereGo = false;
                }

                else
                {
                    LookAtTarget(pointB.transform);
                    UpdatePath(pointB.transform);
                    enemyReferences.animator.SetFloat("speed", patrolSpeed);
                    if (Vector3.Distance(pointB.transform.position, transform.position)<1) whereGo = true;
                }
            }

            if(isChasing || CheckPlayerPresence())
            {
                if(!isChasing) //things to do once
                {
                    if(pointA)
                        pointA.GetComponent<DestroyMyself>().Destroy();
                    if(pointB)
                        pointB.GetComponent<DestroyMyself>().Destroy();
                    enemyReferences.navMeshAgent.stoppingDistance = shootingDistance;
                    isPatrolling = false;
                    isChasing = true;
                }

                if (!player) return;

                bool inRange = Vector3.Distance(transform.position, player.position) <= shootingDistance;

                //if (inRange && CanSeePlayer()) LookAtTarget(player);
                //else UpdatePath(player);

                if (!inRange)
                {
                    enemyReferences.navMeshAgent.stoppingDistance = shootingDistance;
                    UpdatePath(player);
                    enemyReferences.animator.SetFloat("speed", enemyReferences.navMeshAgent.desiredVelocity.sqrMagnitude);
                    enemyReferences.animator.SetBool("shooting", inRange);
                }

                else
                {
                    LookAtTarget(player);
                    enemyReferences.animator.SetFloat("speed", enemyReferences.navMeshAgent.desiredVelocity.sqrMagnitude);
                    if (CanSeePlayer())
                    {
                        enemyReferences.navMeshAgent.stoppingDistance = shootingDistance;
                        enemyReferences.animator.SetFloat("speed", enemyReferences.navMeshAgent.desiredVelocity.sqrMagnitude);
                        enemyReferences.animator.SetBool("shooting", inRange);
                    }
                    else
                    {
                        enemyReferences.navMeshAgent.stoppingDistance -=1;
                        enemyReferences.animator.SetFloat("speed", enemyReferences.navMeshAgent.desiredVelocity.sqrMagnitude);
                        UpdatePath(player);
                    }
                }
            }

        }

        private void LookAtTarget(Transform target)
        {
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
        }

        private void UpdatePath(Transform target)
        {
            if (Time.time >= pathUpdateDeadLine)
            {
                pathUpdateDeadLine = Time.time + enemyReferences.pathUpdateDelay;
                enemyReferences.navMeshAgent.SetDestination(target.position);
            }
        }

        private bool CheckPlayerPresence()
        {
            if (Physics.CheckSphere(transform.position, sightRange, whatIsPlayer))
            {
                Vector3 direction = player.position - transform.position;

                Ray ray = new Ray(transform.position, direction);

                float maxDistance = direction.magnitude - 0.5f;

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, maxDistance, obstacleLayer)) return false;
                else return true;
            }

            return false;
        }

        bool CanSeePlayer()
        {
            Vector3 offset = new Vector3(0, 1.5f, 0);
            Vector3 directionToPlayer = player.transform.position  - transform.position;
            
            RaycastHit hit;
            if (Physics.Raycast(transform.position + offset, directionToPlayer.normalized, out hit, directionToPlayer.magnitude, obstacleLayer)) return false;
            else return true;
        }

        public void StopRobot()
        {
            isChasing = false;
            enemyReferences.animator.SetFloat("speed", 0f);
            enemyReferences.animator.SetBool("shooting", false);
            //enemyReferences.navMeshAgent.stoppingDistance = 100000f;
            //enemyReferences.navMeshAgent.stoppingDistance = shootingDistance;
        }

    }
}
