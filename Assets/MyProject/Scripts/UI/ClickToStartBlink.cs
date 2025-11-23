using UnityEngine;
using TMPro;

public class ClickToStartBlink : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float speed = 2f;

    void Update()
    {
        float alpha = Mathf.Abs(Mathf.Sin(Time.time * speed));
        text.alpha = alpha;
    }
}
