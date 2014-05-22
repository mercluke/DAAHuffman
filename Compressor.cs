using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    //This class will accept plain text and a frequency table (as a dictionary collection) and pass a bit array to Encoder class to be represesnted as compressed text
    //Or accept compressed text and a frequency table and pass a string to Encoder class which returns a compressed bit array to be decompressed back to plain text 
    static class Compressor
    {

        //Compress plain text and send bitt array to encoder class
        public static String Compress(String data, Dictionary<char, double> freq)
        {
            DAABitArray rawOutput = new DAABitArray();
            Encoder outStr = new Encoder();
            HuffmanTree tree = new HuffmanTree(freq);

            //go through each character of plain text
            foreach(char currChar in data)
            {
                //have we found desired char yet?
                bool symbolFound = false;

                //strip carriage returns for reasons
                if (currChar != '\r')
                {
                    //search each leaf node in tree for currChar
                    foreach (HuffmanTreeNode leaf in tree.leaves)
                    {
                        //found it!
                        if (leaf.symbol == currChar)
                        {
                            //let program know symbol was found
                            symbolFound = true;
                            //add char's "code" to bit array
                            rawOutput.Append(leaf.binaryCode);
                            //exit foreach early because we already found our symbol
                            break;
                        }
                    }
                    //symbol wasn't in the tree. must be an invalid frequency table
                    if (!symbolFound)
                    {
                        throw new KeyNotFoundException("Symbol: " + currChar + " not found in frequency table.");
                    }
                }
            }

            //send compressed bit array to be encoded as text to display
            return outStr.Encode(rawOutput);
        }

        //Pass compressed text to Encoder class and decompress the returned bit array
        public static String Decompress(String data, Dictionary<char, double> freq)
        {
            Encoder output = new Encoder();
            StringBuilder outStr = new StringBuilder();
            HuffmanTree tree = new HuffmanTree(freq);
            DAABitArray rawInput = output.Decode(data);

            //start from top of the tree
            HuffmanTreeNode curr = tree.head;

            //move through the tree until we hit a leaf
            for(int i = 0; i < rawInput.NumBits; i++)
            {
               /*right*/
                if (rawInput.GetBitAsBool(i))
                {
                    curr = curr.childRight;
                }
                /*left*/
                else
                {
                    curr = curr.childLeft;
                } 
                
                /*at desired leaf, add symbol to string*/
                if (isLeaf(curr))
                {
                    outStr.Append(curr.symbol);
                    //move back to the top of the tree
                    curr = tree.head;
                }
                
            }
           
            //return plain text
            return outStr.ToString();
        }
        
        //this function is useless now, but i kept it for the sake of clarity i guess...
        //i initially made it because i was checking whether both children were null 
        //as a test for node being a leaf until i realised i could check the symbol.
        private static bool isLeaf(HuffmanTreeNode node)
        {
            return (node.symbol != null);
        }
    }

    
}
