using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Animator anim;
    
    public void InitTxtBox(string contents)
    {
        text.text = contents;
        anim.SetTrigger("Action"); // 텍스트의 페이드인, 페이드아웃을 애니메이션으로 실행
    }
}
