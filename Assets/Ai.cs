using Mirror;
using UnityEngine;

public class Ai : NetworkBehaviour
{
    public float speed = 5f;
    private Vector3 randomDirection;
    private float timer = 1f;

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

   

}
