using CnControls;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    private Player player;
    private float axisX = 0;

    void Start()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        UpdatePlayerMovement();
    }

    private void Update()
    {
        //CnInputManager is on screen joystick
        axisX = Input.GetAxis("Horizontal")+ CnInputManager.GetAxis("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        player.LunchRope();
    }

    private void UpdatePlayerMovement()
    {
        player.Move(axisX);
    }
}
