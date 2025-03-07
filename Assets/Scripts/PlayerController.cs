using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Temporary KeyMappings
    // TODO: these are solid at the moment, later controls should be customizable
    KeyCode supportItemKey = KeyCode.J;
    KeyCode supportItemSwitchKey = KeyCode.I;

    // Movement
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    public float playerSpeed = 3.0f;
    // playerSpeedLanternReduction divides player speed when the lantern is being used.
    public float playerSpeedLanternReduction = 2.5f;
    Vector2 move;

    // Health
    public int maxHealth = 100;
    int currentHealth;

    // magic
    public int maxMagic = 100;  // note that calculations might not work if this number is very small
    int currentMagic;
    float magicPercentIncPerSec = 0.0166f;  // 0.033f fills in ~30s, 0.0166f fills in ~60s
    float magicFiller = 0;

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
    public int maxLanternFuel = 100;
    int currentLanternFuel;
    float lanternPercentIncPerSec = 0.01f;
    int lanternFuelUsePerSec = -8;
    float lanternFiller = 0;
    float lanternDrainer = 0;
    //float lanternNoLightCooldown = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentMagic = 50;  // TODO: set to maxMagic
        currentLanternFuel = maxLanternFuel;

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
            // ensure that the correct key is pressed and there is enough fuel. Divided by 4 to smooth out the process.
            if (Input.GetKey(supportItemKey) && currentLanternFuel > 0)
            {
                Illuminate();
                // drain lantern fuel
                // note that numbers are divided by 4 to smooth out the process.
                lanternDrainer += Time.deltaTime;
                if (lanternDrainer >= 0.25)
                {
                    ChangeLanternFuel(lanternFuelUsePerSec / 4);
                    lanternDrainer = 0;
                }
            }
            if (Input.GetKeyDown(supportItemKey))
            {
                supportItemHeld = true;
                playerSpeed /= playerSpeedLanternReduction;   // reduce player speed when they are using the lantern.
            }
            if (Input.GetKeyUp(supportItemKey))
            {
                supportItemHeld = false;
                playerSpeed *= playerSpeedLanternReduction;
            }
        }
        /* TODO: currently, there are several issues with how I have programmed things.
         * second is the way the lantern light follows the player.
         * Currently, I am creating and destroying a LanternLightHeld Prefab every frame
         * This works, but could likely be optimized to actually follow the player.
        */

        // for determining if lantern fuel regens or not
        if (((lanternEquipped && !supportItemHeld) || !lanternEquipped) && (currentLanternFuel < maxLanternFuel))
        {
            lanternFiller += Time.deltaTime;
            if (lanternFiller >= 1)
            {
                ChangeLanternFuel(ConvertIntToOne((int)Math.Round(maxLanternFuel * (lanternPercentIncPerSec))));
                lanternFiller = 0;    // reset lantern filler
            }
        }

        // for swapping between lantern and shield. supportItemHeld must be false to prevent movement bugs.
        if (Input.GetKeyDown(supportItemSwitchKey) && !supportItemHeld)
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

        // for passive magic regeneration
        if (currentMagic < maxMagic)
        {
            magicFiller += Time.deltaTime;
            if (magicFiller >= 1)   // hard coded number because magic will always increase per set amount of time.
            {
                ChangeMagic(ConvertIntToOne((int)Math.Round(maxMagic * (magicPercentIncPerSec))));
                magicFiller = 0;    // reset magicFiller
            }
        }
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
        HUDHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
        //Debug.Log("Health is" + currentHealth + "/" + maxHealth);
    }

    /*
     * a function that changes player magic and rounds if necessary.
     */
    public void ChangeMagic(int amount)
    {
        // make sure any items that uses magic checks if there is enough magic
        currentMagic = Mathf.Clamp(currentMagic + amount, 0, maxMagic);
        HUDHandler.instance.SetMagicValue(currentMagic / (float)maxMagic);
        //Debug.Log("Magic is" + currentMagic + "/" + maxMagic);
    }


    /*
     * a function that changes player lantern fuel and rounds if necessary.
     */
    public void ChangeLanternFuel(int amount)
    {
        currentLanternFuel = Mathf.Clamp(currentLanternFuel + amount, 0, maxLanternFuel);
        HUDHandler.instance.SetLanternValue(currentLanternFuel / (float)maxLanternFuel);
        Debug.Log("Lantern Fuel is " + currentLanternFuel + "/" + maxLanternFuel);
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

    /*
     * a function that checks if an interger is < 1
     * if it is, it converts the integer to 1
     */
    int ConvertIntToOne(int integer)
    {
        if (integer < 1)
        {
            integer = 1;
        }
        return integer;
    }
}
