using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; 
    private int currentHealth; 

    private void Start()
    {
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
        gameObject.GetComponent<Animator>().SetBool("Death", true);
    }
}
