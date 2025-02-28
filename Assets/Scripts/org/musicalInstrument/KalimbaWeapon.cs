using System;
using UnityEngine;

public class KalimbaWeapon : MonoBehaviour, IInstrument {
    private float totalTime = 10f; // 전체 제한시간 10초
    private float inputTimeLimit = 2f; // 입력 간 제한시간 2초
    private float currentTime = 0f; // 현재 시간 (카운트용)
    private float lastInputTime = 0f; // 마지막 입력 시간을 기록

    private bool isTimerRunning = false; // 타이머가 작동 중인지 체크하는 변수

    private float weaponDamage = 10.0f; // 무기 데미지

    [SerializeField]
    public GameObject enemy;

    string inputNotes; // 입력된 음계
    string rhythm; // 입력된 박자 (시간 간격에 의해 계산됨)

    private HarmonyChecker harmonyChecker; // HarmonyChecker 인스턴스
    private ChatGPTClient chatGPTClient; // ChatGPTClient 인스턴스

    // Start에서 HarmonyChecker와 ChatGPTClient 인스턴스를 초기화
    void Start() {
        // ChatGPTClient 인스턴스를 직접 생성
        chatGPTClient = new ChatGPTClient();

        // HarmonyChecker 인스턴스를 ChatGPTClient를 전달하면서 생성
        harmonyChecker = new HarmonyChecker(chatGPTClient);

        Init();
    }

    void Update() {
        if (isTimerRunning) {
            currentTime += Time.deltaTime;

            if (currentTime > totalTime || (Time.time - lastInputTime) > inputTimeLimit) {
                harmonyChecker.CheckHarmony(inputNotes, rhythm, OnHarmonyChecked);
                Init();
            }
        }
    }

    public void OnInputReceived(string note) {
        inputNotes += note + " ";
        rhythm += Mathf.Round((Time.time - lastInputTime) * 10f) / 10f + " ";

        if (!isTimerRunning) rhythm = "1 ";

        Debug.Log("입력된 음계: " + inputNotes);
        Debug.Log("입력된 박자: " + rhythm);

        lastInputTime = Time.time;
        currentTime = 0f;
        isTimerRunning = true;
    }

    private void OnHarmonyChecked(float harmonyScore) {
        // enemy가 있을 경우, 데미지를 적용
        enemy.GetComponent<DefaultEnemy>().OnDamage(weaponDamage * harmonyScore);

    }

    public void Init() {
        currentTime = 0f;
        lastInputTime = 0f;
        isTimerRunning = false;

        inputNotes = "";
        rhythm = "";
    }
}
