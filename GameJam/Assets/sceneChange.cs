using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    public void loadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
