using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public float swingDuration = 0.5f;
    float swingTimer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (!PlayerController.gamePaused)
        {
            swingTimer += Time.deltaTime;
            if (swingTimer >= swingDuration)
            {
                Destroy(gameObject);
            }
        }
    }

    public void GetSwingDuration(float amount)
    {
        this.swingDuration = amount;
    }
}
