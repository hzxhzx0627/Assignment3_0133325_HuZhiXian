using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private float moveSpeed = 5f; 
    private Camera mainCamera;

    [Command]
    private void CmdMove(Vector3 direction)
    {
     
        RpcMove(direction);
    }

    [ClientRpc]
    private void RpcMove(Vector3 direction)
    {

        Move(direction);
    }

    private void Move(Vector3 direction)
    {

        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    public override void OnStartAuthority()
    {
        mainCamera = Camera.main; 
    }

    private void Update()
    {
        if (!hasAuthority)
        {
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

      
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

      
        Move(moveDirection);

      
        CmdMove(moveDirection);
    }
}
