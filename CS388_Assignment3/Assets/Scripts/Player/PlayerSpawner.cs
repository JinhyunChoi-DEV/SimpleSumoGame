using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] PlayerSpawnPoints;
    public GameObject startLookAtPoint;

    private string[] characterResourcesName = { "Player_1", "Player_2", "Player_3", "Player_4" };

    public void Spawn()
    {
       
        SpawnMultiMode();

    }

    private void SpawnSingleMode()
    {
        for (int i = 0; i < 2; ++i)
        {
            var character = Resources.Load(characterResourcesName[i]) as GameObject;
            var player = Instantiate(character, PlayerSpawnPoints[i].transform.position, Quaternion.identity);

            if (i != 0)
                player.GetComponent<Player>().SetIdx(-1);   // -1 is Bot's Idx
            else
                player.GetComponent<Player>().SetIdx(i);

            player.transform.LookAt(startLookAtPoint.transform);
        }
    }

    private void SpawnMultiMode()
    {
        for (int i = 0; i < InputManager.Instance.CurrentPlayer; ++i)
        {
            var character = Resources.Load(characterResourcesName[i]) as GameObject;
            var player = Instantiate(character, PlayerSpawnPoints[i].transform.position, Quaternion.identity);
            player.GetComponent<Player>().SetIdx(i);
            player.transform.LookAt(startLookAtPoint.transform);
        }
    }
}
