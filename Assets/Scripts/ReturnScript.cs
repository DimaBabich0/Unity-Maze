using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.name == "Player")
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
