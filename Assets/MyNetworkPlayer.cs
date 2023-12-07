using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;
    [SerializeField] private TMP_Text gameOverText = null;
    [SyncVar(hook = nameof(HandleDisplayNameUpdate))]
    [SerializeField]
    private string displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColourUpdate))]
    [SerializeField]
    private Color displayColor = Color.black;

    #region server
    [Server]
    public void setDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void setDisplayColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor;
    }
    private Vector3 GetRandomSpawnPosition()
    {
        // 返回一个新的随机生成位置，你需要根据你的场景和需求来实现这个方法
        return new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
    }
    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        //server authority to limit displayName into 2-20 letter length
        if (newDisplayName.Length < 2 || newDisplayName.Length > 20)
        {
            return;
        }
        RpcDisplayNewName(newDisplayName);
        setDisplayName(newDisplayName);
    }

    #endregion

    #region client
    private void HandleDisplayColourUpdate(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_BaseColor", newColor);
    }

    private void HandleDisplayNameUpdate(string oldName, string newName)
    {
        displayNameText.text = newName;
    }

    [ContextMenu("Set This Name")]
    private void SetThisName()
    {
        CmdSetDisplayName("This is a new name");
    }

    [ClientRpc]
    private void RpcDisplayNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }
    [ServerCallback]
    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("AI"))
        {
            NetworkServer.Destroy(collision.
                gameObject);

            RpcGameOver("AI has been defeated!");
           
        }
    }
   

    [ClientRpc]
    private void RpcGameOver(string message)
    {
        Debug.Log(message);

    }

    #endregion
}