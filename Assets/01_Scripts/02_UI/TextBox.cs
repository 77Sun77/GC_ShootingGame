using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Animator anim;
    
    public void InitTxtBox(string contents)
    {
        text.text = contents;
        anim.SetTrigger("Action");
    }
}
