using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public float scrollSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        transform.position = new Vector3(player.position.x,
                transform.position.y, player.position.z - 5);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        //center camera on player with spacebar
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = new Vector3(player.position.x, 
                transform.position.y, player.position.z - 5);
        }

        //mouse at top of screen
        if(mousePos.y >= 0.95 * Screen.height)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * scrollSpeed);
        }
        //mouse at bottom of screen
        if (mousePos.y <= 0.05 * Screen.height)
        {
            transform.Translate(Vector3.back * Time.deltaTime * scrollSpeed);
        }
        //mouse at right of screen
        if (mousePos.x >= 0.95 * Screen.width)
        {
            transform.Translate(Vector3.right * Time.deltaTime * scrollSpeed);
        }
        //mouse at left of screen
        if (mousePos.x <= 0.05 * Screen.width)
        {
            transform.Translate(Vector3.left * Time.deltaTime * scrollSpeed);
        }

        ////scroll up
        //if (Input.GetAxis("Mouse ScrollWheel") < 0f) 
        //{
        //    transform.Translate(Vector3.up * Time.deltaTime * 50);
        //}
        //else if (Input.GetAxis("Mouse ScrollWheel") > 0f) 
        //{
        //    transform.Translate(Vector3.down * Time.deltaTime * 50);
        //}
    }
}
