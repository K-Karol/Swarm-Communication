using UnityEngine;
using System.Collections;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
public class Blockchain {

    public Block[] init_blockchain()
    {
        Block initBlock = new Block(new DateTime(), "0", "init", 0);
        return new Block[] { initBlock };
    }
    public Block[] add_Block(Block[] user_version,string data_para)
    {
        List<Block> working_Version = user_version.ToList<Block>();
        int lastIndex = working_Version.Count() - 1;
        Block lastBlock = working_Version[lastIndex];
        string prev_hash = HashCalc(lastBlock);
        Block newBlock = new Block(DateTime.Now, prev_hash, data_para, lastIndex + 1);
        working_Version.Add(newBlock);
        return working_Version.ToArray<Block>();

    }
    public string HashCalc(Block block)
    {
        SHA256 cal_hash = SHA256.Create();

        XElement block_xml = new XElement("Block",
            new XElement("index", block.index),
            new XElement("time_stamp", block.time_stamp.ToString()),
            new XElement("prev_hash", block.prev_hash),
            new XElement("data", block.data)
            );
        byte[] inputBytes = Encoding.ASCII.GetBytes(block_xml.ToString());
        byte[] outputBytes = cal_hash.ComputeHash(inputBytes);
        return Convert.ToBase64String(outputBytes);

    }

    public int compare_full(Block[] suspicion,Block[] current)
    {
        if(suspicion.Count<Block>() != current.Count<Block>())
        {
            return 0;
        }
        else
        {
            for(int i = 1; i < current.Count<Block>(); i++)
            {
                string suspicious_Hash = HashCalc(suspicion[i]);
                string current_Hash = HashCalc(current[i]);
                if(suspicious_Hash != current_Hash)
                {
                    return 1;
                }
            }
            return 2;
        }
    }
    public int check_chain(Block[] current)
    {
        for(int i = 1; i < current.Count<Block>(); i++) {
            string prev_hash = HashCalc(current[i-1]);
            if(current[i].prev_hash != prev_hash)
            {
                return 1;
            }
        }
        return 0;
    }

}
public class Block
{
    private int ind;
    private DateTime tim;
    private string prev;
    private string dat;


    public int index { get { return ind; } }
    public DateTime time_stamp { get {return tim; } }
    public string prev_hash { get {return prev; } }
    public string data { get {return dat; } }
    public Block(DateTime time_s, string prev_h, string data_para,int ind_p){
        ind = ind_p;
        tim = time_s;
        prev = prev_h;
        dat = data_para;
    }


}






