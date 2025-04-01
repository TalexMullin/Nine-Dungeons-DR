using UnityEngine;

public class HealthImpacterEnemy : MonoBehaviour
{
    //public vars that can be adjusted in settings.
    public bool enterOrNot = true; // determins if ontriggerenter or ontriggerstay runs.
    public int healthChangeAmount = -5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PlayerController.gamePaused)
        {
            if (enterOrNot)
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
