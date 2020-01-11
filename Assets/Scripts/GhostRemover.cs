using UnityEngine;

public class GhostRemover : MonoBehaviour
{
    private GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (gameObject.transform.position.x < Player.transform.position.x - 4f)
        {
            Destroy(gameObject);
        }      
    }
}
