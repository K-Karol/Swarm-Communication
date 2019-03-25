using UnityEngine;
using System.Collections;
using System;

public class agent : MonoBehaviour
{
    public int agID;
    public string commMethod;
    public console debug;
    public communications comm;
    public bool inits = false;
    public Color colour = Color.white;

    public void init(string c, int id)
    {
        commMethod = c;
        agID = id;
        comm = new communications();
        comm.init(agID, commMethod);
        inits = true;
        this.name = "agent:" + agID.ToString();
        debug = GameObject.Find("Debug").GetComponent<console>();

        debug.log(this.name + " was initilised");
    }
    // Update is called once per frame
    void Update()
    {
        if (inits != false)
        {
            if (commMethod == "Mesh")
            {
                foreach (Transform child in this.transform.parent.transform)
                {
                    int tempID;
                    string[] tempName = child.name.Split(':');
                    if (Int32.TryParse(tempName[1], out tempID))
                    {
                        if (tempID != agID)
                        {
                            if (comm.connections.Count != 0)
                            {
                                bool exists = false;
                                //debug.log("not the same");
                                for(int i = 0;i < comm.connections.Count;i++)     //link connection in comm.connections
                                {
                                    if (comm.connections[i].agentID == tempID)
                                    {
                                        exists = true;
                                    }
                                }
                                if(exists == false)
                                {
                                    comm.connect(child.name);
                                }
                            }
                            else
                            {
                                comm.connect(child.name);
                            }
                        }
                    }
                    else
                    {
                        debug.log("ERROR: Could not parse 'tempID' in the Update function, in the subroutine where the child.name is split into the ID and parsed");
                    }
                }
            }
        }
    }
}