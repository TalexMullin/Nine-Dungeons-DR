using UnityEngine;

public class ShieldBehavior : MonoBehaviour
{
    GameObject playerObject;
    PlayerController playerConScript;
    float currentShieldHealth;
    float maxShieldHealth;
    private void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerConScript = playerObject.GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!playerConScript.shieldActive)
        {
            Destroy(gameObject);
        }
    }
}
