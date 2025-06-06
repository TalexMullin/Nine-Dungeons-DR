using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerFaceVert
{
    up,
    down,
    none
}
public enum PlayerFaceHor
{
    left,
    right,
    none
}

public class PlayerController : MonoBehaviour
{
    // Temporary KeyMappings
    // TODO: these are hard coded at the moment, later controls should be customizable
    KeyCode supportItemKey = KeyCode.J;
    KeyCode supportItemSwitchKey = KeyCode.I;
    // KeyCode pauseKey = KeyCode.Escape;
    KeyCode attackKey = KeyCode.Space;
    KeyCode dodgeKey = KeyCode.K;


    // Movement
    [Header("Movement")]
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    public float playerSpeed = 3.0f;
    float playerSpeedTemp;
    // playerSpeedLanternReduction divides player speed when the lantern is being used.
    public float playerSpeedLanternReduction = 2.5f;
    Vector2 move;
    Vector2 moveDirection = new Vector2(1, 0);
    PlayerFaceVert playerFacingVert = PlayerFaceVert.down;
    PlayerFaceHor playerFacingHor = PlayerFaceHor.none;


    [Header("Health and Resources")]

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

    [Header("Lantern")]
    // lantern
    //public InputAction useLanternShield;
    bool lanternEquipped = false; // true means the lantern is equipped, false means the shield is equipped.
    public static bool supportItemHeld = false; // for determining action for shield and lantern.
    public GameObject lanternLightPassive;
    public GameObject lanternLightUse;
    public int maxLanternFuel = 100;
    int currentLanternFuel;
    float lanternPercentIncPerSec = 0.01f;
    int lanternFuelUsePerSec = -8;
    float lanternFiller = 0;
    float lanternDrainer = 0;
    //float lanternNoLightCooldown = 0;

    // shield
    public float maxShieldHealth = 4.0f;
    float currentShieldHealth;
    public static float shieldHealthLossOnBlock = -1.0f;   // lose one block upon blocking
    public static float shieldHealthGainOnSwordAttack = 0.25f;


    [Header("Combat")]
    // combat
    public GameObject swordPrefab;
    GameObject sword;
    public GameObject shieldPrefab;
    GameObject shield;
    public bool shieldActive = false;
    public float attackCooldownAmount = 0.5f;
    float attackCooldownTimer = 0;
    bool attackOnCooldown = false;

    [Header("SwordVariables")]
    public float swordDistanceDownOffset = 0.5f;
    public float swordDistanceUp = 1.0f;
    public float swordDistanceDown = 1.0f;
    public float swordDistanceLeftRight = 1.0f;
    public float initialSwingAngle = 0.0f;

    [Header("ShieldVariables")]
    public float shieldDistanceDownOffset = 0.0f;
    public float shieldDistanceUp = 1.0f;
    public float shieldDistanceDown = 1.0f;
    public float shieldDistanceLeftRight = 1.0f;

    [Header("DodgeRollVariables")]
    public float dodgeRollSpeed = 10.0f;
    bool dodgeRollActive = false;
    float dodgeRollDurationTimer;
    public float dodgeRollDurationAmount = 0.25f;
    float dodgeRollCooldownTimer;
    public float dodgeRollCooldownAmount = 2.0f;
    bool dodgeRollOnCooldown = false;
    Vector2 dodgeMove;

    [Header("Knockback")]
    public bool knockbackActive = false;
    public float knockbackTimer;
    public float knockbackAmount = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*
     *
     *
     *
     *
     *
     */
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentMagic = 0;  // TODO: set to maxMagic once magic items are available
        currentLanternFuel = maxLanternFuel;
        currentShieldHealth = maxShieldHealth;

