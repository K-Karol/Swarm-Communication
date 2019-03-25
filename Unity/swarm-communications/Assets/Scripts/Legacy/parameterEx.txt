using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class parameterEx : MonoBehaviour {
    public int agentNumber;
    public string communicationMethod;
    public console debug;
    public UnityEngine.Object agentP;
    public GameObject network;
    public Dictionary<int, GameObject> agentsDict = new Dictionary<int, GameObject>();

    // Use this for initialization
    public void initilise (int agN, string com)
    {
        network = GameObject.Find("network");

        debug = GameObject.Find("Debug").GetComponent<console>();
        
        communicationMethod = com;
        agentNumber = agN;

        var agentText = GameObject.Find("agentNO").GetComponent<Text>();
        agentText.text = agentNumber.ToString();

        var commText = GameObject.Find("commMO").GetComponent<Text>();
        commText.text = communicationMethod;

        debug.log("All variables initilised");

        /*
        GameObject ag = Instantiate(agentP,network.transform) as GameObject;
        GameObject ag2 = Instantiate(agentP, network.transform) as GameObject;
        GameObject ag3 = Instantiate(agentP, network.transform) as GameObject;
        agent a = ag.GetComponent<agent>();
        agent a2 = ag2.GetComponent<agent>();
        agent a3 = ag3.GetComponent<agent>();
        a.init(communicationMethod,1);
        a2.init(communicationMethod, 2);
        a3.init(communicationMethod, 3);
        */

        for(int i = 1; i <= agentNumber; i++)
        {
            agentsDict.Add(i, Instantiate(agentP, network.transform) as GameObject);
            agent temp = agentsDict[i].GetComponent<agent>();
            temp.init(communicationMethod, i);
        }
        
    }
}
