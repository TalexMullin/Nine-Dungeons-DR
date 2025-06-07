using UnityEngine;

public class ResourceRefill : MonoBehaviour
{
    public bool refillHealth = false;
    public int refillHealthAmount = 10;
    public bool refillMagic = false;
    public int refillMagicAmount = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {

            PlayerController controller = collision.GetComponent<PlayerController>();
            if (controller != null)
            {
                // check to see which resource should be increased
                
                // health
                if (refillHealth)
                {
                    controller.ChangeHealth(refillHealthAmount);
                }
                // magic
                if (refillMagic)
                {
                    controller.ChangeMagic(refillMagicAmount);
                }
                Destroy(gameObject);
            }
    }
}
