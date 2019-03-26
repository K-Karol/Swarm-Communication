using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Console : MonoBehaviour {

    public Text debug_text; //UI log text box
    public InputField query_input;
    public Button submit_button;

    public Dictionary<string, Func<String[], int>> functions = new Dictionary<string, Func<String[], int>>();


    // Use this for initialization
    void Start()
    {
        if (debug_text == null)
        {
            Debug.Log("Debug text box not passed through the inspector");
        }
        if (query_input == null)
        {
            Debug.Log("query input box not passed through the inspector");
        }
        if (submit_button == null)
        {
            Debug.Log("submit button not passed through the inspector");
        }
        //functions.Add("Help", Help_func);
        //functions.Add("Copyright", Copyright);
        functions.Add("Node", Node_func);
        functions.Add("Visual", Visual_func);

    }
    public void log(string message)
    {
        if (debug_text.text.Length > 1500)
        {
            clear();
            string log_message = debug_text.text + "\n>" + message;
            debug_text.text = log_message;
        }
        else
        {
            string log_message = debug_text.text + "\n>" + message;
            debug_text.text = log_message;
        }

    }
    public void queryLog(string message)
    {

        if (debug_text.text.Length > 1500)
        {
            clear();
            string log_message = debug_text.text + "\n>>>" + message;
            debug_text.text = log_message;
        }
        else
        {
            string log_message = debug_text.text + "\n>>>" + message;
            debug_text.text = log_message;
        }
    }
    public void sub(string message)
    {
        if (debug_text.text.Length > 1500)
        {
            clear();
            string log_message = debug_text.text + "\n   " + message;
            debug_text.text = log_message;
        }
        else
        {
            string log_message = debug_text.text + "\n   " + message;
            debug_text.text = log_message;
        }
    }
    private void clear()    //Clearing log overflow till I sort out a better method to store a big log
    {
        string[] temp = debug_text.text.Split('\n');    //Splitting the log into lines
        List<string> split = temp.ToList();     //converting the split array into a list
        split.RemoveRange(0, 2);        //removes first 2 lines
        string modified = "LOG LIMIT";       //This is the modified log, as a string
        foreach (string line in split)      
        {
            modified += "\n" + line;     
        }
        debug_text.text = modified;
    }
    public void submit_query()
    {
        submit_button.enabled = false;
        submit_button.enabled = true;
        string user_query = query_input.text;
        if (user_query != null || user_query != "")
        {
            parse_command(user_query);
        }
        else
        {
            log("Enter in a command / null");
        }
    }
    void parse_command(string user_query_para)
    {
        string user_query = user_query_para;

        string[] query_split = user_query.Split('.');
        if (functions.ContainsKey(query_split[0]))
        {
            List<string> args_list = new List<string>();
            for (int i = 1; i < query_split.Length; i++)
            {
                args_list.Add(query_split[i]);
            }
            String[] args = args_list.ToArray();
            queryLog(user_query);
            functions[query_split[0]](args);
        }
        else
        {

            log("Command does not exist. Enter 'help' for options");
        }
    }
    public int Node_func(String[] args)
    {

        if (args.Length > 1)
        {
            if (args[1] == "connections")
            {
                string agent_name = args[0];
                GameObject target_agent = GameObject.Find(agent_name);
                if(target_agent != null) {
                    Agent agent_class = target_agent.GetComponent<Agent>();
                    Communications target_communicaitons = agent_class.communications;
                    log("All connections for: " + agent_name);
                    foreach (var connection in target_communicaitons.active_connections)
                    {
                        string temp_output = "Agent:" + connection.Value.id;
                        sub(temp_output);
                    }
                }
                else
                {
                    log("Agent not found");
                }
                
            } else

            /*
            else
            if (args[1] == "colour")
            {
                Dictionary<string, Color> cols = new Dictionary<string, Color>();
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
            */
            if (args[0] == "connect")
            {
                GameObject execution_obj = GameObject.Find("ExecutionManager");
                ParameterExecution execution_class = execution_obj.GetComponent<ParameterExecution>();
                if (args[1] == "all")
                {
                    execution_class.network_Connect("all");
                }
                else if (args[1] == "random")
                {
                    execution_class.network_Connect("random");
                }
            }
            else
            {
                log("Command not found");
            }
        }
        
        else
        {
            if (args[0] == "list")
            {
                log("All agents:");
                GameObject network_obj = GameObject.Find("network");
                foreach (Transform child in network_obj.transform)
                {
                    sub(child.name);
                }
            }
            else
            {
                log("Command does not exist");
            }
            
        }
        
        return 0;
    }
    public int Visual_func(String[] args)
    {
        if(args[0] == "init")
        {
            log("Init. visuals");
            GameObject topology_obj = GameObject.Find("Topology");
            Toplogy top_class = topology_obj.GetComponent<Toplogy>(); //spelt topology wrong in class name. That is a commit for future
            top_class.initialise();
        }
        return 0;
    }
}
