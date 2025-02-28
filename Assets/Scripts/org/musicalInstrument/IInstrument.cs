public interface IInstrument {
    void OnInputReceived(string node);  // 입력을 받았을 때의 동작
    void Init();  // 초기화
}
