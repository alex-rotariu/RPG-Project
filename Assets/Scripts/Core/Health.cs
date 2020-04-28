using UnityEngine;
using UnityEditor;


namespace RPG.Core {
    public class Health : MonoBehaviour {
        [SerializeField] float healthPoints = 100f;

        float initialHealth;
        bool isDead = false;

        private void Start() {
            initialHealth = healthPoints;
        }

        public bool IsDead() {
            return isDead;
        }

        public void TakeDamage(float damage) {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if(healthPoints == 0) {
                Die();
            }
        }

        private void Die() {
            if (isDead) {
                return;
            }
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public float[] getHealth() {
            float[] result = { healthPoints, initialHealth};
            return result;
        }
    }
}