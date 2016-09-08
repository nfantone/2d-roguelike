using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public GameObject gameManager;

    // Use this for initialization
    void Awake()
    {
        if (GameManager.instance == null)
        {
            // Instantiate gameManager prefab
            Instantiate(gameManager);
        }
    }

}