        //useLanternShield.Enable();
    }



    // Update is called once per frame
    /*
     *
     *
     *
     *
     *
     */
    void Update()
    {
            if (!knockbackActive) // ensure that player is not being knocked back
            {
                if (attackOnCooldown) // halt movement if attacking
                {
                    move = new Vector2(0, 0);

                }
                else
                {
                    move = MoveAction.ReadValue<Vector2>();
                }
                //Debug.Log(move);
                if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
                {
                    moveDirection.Set(move.x, move.y);
                    moveDirection.Normalize();
                }
                // determine approximately which direction the player is facing
                // ensure the player is not standing still before changing direction
                // this way, the player is still marked as facing a direction
                if (!(move.y == 0 && move.x == 0))
                {
                    // find player vertical direction
                    switch (move.y)
                    {
                        case < 0:
                            playerFacingVert = PlayerFaceVert.down;
                            break;
                        case > 0:
                            playerFacingVert = PlayerFaceVert.up;
                            break;
                        default:
                            playerFacingVert = PlayerFaceVert.none;
                            break;
                    }
                    // find player horizontal direction
                    switch (move.x)
                    {
                        case < 0:
                            playerFacingHor = PlayerFaceHor.left;
                            break;
                        case > 0:
                            playerFacingHor = PlayerFaceHor.right;
                            break;
                        default:
                            playerFacingHor = PlayerFaceHor.none;
                            break;
                    }
                    // assign x and y of move to dodgeMove
                    dodgeMove.x = move.x;
                    dodgeMove.y = move.y;
                }
            }

            // knockback timer
            if (knockbackActive)
            {
                knockbackTimer += Time.deltaTime;
                if (knockbackTimer >= knockbackAmount)
                {
                    rigidbody2d.linearVelocity = Vector2.zero;
                    knockbackActive = false;
                    knockbackTimer = 0;
                }
            }

            //Debug.Log("vertical direction: " + playerFacingVert +"\nhorizontal direction: " + playerFacingHor);

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
                if (Input.GetKey(supportItemKey) && currentLanternFuel > 0 && !attackOnCooldown && !dodgeRollActive)
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

            /* TODO: currently, there are several issues with how I have programmed things.
             * primarily is the way the lantern light follows the player.
             * Currently, I am creating and destroying a LanternLightHeld Prefab every frame
             * This works, but could be optimized to actually follow the player.
            */

            } else if (!lanternEquipped)
            {
                if (Input.GetKeyDown(supportItemKey) && currentShieldHealth >= Math.Abs(shieldHealthLossOnBlock)
                    && !attackOnCooldown && !dodgeRollActive)
                {
                    Vector2 shieldLocation = rigidbody2d.position;
                    float angle = 0;
                    // get shield location
                    switch (playerFacingVert)
                    {
                        case PlayerFaceVert.up:
                            shieldLocation.y += shieldDistanceUp;
                            break;
                        case PlayerFaceVert.down:
                            shieldLocation.y -= shieldDistanceDown;
                            break;
                        // offset for if the player is facing straight left or stright right
                        case PlayerFaceVert.none:
                            shieldLocation.y -= shieldDistanceDownOffset;
                            break;
                    }
                    switch (playerFacingHor)
                    {
                        case PlayerFaceHor.right:
                            shieldLocation.x += shieldDistanceLeftRight;
                            break;
                        case PlayerFaceHor.left:
                            shieldLocation.x -= shieldDistanceLeftRight;
                            break;
                    }

                    // get angle
                    if (playerFacingHor == PlayerFaceHor.left)
                    {
                        if (playerFacingVert == PlayerFaceVert.up) // angle left up
                        {
                            angle += 45.0f;
                        }
                        else if (playerFacingVert == PlayerFaceVert.down) // angle left down
                        {
                            angle += 135.0f;
                        }
                        else // angle straight left
                        {
                            angle += 90.0f;
                        }
                    }
                    else if (playerFacingHor == PlayerFaceHor.right)
                    {
                        if (playerFacingVert == PlayerFaceVert.up) // angle right up
                        {
                            angle += 315.0f;
                        }
                        else if (playerFacingVert == PlayerFaceVert.down) // angle right down
                        {
                            angle += 225.0f;
                        }
                        else // angle right
                        {
                            angle += 270.0f;
                        }
                    }
                    else if (playerFacingVert == PlayerFaceVert.down) // angle down
                    {
                        angle += 180.0f;
                    }
                    // nothing for angle down, as it is the default
                    shield = Instantiate(shieldPrefab, shieldLocation, Quaternion.Euler(0, 0, angle)) as GameObject;
                }
            }

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
                }
                else
                {
                    lanternEquipped = true;
                }
            }


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


            // attack with the sword
            if (Input.GetKeyDown(attackKey) && !attackOnCooldown && !supportItemHeld && !dodgeRollActive)
            {
                Vector2 swordLocation = rigidbody2d.position;
                float angle = initialSwingAngle;
                float swingAngle = -2 * angle;
                // get sword location
                switch (playerFacingVert)
                {
                    case PlayerFaceVert.up:
                        swordLocation.y += swordDistanceUp;
                        break;
                    case PlayerFaceVert.down:
                        swordLocation.y -= swordDistanceDown;
                        break;
                    // offset for if the player is facing straight left or stright right
                    case PlayerFaceVert.none:
                        swordLocation.y -= swordDistanceDownOffset;
                        break;
                }
                switch (playerFacingHor)
                {
                    case PlayerFaceHor.right:
                        swordLocation.x += swordDistanceLeftRight;
                        break;
                    case PlayerFaceHor.left:
                        swordLocation.x -= swordDistanceLeftRight;
                        break;
                }
                // get sword angle
                // TODO: Consider angeling sword
                // an invisible child object will have to rotate as the sword's rotation point is in the center
                if (playerFacingHor == PlayerFaceHor.left)
                {
                    if (playerFacingVert == PlayerFaceVert.up) // angle left up
                    {
                        angle += 45.0f;
                    }
                    else if (playerFacingVert == PlayerFaceVert.down) // angle left down
                    {
                        angle += 135.0f;
                    }
                    else // angle straight left
                    {
                        angle += 90.0f;
                    }
                }
                else if (playerFacingHor == PlayerFaceHor.right)
                {
                    if (playerFacingVert == PlayerFaceVert.up) // angle right up
                    {
                        angle += 315.0f;
                    }
                    else if (playerFacingVert == PlayerFaceVert.down) // angle right down
                    {
                        angle += 225.0f;
                    }
                    else // angle right
                    {
                        angle += 270.0f;
                    }
                }
                else if (playerFacingVert == PlayerFaceVert.down) // angle down
                {
                    angle += 180.0f;
                }
                // nothing for angle down, as it is the default

                // instantiante the sword using angle and location
                sword = Instantiate(swordPrefab, swordLocation, Quaternion.Euler(0, 0, angle)) as GameObject;
                SwordSwing.GetSwingDurationAndAngle(attackCooldownAmount, swingAngle, angle);
                attackOnCooldown = true;
            }
            if (attackOnCooldown)
            {
                attackCooldownTimer += Time.deltaTime;
                if (attackCooldownTimer >= attackCooldownAmount)
                {
                    attackOnCooldown = false;
                    attackCooldownTimer = 0;
                }
            }

            // dodge roll
            if (!dodgeRollActive && !dodgeRollOnCooldown && !supportItemHeld && !attackOnCooldown && Input.GetKeyDown(dodgeKey))
            {
                dodgeRollActive = true;
                HUDHandler.instance.SetDodgeValue(0);   // alter HUD
            }
            if (dodgeRollActive)
            {
                dodgeRollDurationTimer += Time.deltaTime;
                if (dodgeRollDurationTimer >= dodgeRollDurationAmount)
                {
                    dodgeRollActive = false;
                    dodgeRollDurationTimer = 0;
                    dodgeRollOnCooldown = true;
                }
            }
            if (dodgeRollOnCooldown)
            {
                dodgeRollCooldownTimer += Time.deltaTime;
                HUDHandler.instance.SetDodgeValue(dodgeRollCooldownTimer / dodgeRollCooldownAmount);   // alter HUD
                if (dodgeRollCooldownTimer >= dodgeRollCooldownAmount)
                {
                    dodgeRollOnCooldown = false;
                    dodgeRollCooldownTimer = 0;
                }
            }
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //


        // reduce player movement speed while holding the lantern key, reset it on release
        if (lanternEquipped)
        {
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

        // halt player movement speed while holding the shield key, reset it on release
        // also, change shieldActive to true or false so that shield can be destroyed when appropriate
        if (!lanternEquipped)
        {
            if (Input.GetKeyDown(supportItemKey))
            {
                supportItemHeld = true;
                playerSpeedTemp = playerSpeed;
                playerSpeed = 0;
                shieldActive = true;
            }
            if (Input.GetKeyUp(supportItemKey))
            {
                supportItemHeld = false;
                playerSpeed = playerSpeedTemp;
                shieldActive = false;
            }
        }

    }



    // FixedUpdate ensures independence from framerate
    // anything that has to do with movement and physics should go here instead of in Update
    /*
     *
     *
     *
     *
     *
     */
    private void FixedUpdate()
    {

            if (!knockbackActive)
            {
                Vector2 position;
                // Movement
                // check for dodge roll movement first, otherwise use normal movement.
                if (dodgeRollActive)
                {
                    position = (Vector2)rigidbody2d.position + dodgeMove * dodgeRollSpeed * Time.deltaTime;
                }
                else
                {
                    position = (Vector2)rigidbody2d.position + move * playerSpeed * Time.deltaTime;
                }

                rigidbody2d.MovePosition(position);
            }
    }



    // a function that changes player health and rounds if necessary
    /*
     *
     *
     *
     *
     *
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
            shieldActive = false;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        HUDHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
        //Debug.Log("Health is" + currentHealth + "/" + maxHealth);
    }



    // a function that changes player magic and rounds if necessary.
    /*
     *
     *
     *
     *
     *
     */
    public void ChangeMagic(int amount)
    {
        // make sure any items that uses magic checks if there is enough magic
        currentMagic = Mathf.Clamp(currentMagic + amount, 0, maxMagic);
        HUDHandler.instance.SetMagicValue(currentMagic / (float)maxMagic);
        //Debug.Log("Magic is" + currentMagic + "/" + maxMagic);
    }


    // a function that changes shield health
    /*
     *
     *
     *
     *
     *
     */
    public void ChangeShieldHealth(float amount)
    {
        currentShieldHealth = Mathf.Clamp(currentShieldHealth + amount, 0, maxShieldHealth);
        HUDHandler.instance.SetShieldValue(currentShieldHealth / maxShieldHealth);
        if (currentShieldHealth < Math.Abs(shieldHealthLossOnBlock))
        {
            shieldActive = false;
        }
        //Debug.Log("Shield health is at " + currentShieldHealth + "/" + maxShieldHealth);
    }


    // a function that changes player lantern fuel and rounds if necessary.
    /*
     *
     *
     *
     *
     *
     */
    public void ChangeLanternFuel(int amount)
    {
        currentLanternFuel = Mathf.Clamp(currentLanternFuel + amount, 0, maxLanternFuel);
        HUDHandler.instance.SetLanternValue(currentLanternFuel / (float)maxLanternFuel);
        //Debug.Log("Lantern Fuel is " + currentLanternFuel + "/" + maxLanternFuel);
    }

    // functions used to create light from the lantern
    /*
     *
     *
     *
     *
     *
     */
    void Illuminate()
    {
        GameObject lanternObject = Instantiate(lanternLightUse, rigidbody2d.position, Quaternion.identity);
    }
    void PassiveIlluminate()
    {
        GameObject lanternObject = Instantiate(lanternLightPassive, rigidbody2d.position, Quaternion.identity);
    }



    // a function that checks if an interger is < 1
    // if it is, it converts the integer to 1
    /*
     *
     *
     *
     *
     *
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