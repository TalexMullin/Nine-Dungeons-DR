using System.Threading;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public static float degreesToSwing = 0.0f;
    public static float swingDuration = 1.0f;
    float swingTimer = 0.0f;
    public static float anglePerTime = 0.0f;
    public static float initialAngle = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (!PlayerController.gamePaused)
        {

            swingTimer += Time.deltaTime;
            gameObject.transform.eulerAngles = new Vector3(0, 0, initialAngle + swingTimer * (degreesToSwing / swingDuration));
            if (swingTimer >= swingDuration)
            {
                Destroy(gameObject);
            }
        }
    }

    public static void GetSwingDurationAndAngle(float swingDurationAmount, float degreesToSwingAmount, float initialAngleAmount)
    {
        swingDuration = swingDurationAmount;
        degreesToSwing = degreesToSwingAmount;
        anglePerTime = degreesToSwingAmount / swingDurationAmount;
        initialAngle = initialAngleAmount;
    }
}
