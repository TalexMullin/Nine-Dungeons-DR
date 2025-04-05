using UnityEngine;

public class HealthImpacterEnemy : MonoBehaviour
{
    //public vars that can be adjusted in settings.
    public bool enterOrNot = true; // determins if ontriggerenter or ontriggerstay runs.
    public int healthChangeAmount = -5;
    public float knockbackSpeed = 500;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PlayerController.gamePaused)
        {
            if (enterOrNot)
            {
                EnemyHealthController controller = collision.GetComponent<EnemyHealthController>();
                //EnemyController toKnockback = collision.GetComponent<EnemyController>();

                if (controller != null)
                {
                    controller.ChangeHealth(healthChangeAmount);
                    PlayerController.ChangeShieldHealth(PlayerController.shieldHealthGainOnSwordAttack);
                }

                // apply enemy knockback
                /*Vector2 direction = (transform.position - toKnockback.transform.position).normalized;
                toKnockback.GetComponent<Rigidbody2D>().AddForce(direction * knockbackSpeed, ForceMode2D.Impulse);*/
            }
        }
    }



    /*
     *
     *
     *
     *
     *
     */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!PlayerController.gamePaused)
        {
            if (!enterOrNot)
            {
                EnemyHealthController controller = collision.GetComponent<EnemyHealthController>();

                if (controller != null)
                {
                    controller.ChangeHealth(healthChangeAmount);
                    PlayerController.ChangeShieldHealth(PlayerController.shieldHealthGainOnSwordAttack);
                }
            }
        }
    }
}
