using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class communications {

    private int agentID;
    private string comms;

    private console debug;
    public link self;
    public List<link> connections = new List<link>();

    public void init(int id,string c)
    {
        agentID = id;
        comms = c;
        self = new link(agentID, comms,this);
        debug = GameObject.Find("Debug").GetComponent<console>();
    }

    public void connect(string targetName)
    {
        debug.log("Attempting connection[ag:"+agentID + ";" + targetName + "]");
        GameObject target = GameObject.Find(targetName);
        agent targetAgentClass = target.GetComponent<agent>();
        communications targetComms = targetAgentClass.comm;
        link targetLink = targetComms.incomingConnection(self);
        if (!(connections.Contains(targetLink)) && targetLink != null)
        {
            connections.Add(targetLink);
            debug.log("Connection successful[ag"+agentID+";"+targetName+"]");
        }
        
    }

    public link incomingConnection(link sender)
    {
        if (!(connections.Contains(sender))) {
        connections.Add(sender);
        return self;
    }
        return null;
    }
}
