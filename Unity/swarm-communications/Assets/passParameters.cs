using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class passParameters : MonoBehaviour {
    private int agentsNo;
    private bool agT = false;

    private string[] commMethodList = new string[] { "Mesh" };
    private string method;
    private bool meT = false;

    public InputField agentIn;
    public Dropdown commDrop;

    private parameterEx targetOb;
    private bool passed = false;

    public UnityEngine.Object exManager;

    void Start()
    {
        if(exManager == null)
        {
            Debug.Log("Execution manager prefab not loaded!");
        }
    }

    public void Execute()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Int32.TryParse(agentIn.text, out agentsNo))
        {
            agT = true;
            Debug.Log("Agent number was parsed succesfully");

        } else
            Debug.Log("Agent number could not be parsed");

        method = commMethodList[commDrop.value];
        meT = true;

        if (agT == true && meT == true)
        {
            Debug.Log("Loading execution screen");
            SceneManager.LoadScene("execution");
        }

    }
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "execution" && passed == false)
        {
            var exIn = Instantiate(exManager, this.transform) as GameObject;
            passed = true;
            exIn.transform.parent = null;

            targetOb = exIn.GetComponent<parameterEx>();
            print(targetOb);
            targetOb.initilise(agentsNo, method);
        }
    }
}
