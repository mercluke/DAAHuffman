using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    class SymbolTable
    {
        public Dictionary<char, double> freq = new Dictionary<char, double>();

        //build up a string to display frequencies in txtFreq
        public String ToString(String symbolString)
        {
            StringBuilder retStr = new StringBuilder();
            
            //remove previous frequency table
            freq.Clear();

            //go through each character
            foreach(char currChar in symbolString)
            {
                //increment existing symbol
                if (freq.ContainsKey(currChar))
                {
                    freq[currChar] += 1;
                }
                //add a new symbol to dictionary
                else 
                {
                    freq[currChar] = 1;
                }
            }

            //build up a string of frequencies to display in txtFreq
            foreach(KeyValuePair<char, double> entry in freq)
            {
                //escape carriage return and linefeed
                if (entry.Key == '\n')
                {
                    retStr.Append("\\n");
                }
                else if(entry.Key == '\r')
                {
                   // retStr.Append("\\r");
                    continue;
                }
                //normal symbol, add it normally
                else
                {
                    retStr.Append(entry.Key);
                }

                //seperate with colon, add frequency count, new line
                retStr.Append(':');
                retStr.Append(entry.Value);
                retStr.Append('\n');
            }

            //return full string
            return retStr.ToString();
        }

        //build up frequency table from contents of txtFreq
        public Dictionary<char, double> getFreq(String freqString)
        {
            //get rid of previous frequency table if present
            freq.Clear();

            //split string by lines
            String[] lines = freqString.Split('\n');

            //loop through each line
            foreach(string currLine in lines)
            {
                /*when i split by newline i get a blank currLine at the end*/
                if(currLine == "")
                {
                    continue;
                }
                
                //split by colon
                String[] values = currLine.Split(':');

                //this counters breaking a symbol that is a colon
                String symbol = String.Join(":", values, 0, values.Length - 1);

                //make sure frequency isn't negative
                if(Double.Parse(values[values.Length-1]) >= 0)
                { 
                    //change "\\n" back to '\n'
                    if(symbol == "\\n")
                    {
                        freq['\n'] = Double.Parse(values[values.Length-1]);
                    }
                    //code removed, ignore  (as per email)
                    ////change "\\r" back to '\r'
                    //else if(symbol == "\\r")
                    //{
                    //    freq['\r'] = Double.Parse(values[values.Length-1]);
                    //}
                    //normal symbol
                    else
                    {
                        freq[symbol[0]] = Double.Parse(values[values.Length-1]);
                    }
                }
                //as per spec, cannot have negative frequency.  would mess with building the huffman tree 
                //(parent being smaller than a child node and being pulled out of heap in wrong order)
                else
                {
                    throw new FormatException("Frequency: " + values[1] + " cannot be less than 0");
                }
            }

            return freq;
        }
    }

}
