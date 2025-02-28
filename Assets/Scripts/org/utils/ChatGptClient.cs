using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;

[Serializable]
public class ChatGPTMessage {
    public string role;
    public string content;
}

[Serializable]
public class ChatGPTRequest {
    public string model;
    public bool store; // 추가된 store 속성
    public ChatGPTMessage[] messages;
}

public class ChatGPTClient {
    private string apiKey; // 초기값 null로 설정

    public delegate void OnResponseReceived(string response);

    // 생성자에서 환경 변수로 API 키 설정
    public ChatGPTClient() {
        apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrEmpty(apiKey)) {
            Debug.LogError("ChatGPT API 키가 설정되지 않았습니다. 환경 변수 'OPENAI_API_KEY'를 설정하세요.");
        }
    }

    public void SendMessageToChatGPT(string message, OnResponseReceived callback) {
        if (string.IsNullOrEmpty(apiKey)) {
            Debug.LogError("API 키가 없어 요청을 보낼 수 없습니다.");
            return;
        }

        CoroutineHelper.Instance.StartCoroutine(SendRequest(message, callback));
    }

    private IEnumerator SendRequest(string prompt, OnResponseReceived callback) {
        string url = "https://api.openai.com/v1/chat/completions";

        // 메시지 내용과 추가된 store 속성
        ChatGPTMessage chatMessage = new ChatGPTMessage { role = "user", content = prompt };
        ChatGPTRequest requestData = new ChatGPTRequest { 
            model = "gpt-4o-mini", 
            store = true,  // store 속성 추가
            messages = new ChatGPTMessage[] { chatMessage } 
        };

        string jsonData = JsonUtility.ToJson(requestData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            callback(request.downloadHandler.text);
        else
            Debug.LogError("ChatGPT 요청 실패: " + request.error);
    }
}
