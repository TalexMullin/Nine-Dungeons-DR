using UnityEngine;
using UnityEngine.InputSystem;

public class LanternLighter : MonoBehaviour
{

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

            Destroy(gameObject);
            /*if (Input.GetKeyUp(KeyCode.J))      //TODO: change hard coded key, also do so in PlayerController.
            {
                Destroy(gameObject);
            }*/
    }
}
