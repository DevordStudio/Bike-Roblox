using UnityEngine;

public class TextAnimActivator : MonoBehaviour
{
    void Start()
    {
        TextAnimation anim = GetComponent<TextAnimation>();
        anim.PlayAnim();
    }
}
