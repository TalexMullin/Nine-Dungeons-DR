using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    // health
    public int maxHealth = 50;
    public int currentHealth;

    // damage cooldown
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerController.gamePaused)
        {
            //timer for determining invincibility frames;
            if (isInvincible)
            {
                damageCooldown -= Time.deltaTime;
                if (damageCooldown < 0)
                {
                    isInvincible = false;
                }
            }
        }
    }

    // a function that changes health and rounds if necessary
    /*
     *
     *
     *
     *
     *
     */
    public void ChangeHealth(int amount)
    {
        // determine when to activate damage cooldown.
        if (amount < 0)
        {
            // kicks out if the enemy is already invincible.
            // Otherwise, invinciblilty is turned on and enemy takes damage.
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log("Health is" + currentHealth + "/" + maxHealth);
    }
}
