using Mirror;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Ai : NetworkBehaviour
{
    public float speed = 5f;
    private Vector3 randomDirection;
    private float timer = 1f;
    [SyncVar(hook = nameof(HandleDestructionTextUpdate))]
    private string destructionTextMessage = "";
    [SerializeField]
    private TMP_Text destructionText=null;
    void Start()
    {
   
        randomDirection = GetRandomDirection();
    }

    void Update()
    {
        
        if (isServer)
        {
            
            transform.Translate(randomDirection * speed * Time.deltaTime);

            
            timer -= Time.deltaTime;

          
            if (timer <= 0f)
            {
                randomDirection = GetRandomDirection();
                timer = 2f; 
            }
        }
    }

    Vector3 GetRandomDirection()
    {
      
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);

        
        Vector3 direction = new Vector3(randomX, 0f, randomZ).normalized;

        return direction;
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Assuming the AI is destroyed when it collides with a player
            DestroyAI();
        }
    }

    [Server]
    private void DestroyAI()
    {
        // Display destruction text on all clients
        RpcDisplayDestructionText("AI Destroyed!");

        // Destroy the AI object on the server
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    private void RpcDisplayDestructionText(string message)
    {
        // Display destruction text on all clients using UI Text
        destructionText.text = message;
        Invoke(nameof(ResetText), 1f);
    }
    private void HandleDestructionTextUpdate(string oldMessage, string newMessage)
    {
        destructionText.text = newMessage;
        Invoke(nameof(ResetText), 1f);

    }

    private void ResetText()
    {
        // Reset the text
        destructionText.text = "";
    }


}
