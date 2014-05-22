using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{

    //This class is used to "encode" a bit array to be displayed as a string in the gui
    //or to "decode" compressed text and return a bit array to be uncompressed by the compressor class
    class Encoder
    {
        //our 64 char long alphabet
        private String dictionaryStr = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789\n";


        //transform bit array into encoded text for display
        public String Encode(DAABitArray input)
        {
            StringBuilder output = new StringBuilder();

            //go through bit array one at a time
           for(int i = 0; i < input.NumBits; i+=6)
           {
               int index;

               /*last iteration will cause issues if not a number of bits divisible by 6...*/
               if(input.NumBits - i < 6)
               {
                   //how many bits are remaining after dividing by 6?
                   int pad =  (input.NumBits - i)-1;
                   index = (int)input.GetBitRange(i, i + pad);

                   //bit shift magic to pad with zeros to the right of the number
                   index <<= (5-pad);
               }
               else
               {
                   index = (int)input.GetBitRange(i, i + 5);
               }

               //add current character to the output string
               output.Append(dictionaryStr[index]);
           }

            //return our encoded string to be displayed
            return output.ToString();
        }


        //take an encoded string and return it as a bit array to be decompressed
        public DAABitArray Decode(String input)
        {
            DAABitArray output = new DAABitArray();
            
            Dictionary<char, int> dictionary = new Dictionary<char,int>();
            int i;

            //need some way to access the letter in less than O(n) else decoding will be O(n^2)..
            for(i = 0; i < dictionaryStr.Length; i++)
            {
                //add each letter of dictionaryStr to our dictionary collection for faster [than O(n)] access times
                dictionary[dictionaryStr[i]] = i;
            }
            

            foreach(char currChar in input)
            {
                //append each char to the compressed bit array
                output.Append(dictionary[currChar], 6);
            }
            
            //send the bit array back up to caller (compressor class) to be decompressed
            return output;
        }

    }
}
