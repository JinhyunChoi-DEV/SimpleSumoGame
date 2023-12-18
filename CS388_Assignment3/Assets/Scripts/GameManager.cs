using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public bool IsSolo { get; private set; }
    public bool IsPlaying { get; private set; }
    public bool IsFinishedGame { get; private set; }
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;

            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        IsPlaying = false;
        //IsSolo = true;
        IsFinishedGame = false;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (IsPlaying)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            int count = 0;
            foreach (var player in players)
            {
                if (player.GetComponent<Player>().Alive)
                    count++;
            }

            if (count == 1)
            {
                IsFinishedGame = true;
                IsPlaying = false;
            }
        }
    }

    public void SetPlayMode(bool isSingle)
    {
        //IsSolo = isSingle;
        IsFinishedGame = false;
    }

    public void PlayingGame()
    {
        IsPlaying = true;
    }

    public void NotPlayingGame()
    {
        IsPlaying = false;
    }
}
