using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    public string mapSceneName;

    public void LoadMap()
    {
        SceneManager.LoadScene(mapSceneName);
    }
}