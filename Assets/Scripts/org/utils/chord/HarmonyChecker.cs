using System;
using UnityEngine;

[System.Serializable]
public class Choice {
    public Message message;
}

[System.Serializable]
public class Message {
    public string role;
    public string content;
    public object refusal;
}

[System.Serializable]
public class ChatResponse {
    public string id;
    public string @object;
    public int created;
    public string model;
    public Choice[] choices;
}


public class HarmonyChecker {
    private ChatGPTClient chatGPTClient;

    // 생성자에서 ChatGPTClient 인스턴스를 받아 초기화
    public HarmonyChecker(ChatGPTClient chatGPTClientInstance) {
        chatGPTClient = chatGPTClientInstance;
    }

    // 외부에서 호출할 수 있는 함수 (콜백을 사용하여 결과 반환)
    public void CheckHarmony(string inputNotes, string rhythm, Action<float> callback) {
        string prompt = $"음계: {inputNotes}, 박자: {rhythm}박자\n" +
                        "이 음과 박자가 조화로운지 0~1 사이 값으로 숫자만 답해줘. 중간은 0.5.";

        // ChatGPT로 요청하고, 응답을 받을 때 콜백 호출
        chatGPTClient.SendMessageToChatGPT(prompt, (response) => OnResponseReceived(response, callback));
    }

    // 응답을 처리하는 부분 (콜백을 통해 float 값을 반환)
    private void OnResponseReceived(string response, Action<float> callback) {
        Debug.Log(response);
        
        ChatResponse chatResponse = JsonUtility.FromJson<ChatResponse>(response);
        string content = chatResponse.choices[0].message.content;
        
        // 'content'를 float로 변환 시도
        if (float.TryParse(content, out float harmonyScore)) {
            Debug.Log("하모니 점수: " + harmonyScore);
            callback(harmonyScore);
        } else {
            Debug.LogWarning("하모니 점수 변환 실패");
            callback(0f); // 오류 시 0으로 반환
        }
    }
}
