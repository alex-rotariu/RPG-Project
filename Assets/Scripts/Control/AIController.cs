using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control {

    public class AIController : MonoBehaviour {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwelltime = 2f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        Fighter fighter;
        Mover mover;
        GameObject player;
        Health health;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWayPoint = Mathf.Infinity;
        int currentWayPointIndex = 0;

        private void Start() {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            guardPosition = transform.position;
        }

        private void Update() {

            if (health.IsDead())
                return;
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player)) {
                AttackBehaviour();
            } else if (timeSinceLastSawPlayer < suspicionTime) {
                SuspicionBehaviour();
            } else {
                PatrolBehaviour();
            }
            UpdateTimers();
        }

        private void UpdateTimers() {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWayPoint += Time.deltaTime;
        }

        private void SuspicionBehaviour() {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PatrolBehaviour() {
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null) {
                if (AtWaypoint()) {
                    timeSinceArrivedAtWayPoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if(timeSinceArrivedAtWayPoint > waypointDwelltime)
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }

        private Vector3 GetCurrentWaypoint() {
            return patrolPath.GetWayPoint(currentWayPointIndex);
        }

        private void CycleWaypoint() {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        private bool AtWaypoint() {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWayPoint < waypointTolerance;
        }

        private void AttackBehaviour() {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer() {
            
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
