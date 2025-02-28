using UnityEngine;

public class CoroutineHelper : MonoBehaviour {
    private static CoroutineHelper instance;
    public static CoroutineHelper Instance {
        get {
            if (instance == null) {
                // 씬에 이미 존재하는지 확인
                instance = FindObjectOfType<CoroutineHelper>();
                // 없으면 새 GameObject를 생성
                if (instance == null) {
                    GameObject helperObject = new GameObject("CoroutineHelper");
                    instance = helperObject.AddComponent<CoroutineHelper>();
                    DontDestroyOnLoad(helperObject);
                }
            }
            return instance;
        }
    }
}
