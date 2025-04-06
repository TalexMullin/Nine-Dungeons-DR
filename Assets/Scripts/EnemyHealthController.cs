using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    // health
    public int maxHealth = 50;
    int currentHealth;

    // damage cooldown
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    Rigidbody2D rigidbody2d;

    // item drops
    GameObject itemDrop;
    public GameObject[] itemDropsList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        rigidbody2d = GetComponent<Rigidbody2D>();
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

        if (currentHealth <= 0)
        {
            // get random item drop
            int dropNumber = Random.Range(0, itemDropsList.Length);
            itemDrop = itemDropsList[dropNumber];
            Vector2 dropLocation = rigidbody2d.position;
            if (itemDrop != null)
            {
                itemDrop = Instantiate(itemDrop, dropLocation, Quaternion.identity) as GameObject;
            }
            Destroy(gameObject);
        }
        //Debug.Log("Health is" + currentHealth + "/" + maxHealth);
    }
}
