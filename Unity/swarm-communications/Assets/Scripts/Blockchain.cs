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
        Block initBlock = new Block(new DateTime(), "0", "init", 0,"0");
        return new Block[] { initBlock };
    }
    public Block[] add_Block(Block[] user_version, string data_para, DateTime dateTime, string hash)
    {
        List<Block> working_Version = user_version.ToList<Block>();
        int lastIndex = working_Version.Count() - 1;
        Block lastBlock = working_Version[lastIndex];
        string prev_hash = HashCalc(lastBlock);
        Block newBlock = new Block(dateTime, prev_hash, data_para, lastIndex + 1,hash);
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
    //public Block[] attatch_block(Block[] user_version,Block block)
    //{
    //    List<Block> working_version = user_version.ToList<Block>();
    //    int lastIndex = working_version.Count() - 1;
    //    string prev_hash = HashCalc(working_version[lastIndex]);
    //    Block newBlock = new Block(block.time_stamp, prev_hash, block.data, block.index);
    //    working_version.Add(newBlock);
    //    return working_version.ToArray<Block>();

    //}

    //public int compare_full(Block[] suspicion,Block[] current)
    //{
    //    if(suspicion.Count<Block>() != current.Count<Block>())
    //    {
    //        return 0;   //not the same length
    //    }
    //    else
    //    {
    //        for(int i = 1; i < current.Count<Block>(); i++)
    //        {
    //            string suspicious_Hash = HashCalc(suspicion[i]);
    //            string current_Hash = HashCalc(current[i]);
    //            if(suspicious_Hash != current_Hash)
    //            {
    //                return 1;   // change detected
    //            }
    //        }
    //        return 2;   //same chain
    //    }
    //}
    //public int[] check_chain(Block[] current)
    //{
    //    List<int> change = new List<int>() { 0 };
    //    for(int i = 1; i < current.Count<Block>(); i++) {
    //        string prev_hash = HashCalc(current[i-1]);
    //        if(current[i].prev_hash != prev_hash)
    //        {
    //            change.Add(i - 1);
    //        }
    //    }
    //    if(change.Count > 1)
    //    {
    //        change[0] = 1;
    //        return change.ToArray<int>();
    //    }
    //    else
    //    {
    //        return change.ToArray<int>();
    //    }

    //}
    //public int difference(Block[] suspicion, Block[] current)
    //{
    //    if (suspicion.Count<Block>() == current.Count<Block>())
    //    {
    //        return 1; //same length
    //    }
    //    else if (suspicion.Count<Block>() > current.Count<Block>())
    //    {
    //        //possible update.
    //        Block[] core = suspicion.Slice<Block>(0, current.Count<Block>());
    //        Block[] extra = suspicion.Slice<Block>(current.Count<Block>(), suspicion.Count<Block>()); //will include one of the previous blocks for hash checking
    //        int[] change = check_chain(core);
    //        int compare = compare_full(core, current);
    //        if (change[0] == 1)
    //        {
    //            return 2; //the sender chain is corrupted so it will not be accepted and the host instance version will be enforced
    //        } else if (compare == 1 && change[0] != 1)
    //        {

    //        }
    //        else
    //        {

    //            int[] check = check_chain(extra);
    //            if (check[0] == 1)
    //            {
    //                return 3; //the extra blocks are not linked with the rest so they were modified/corrupted by a third party so it is rejected.
    //            }
    //            else
    //            {
    //                return 4; //This instance can accept the new blockchain in whole
    //            }
    //        }
    //    } else if (suspicion.Count<Block>() < current.Count<Block>())
    //    {
    //        //the sender is trying to push a chain that isn't updated in accordance to this instance, or simply 
    //    }
    //}

}
public class Block
{
    private int ind;
    private DateTime tim;
    private string prev;
    private string dat;
    private string hash;

    public int index { get { return ind; } }
    public DateTime time_stamp { get {return tim; } }
    public string prev_hash { get {return prev; } }
    public string data { get {return dat; } }
    public string Hash { get { return hash; } }
    public Block(DateTime time_s, string prev_h, string data_para,int ind_p,string ha){
        ind = ind_p;
        tim = time_s;
        prev = prev_h;
        dat = data_para;
        hash = ha;
    }
    public Block(DateTime time_s,string data_para)
    {
        dat = data_para;
        tim = time_s;
        hash = HashC();
    }
    private string HashC()
    {
        SHA256 cal_hash = SHA256.Create();

        XElement block_xml = new XElement("Block",
            new XElement("time_stamp", time_stamp.ToString()),
            new XElement("data", data)
            );
        byte[] inputBytes = Encoding.ASCII.GetBytes(block_xml.ToString());
        byte[] outputBytes = cal_hash.ComputeHash(inputBytes);
        return Convert.ToBase64String(outputBytes);
    }

}




public static class Extensions
{
    /// <summary>
    /// Get the array slice between the two indexes.
    /// ... Inclusive for start index, exclusive for end index.
    /// </summary>
    public static T[] Slice<T>(this T[] source, int start, int end)
    {
        // Handles negative ends.
        if (end < 0)
        {
            end = source.Length + end;
        }
        int len = end - start;

        // Return new array.
        T[] res = new T[len];
        for (int i = 0; i < len; i++)
        {
            res[i] = source[i + start];
        }
        return res;
    }
}

