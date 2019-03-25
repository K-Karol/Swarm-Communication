using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class button : MonoBehaviour
{
    public console debug;
    public Dictionary<string, Func<String[], int>> functions = new Dictionary<string, Func<String[], int>>();
    void Start()
    {
        debug = this.GetComponentInParent<console>();
        functions.Add("help", helpF);
        functions.Add("Copyright", copy);
        functions.Add("node", accessAgents);
        functions.Add("visual",visual);
    }

    // Use this for initialization
    public void res()
    {
        this.GetComponent<Button>().enabled = false;
        this.GetComponent<Button>().enabled = true;
        debug.log("Refreshed button");
    }
    public void submit()
    {
        this.GetComponent<Button>().enabled = false;
        this.GetComponent<Button>().enabled = true;
        InputField input = GameObject.Find("InputField").GetComponent<InputField>();
        string userInput = input.text;
        if (userInput != null || userInput != "")
        {
            parseCommand(userInput);
        }

    }
    public void parseCommand(string userInput)
    {
        string[] splitCommand = userInput.Split('.');
        if (functions.ContainsKey(splitCommand[0]))
        {
            List<string> test = new List<string>();
            for (int i = 1; i < splitCommand.Length; i++)
            {
                test.Add(splitCommand[i]);
            }
            String[] args = test.ToArray();
            debug.queryLog(userInput);
            functions[splitCommand[0]](args);
        }
        else
        {
            debug.log("Command does not exist. Enter 'help' for options");
        }
    }
    public int helpF(String[] args)
    {
        debug.log("Funtions:");
        debug.sub("Copyright => Display copyright and owner information.");
        debug.sub("node => Access agent commands/properties.\nArgs: .name -> .links");
        return 0;
    }
    public int copy(String[] args)
    {
        debug.log("Karol Kierzkowski | K72 ; karol.kierzkowski72@gmail.com ; +44 7728341100");
        return 0;
    }
    public int accessAgents(String[] args) {

        
        if (args.Length > 1)
        {
            if (args[1] == "links")
            {
                string agentName = args[0];
                GameObject agentTarget = GameObject.Find(agentName);
                agent targetClass = agentTarget.GetComponent<agent>();
                communications targetComms = targetClass.comm;
                debug.log("All connections for:" + agentName);
                for (int i = 0; i < targetComms.connections.Count; i++)
                {
                    string temp = "agent" + targetComms.connections[i].agentID;
                    debug.sub(temp);
                }
            } else 
            if(args[1] == "colour")
            {
                Dictionary<string,Color> cols = new Dictionary<string, Color>();
                cols.Add("white", Color.white);
                cols.Add("black", Color.black);
                cols.Add("green", Color.green);
                if (cols.ContainsKey(args[2]))
                {
                    string agentName = args[0];
                    GameObject agentTarget = GameObject.Find(agentName);
                    agent targetClass = agentTarget.GetComponent<agent>();
                    targetClass.colour = cols[args[2]];
                    GameObject top = GameObject.Find("Topology");
                    topology topClass = top.GetComponent<topology>();
                    //topClass.refresh(agentName);
                }
            }
            else
            {
                debug.log("Command not found");
            }
        }
        else
        {
            if(args[0] == "list")
            {
                debug.log("All agents:");
                GameObject net = GameObject.Find("network");
                foreach (Transform child in net.transform)
                {
                    debug.sub(child.name);
                }
            }
            else
            {
                debug.log("Command not found");
            }
        }

        return 0;
    }
    public int visual(String[] args)
    {
        if(args[0] == "init")
        {
            debug.log("Init. visuals");
            GameObject top = GameObject.Find("Topology");
            topology topClass = top.GetComponent<topology>();
            topClass.init();
        }
        return 0;
    }

}