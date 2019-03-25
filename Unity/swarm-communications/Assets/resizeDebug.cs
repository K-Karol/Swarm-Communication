using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class resizeDebug : MonoBehaviour {

    public Text logText;
    private LayoutElement lay;
    private RectTransform rectT;
    public Scrollbar scroll;
    private bool textPre = false;
    private float thresholdHeight;

    void Start()
    {
        if(logText == null)
        {
            Debug.Log("No log Text!");
        }
        else
        {
            textPre = true;
            rectT = this.gameObject.GetComponent<RectTransform>();
            thresholdHeight = rectT.rect.height;
        }
    }
    // Update is called once per frame
    void Update () {
        if(rectT.rect.height != logText.preferredHeight)
        {

            //onto 800
            if(logText.preferredHeight <= 800f)
            {
                rectT.sizeDelta = new Vector2(rectT.sizeDelta.x, 0f);
            }
            else
            {
                rectT.sizeDelta = new Vector2(rectT.sizeDelta.x, 0f + (logText.preferredHeight - 800f));
            }

            if (scroll.value != 0f)
            {
                scroll.value = 0f;
            }

        }
        
	}

}
