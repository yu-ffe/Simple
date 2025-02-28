using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using workspace.YU__FFE.utils;

namespace workspace.YU__FFE {
    public class AudioRecorder : MonoBehaviour {
        private AudioClip recordedClip;
        private AudioClip gameAudioClip;
        private string apiUrl = "http://localhost:8000/analyze";
        public TextMeshProUGUI resultText; // UI에 결과 표시

        private float[] gameAudioData;
        private float[] microphoneAudioData;

        public void StartRecording() {
            // 마이크로폰 녹음 시작
            recordedClip = Microphone.Start(null, false, 10, 44100);

            // 게임 소리 녹음 시작
            gameAudioClip = AudioClip.Create("GameAudio", 44100 * 10, 1, 44100, true);  // 10초간 녹음
            gameAudioData = new float[44100 * 10];  // 10초의 오디오 데이터 배열 초기화
        }

        public void StopRecording() {
            // 마이크로폰 녹음 종료
            Microphone.End(null);

            // 게임 소리 캡처
            AudioListener.GetOutputData(gameAudioData, 0);

            // 마이크와 게임 소리 합침
            float[] combinedAudioData = new float[gameAudioData.Length + recordedClip.samples];
            gameAudioData.CopyTo(combinedAudioData, 0);
            recordedClip.GetData(microphoneAudioData, 0);
            microphoneAudioData.CopyTo(combinedAudioData, gameAudioData.Length);

            // 합쳐진 데이터를 AudioClip에 저장
            AudioClip combinedClip = AudioClip.Create("CombinedAudio", combinedAudioData.Length, 1, 44100, false);
            combinedClip.SetData(combinedAudioData, 0);

            // 서버로 전송
            StartCoroutine(SendAudioToServer(combinedClip));
        }

        private IEnumerator SendAudioToServer(AudioClip clip) {
            byte[] audioData = WavUtility.FromAudioClip(clip);
            WWWForm form = new WWWForm();
            form.AddBinaryData("file", audioData, "combined_audio.wav", "audio/wav");

            using (UnityWebRequest request = UnityWebRequest.Post(apiUrl, form)) {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success) {
                    string responseText = request.downloadHandler.text;
                    Debug.Log("Response: " + responseText);
                    UpdateUI(responseText);
                }
                else {
                    Debug.LogError("Error: " + request.error);
                }
            }
        }

        private void UpdateUI(string responseText) {
            if (resultText != null) {
                resultText.text = "Analysis Result:\n" + responseText;
            }
        }

        // 게임 오디오 데이터를 실시간으로 받아오는 메서드
        void OnAudioFilterRead(float[] data, int channels) {
            // 게임 오디오 데이터를 받아서 gameAudioData에 저장
            System.Array.Copy(data, gameAudioData, data.Length);
        }
    }
}
