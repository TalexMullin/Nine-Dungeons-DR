using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    //public static float degreesToSwing = 0.0f;
    public static float swingDuration = 1.0f;
    float swingTimer = 0.0f;
    //public static float anglePerDeltaTime = 0.0f;

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

    /*private void FixedUpdate()
    {
        transform.Rotate(0, 0, degreesToSwing * Time.deltaTime);//anglePerDeltaTime);
    }*/

    public static void GetSwingDurationAndAngle(float swingDurationAmount)//, float degreesToSwingAmount)
    {
        swingDuration = swingDurationAmount;
        //degreesToSwing = degreesToSwingAmount;
        //anglePerDeltaTime = Time.deltaTime * (degreesToSwingAmount/swingDurationAmount);
    }
}
