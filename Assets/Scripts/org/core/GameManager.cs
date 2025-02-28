using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    protected override void Awake() {
        base.Awake(); // 싱글톤 기본 동작 유지
    }

    void Start() {

    }
    
    void Update() {

    }

    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);

}
