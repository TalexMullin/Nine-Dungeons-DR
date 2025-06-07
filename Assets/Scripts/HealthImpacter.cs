using UnityEngine;

public class HealthImpacter : MonoBehaviour
{
    //public vars that can be adjusted in settings.
    public bool enterOrNot = true; // determins if ontriggerenter or ontriggerstay runs.
    public int healthChangeAmount = -5;
    public float knockbackSpeed = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (enterOrNot)
        {
            ImpactHealth(collision);
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

        if (!enterOrNot)
        {
            ImpactHealth(collision);
        }
    }

    private void ImpactHealth(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();

        if (controller != null)
        {
        controller.ChangeHealth(healthChangeAmount);

        // apply knockback
        Vector2 direction = (controller.transform.position - transform.position).normalized;
        controller.GetComponent<Rigidbody2D>().AddForce(direction * knockbackSpeed, ForceMode2D.Impulse);
        controller.knockbackActive = true;
        }
    }
}
