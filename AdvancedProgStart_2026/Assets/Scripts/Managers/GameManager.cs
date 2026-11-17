using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (!instance)
                instance = FindFirstObjectByType<GameManager>();
            if (!instance)
                throw new System.Exception("No Game Manager Here");

            return instance;
        }
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    [SerializeField] private GameObject manFisherGO;
    public GameObject ManFisherGo => manFisherGO;

    private PlayerBody manFisher;
    public PlayerBody ManFisher => manFisher;

    [SerializeField] private string[] levelNames;

    private void Start()
    {
        manFisher = manFisherGO.GetComponent<PlayerBody>();

        StartCoroutine(LoadingManager.LoadLevel(levelNames[0]));
    }

    public void LevelComplete()
    {
        ObjectPoolManager.Instance.GameReset();

        LoadingManager.currentLevel++;
        if(LoadingManager.currentLevel < levelNames.Length)
        {
            StartCoroutine(LoadingManager.LoadLevel(levelNames[LoadingManager.currentLevel]));
        }
    }
}
