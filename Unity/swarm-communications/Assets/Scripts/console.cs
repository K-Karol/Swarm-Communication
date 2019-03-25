using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
public class console : MonoBehaviour {
    public Text debugT;
    public bool ok = false;
	// Use this for initialization
	void Start () {
	    if(debugT == null)
        {
            print("No debug text!");
            ok = true;
        }
	}
    public void log(string mes)
    {
        if(debugT.text.Length > 1500)
        {
            clear();
            string logMes = debugT.text + "\n>" + mes;
            debugT.text = logMes;
        }
        else
        {
            string logMes = debugT.text + "\n>" + mes;
            debugT.text = logMes;
        }
       
    }
    public void queryLog(string mes)
    {
        
        if (debugT.text.Length > 1500)
        {
            clear();
            string logMes = debugT.text + "\n>" + mes;
            debugT.text = logMes;
        }
        else
        {
            string logMes = debugT.text + "\n>" + mes;
            debugT.text = logMes;
        }
    }
    public void sub(string mes)
    {
        if (debugT.text.Length > 1500)
        {
            clear();
            string logMes = debugT.text + "\n>" + mes;
            debugT.text = logMes;
        }
        else
        {
            string logMes = debugT.text + "\n>" + mes;
            debugT.text = logMes;
        }
    }
    private void clear()
    {
        string[] temp = debugT.text.Split('\n');
        List<string> split = temp.ToList();
        split.RemoveRange(0, 2);
        string mod = "LOG LIMIT";
        foreach(string line in split)
        {
            mod += "\n"+line;
        }
        //print(mod);
        debugT.text = mod;
    }

}
