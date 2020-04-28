using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement {
    public class Mover : MonoBehaviour, IAction {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;
        NavMeshAgent navMeshAgent;
        Health health;

        private void Start() {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update() {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction) {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        private void UpdateAnimator() {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public void Cancel() {
            navMeshAgent.isStopped = true;
        }

        public void MoveTo(Vector3 destination, float speedFraction) {
            if (navMeshAgent.enabled) {
                navMeshAgent.isStopped = false;
                navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
                navMeshAgent.destination = destination;
            }
            
        }
    }
}
