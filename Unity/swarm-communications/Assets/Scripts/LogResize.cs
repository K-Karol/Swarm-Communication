using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LogResize : MonoBehaviour {

    public Text log_text;
    private LayoutElement layout_element;
    private RectTransform rect;
    public Scrollbar scroll;
    private float thresholdHeight;

    void Start()
    {
        if (log_text == null)
        {
            Debug.LogError("No log Text!");
        }
        else
        {
            rect = this.gameObject.GetComponent<RectTransform>();
            thresholdHeight = rect.rect.height;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (rect.rect.height != log_text.preferredHeight)
        {

            //onto 800
            if (log_text.preferredHeight <= 800f)
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0f);
            }
            else
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0f + (log_text.preferredHeight - 800f));
            }

            if (scroll.value != 0f)
            {
                scroll.value = 0f;
            }

        }

    }
}
