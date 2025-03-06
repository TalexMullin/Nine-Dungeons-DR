using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Movement
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    public float playerSpeed = 3.0f;
    Vector2 move;

    // Health
    public int maxHealth = 100;
    int currentHealth;

    // damage cooldown
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    // lantern
    //public InputAction useLanternShield;
    // TODO: make lanternEquipped not public
    public bool lanternEquipped = true; // true means the lantern is equipped, false means the shield is equipped.
    public bool supportItemHeld = false; // for determining action for shield and lantern.
    public GameObject lanternLightPassive;
    public GameObject lanternLightUse;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        //useLanternShield.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        //Debug.Log(move);

        //supportItemHeld = useLanternShield.ReadValue<bool>();

        //timer for determining invincibility frames;
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }


        // for determining lantern light
        // determine if lanternEquipped for all of these
        if (lanternEquipped)
        {
            PassiveIlluminate();
            if (Input.GetKey(KeyCode.J))    // TODO: change hard coded key, change in LanternLighter as well.
            {
                Illuminate();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                supportItemHeld = true;
            }
            if (Input.GetKeyUp(KeyCode.J))
            {
                supportItemHeld = false;
            }
        }
        /* TODO: currently, there are several issues with how I have programmed things.
         * first and foremost is the hard coded letter J.
         * second is the way the lantern light follows the player.
         * Currently, I am creating and destroying a LanternLightHeld Prefab every frame
         * This works, but could likely be optimized to actually follow the player.
        */

        // for swapping between lantern and shield
        if (Input.GetKeyDown(KeyCode.I))    // TODO: change hard coded key.
        {
            if (lanternEquipped)
            {
                lanternEquipped = false;
            } else
            {
                lanternEquipped = true;
            }
        }

        // for shield mechanics
        // TODO: add shield mechanics here.

    }

    private void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * playerSpeed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }


    /* 
     * a function that changes player health and rounds if necessary
     */
    public void ChangeHealth(int amount)
    {
        // determine when to activate damage cooldown.
        if (amount < 0)
        {
            // kicks out if the player is already invincible.
            // Otherwise, invinciblilty is turned on and player takes damage.
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log("Health is" + currentHealth + "/" + maxHealth);
    }

    /*
     * functions used to create light from the lantern
     */
    void Illuminate()
    {
        GameObject lanternObject = Instantiate(lanternLightUse, rigidbody2d.position, Quaternion.identity);
    }
    void PassiveIlluminate()
    {
        GameObject lanternObject = Instantiate(lanternLightPassive, rigidbody2d.position, Quaternion.identity);
    }
}
