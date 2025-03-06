using UnityEngine;
using UnityEngine.InputSystem;

public class LanternLighter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject);
        /*if (Input.GetKeyUp(KeyCode.J))      //TODO: change hard coded key, also do so in PlayerController.
        {
            Destroy(gameObject);
        }*/
    }
}
