using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayingUIManager : MonoBehaviour
{
    public Camera baseMainCamera;
    public GameObject hide4thPlayerScene, endGamePanel, SettingPanel;
    public Button multiStartButton, multiMenuButton;
    public TMP_Text multiCountText, winnerPlayerText;
    public PlayerSpawner spawner;
    public Button menuButton;


    // Start is called before the first frame update
    void Start()
    {
        baseMainCamera.enabled = true;
        SettingPanel.SetActive(true);


        hide4thPlayerScene.SetActive(false);
        endGamePanel.SetActive(false);
       
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(multiStartButton.gameObject);

        multiStartButton.onClick.AddListener(PlayGame);
        multiMenuButton.onClick.AddListener(GotoMenu);
        menuButton.onClick.AddListener(GotoMenu);

        if (Application.platform == RuntimePlatform.Switch)
            InputManager.Instance.ShowControllerSupport();

    }

    // Update is called once per frame
    void Update()
    {
        multiCountText.text = InputManager.Instance.CurrentPlayer.ToString();

        if (GameManager.Instance.IsFinishedGame && !endGamePanel.activeSelf)
        {
            baseMainCamera.enabled = true;
            endGamePanel.SetActive(true);

            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                var p = player.GetComponent<Player>();
                if (p.Alive)
                {
                    if (p.IsBot)
                    {
                        winnerPlayerText.text = "Winner Player is: BOT";
                    }
                    else
                    {
                        var number = (p.idx + 1).ToString();
                        winnerPlayerText.text = string.Format("Winner Player is: Player{0}", number);
                    }
                }
            }

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(menuButton.gameObject);
        }
    }

    private void PlayGame()
    {
        hide4thPlayerScene.SetActive(InputManager.Instance.CurrentPlayer == 3);
        GameManager.Instance.PlayingGame();
        SettingPanel.SetActive(false);
        //debugPanel.SetActive(false);

        spawner.Spawn();
        baseMainCamera.enabled = false;
    }

    private void GotoMenu()
    {
        SceneManager.LoadScene("Main");
    }

}
