using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public static float swingDuration = 1.0f;
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

    public static void GetSwingDuration(float amount)
    {
        swingDuration = amount;
    }
}
