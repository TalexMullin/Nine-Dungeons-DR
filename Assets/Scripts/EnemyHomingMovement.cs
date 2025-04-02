using UnityEngine;
using UnityEngine.Splines;

public class EnemyHomingMovement : MonoBehaviour
{
    // movement
    Rigidbody2D rigidbody2d;
    Rigidbody2D playerRigidbody2d;
    Vector2 playerPosition;
    GameObject target = null;
    public float moveSpeed = 2;
    public float antiJitterVar = 0.075f;
    //Vector2 move;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*
     * 
     * 
     * 
     */
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    /*
     * 
     * 
     * 
     */
    void Update()
    {
        if (!PlayerController.gamePaused)
        {


        }
    }


    // FixedUpdate
    /*
     * 
     * 
     * 
     */
    private void FixedUpdate()
    {
        if (!PlayerController.gamePaused)
        {
            Vector2 position = rigidbody2d.position;
            if (target != null)
            {   // if there is a target, move towards it
                playerPosition = playerRigidbody2d.position;
                // decide which direction to go
                if (Mathf.Abs(playerPosition.x - position.x) > antiJitterVar)
                {
                    if (playerPosition.x > position.x)
                    {
                        position.x = position.x + moveSpeed * Time.deltaTime;
                    }
                    else if (playerPosition.x < position.x)
                    {
                        position.x = position.x - moveSpeed * Time.deltaTime;
                    }
                }
                if (Mathf.Abs(playerPosition.y - position.y) > antiJitterVar)
                {
                    if (playerPosition.y > position.y)
                    {
                        position.y = position.y + moveSpeed * Time.deltaTime;
                    }
                    else if (playerPosition.y < position.y)
                    {
                        position.y = position.y - moveSpeed * Time.deltaTime;
                    }
                }
            } else
            {   // otherwise, find the target
                target = GameObject.FindWithTag("Player");
                playerRigidbody2d = target.GetComponent<Rigidbody2D>();
            }

                rigidbody2d.MovePosition(position);
        }
    }
}
