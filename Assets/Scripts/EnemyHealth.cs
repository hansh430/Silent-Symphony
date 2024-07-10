using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Collider interactionCollider;
    private Animator enemyAnimator;
    private int currentHealth; 

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        currentHealth = maxHealth; 
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        interactionCollider.enabled = false;
        enemyAnimator.SetBool("Death", true);
        Invoke(nameof(ReSpawnEnemy), 10f);
    }
    private void ReSpawnEnemy()
    {
        enemyAnimator.SetBool("Death", false);
        interactionCollider.enabled = true;
        currentHealth = maxHealth;
    }
}
