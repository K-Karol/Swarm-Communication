using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Platform : MonoBehaviour {

    public Text port_text;
	void Start () {
        if (port_text == null)
        {
            Debug.LogError("No port text added through inspector");
        }
        else
        {
            if(Application.platform == RuntimePlatform.WindowsEditor)
            {
                port_text.text = "-> Unity Editor | Win";
            } else
                if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                port_text.text = "-> Windows";
            }
            else
            if(Application.platform == RuntimePlatform.Android)
            {
                port_text.text = "-> Android";
            }
            else if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                port_text.text = "-> WebGL";
            }
        }
	}
	
}
