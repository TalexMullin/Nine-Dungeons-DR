using UnityEngine;

public class HealthImpacterEnemy : MonoBehaviour
{
    //public vars that can be adjusted in settings.
    public bool enterOrNot = true; // determins if ontriggerenter or ontriggerstay runs.
    public int healthChangeAmount = -5;
    public float knockbackSpeed = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PlayerController.gamePaused)
        {
            if (enterOrNot)
            {
                ImpactHealth(collision);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!PlayerController.gamePaused)
        {
            if (!enterOrNot)
            {
                ImpactHealth(collision);
            }
        }
    }

    private void ImpactHealth(Collider2D collision)
    {
        EnemyHealthController controller = collision.GetComponent<EnemyHealthController>();
        EnemyController toKnockback = collision.GetComponent<EnemyController>();

        if (controller != null)
        {
            controller.ChangeHealth(healthChangeAmount);
            PlayerController.ChangeShieldHealth(PlayerController.shieldHealthGainOnSwordAttack);

            // apply knockback
            Vector2 direction = (toKnockback.transform.position - transform.position).normalized;
            toKnockback.GetComponent<Rigidbody2D>().AddForce(direction * knockbackSpeed, ForceMode2D.Impulse);
            toKnockback.knockbackActive = true;
        }
    }
}
