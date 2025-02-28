using TMPro;
using UnityEngine;

public class DefaultEnemy : MonoBehaviour{

    public float hp = 100.0f;
    
    [SerializeField]
        public TMP_Text hpText;
    
    public void OnDamage(float damage) {
        hp -= damage;
        hpText.text = "HP: " + hp;
        Debug.Log("적이 " + damage + "만큼의 데미지를 입었습니다.");
        if (hp <= 0) {
            Debug.Log("적이 파괴되었습니다.");
        }
    }
}
