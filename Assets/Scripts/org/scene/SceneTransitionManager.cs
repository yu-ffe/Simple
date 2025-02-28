using UnityEngine;
using UnityEngine.SceneManagement;

[System.Obsolete("사용 X")]
public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);

}
