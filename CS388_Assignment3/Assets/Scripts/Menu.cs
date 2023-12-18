using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button multiplay, quit;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(multiplay.gameObject);
        multiplay.onClick.AddListener(PlayByMultiple);
        quit.onClick.AddListener(Quit);
    }

    public void PlayByMultiple()
    {
        GameManager.Instance.SetPlayMode(false);
        SceneManager.LoadScene("Playground");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
