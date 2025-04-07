using UnityEngine;

public class HealthImpacterEnemy : MonoBehaviour
{
    //public vars that can be adjusted in settings.
    public bool enterOrNot = true; // determins if ontriggerenter or ontriggerstay runs.
    public int healthChangeAmount = -5;
    public float knockbackSpeed = 1;
    public enum TypeOfItem
    {
        sword,
        shield,
        other
    }
    public TypeOfItem itemType = TypeOfItem.other;

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
        PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.ChangeHealth(healthChangeAmount);

            // decide what to do with shield health
            switch (itemType) {
                case TypeOfItem.sword:
                    playerController.ChangeShieldHealth(PlayerController.shieldHealthGainOnSwordAttack);
                    break;
                case TypeOfItem.shield:
                    playerController.ChangeShieldHealth(PlayerController.shieldHealthLossOnBlock);
                    break;
                default:
                    break;
            }
            

            // apply knockback
            Vector2 direction = (toKnockback.transform.position - transform.position).normalized;
            toKnockback.GetComponent<Rigidbody2D>().AddForce(direction * knockbackSpeed, ForceMode2D.Impulse);
            toKnockback.knockbackActive = true;
        }
    }
}
