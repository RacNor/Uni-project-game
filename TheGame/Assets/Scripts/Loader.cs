using UnityEngine;


class Loader : MonoBehaviour
{
    public GameObject gameManager;
    void Awake()
    {
        print("loader");
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
        else
        {
            print("scfasaffsa");
        }
    }
}

