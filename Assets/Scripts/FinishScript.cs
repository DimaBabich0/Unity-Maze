using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.name == "Player")
        {
            DontDestroyOnLoad(obj.gameObject);
            SceneManager.LoadScene("Scene 2");
        }
    }
}
