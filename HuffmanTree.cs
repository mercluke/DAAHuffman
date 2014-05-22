using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{

    //Create a huffman tree for encoding/decoding
    class HuffmanTree
    {
        public HuffmanTreeNode[] leaves;
        MinHeap<HuffmanTreeNode> heap;
        public HuffmanTreeNode head;
        public int count;

        //Constructor
        public HuffmanTree(Dictionary<char, double> freq)
        {
            count = freq.Count;
            leaves = new HuffmanTreeNode[count];
            heap = new MinHeap<HuffmanTreeNode>(count);

            int i = 0;

            foreach(KeyValuePair<char, double> entry in freq)
            {
                HuffmanTreeNode value = new HuffmanTreeNode(entry.Key, entry.Value);
                
                //insert all leaf nodes into leaves array and heap
                leaves[i] = value;
                heap.insert(value);

                i++;
            }

            //Corner case of one distict symbol in frequency table breaking my huffman tree
            if (count == 1)
            {
                //throw new ArgumentException("Frequency table will not build a valid tree with fewer than two entries");
                //what if i try to be lazy? it might work..
                head = new HuffmanTreeNode(null, 0);
                head.childLeft = head.childRight = leaves[0];
                leaves[0].parent = head;
                leaves[0].binaryCode.Append(true);
            }
            //more than one type of symbol, "do you want to build a huff tree?"
            else
            {
                //pull two nodes out, put one back in...
                for (i = 1; i < count; i++)
                {
                    HuffmanTreeNode node = new HuffmanTreeNode(null, 0);
                    node.childLeft = heap.remove();
                    node.childRight = heap.remove();
                    node.childLeft.parent = node;
                    node.childRight.parent = node;
                    node.frequency = node.childLeft.frequency + node.childRight.frequency;
                    heap.insert(node);
                }

                //last node, must be the head
                head = heap.remove();
            }
            
            //Pre-calculate the binary code for each leaf
            foreach(HuffmanTreeNode leaf in leaves)
            {
                HuffmanTreeNode curr = leaf;

                //"started from the bottom now we're here!"
                while(curr != head)
                {
                    if(curr == curr.parent.childLeft)
                    {
                        leaf.binaryCode.Append(false);
                    }
                    else if(curr == curr.parent.childRight)
                    {
                        leaf.binaryCode.Append(true);
                    }
                    curr = curr.parent;
                }

                //we traversed backwards, time to flip this bad boy
                leaf.binaryCode.Reverse();

            } 
        }


    }

    //Tree Node, need a CompareTo function for our heap to make use of
    class HuffmanTreeNode : IComparable<HuffmanTreeNode>
    {
        public char? symbol;
        public double frequency;

        public HuffmanTreeNode parent;
        public HuffmanTreeNode childLeft;
        public HuffmanTreeNode childRight;
        public DAABitArray binaryCode;

        //we want the char symbol to be "null" instead of a char for the non-leaf nodes in tree
        public HuffmanTreeNode(char? inChar, double inDbl)
        {
            symbol = inChar;
            frequency = inDbl;
            binaryCode = new DAABitArray();
        }

        //for minheap to compare nodes with
        public int CompareTo(HuffmanTreeNode otherNode)
        {
            int diff;

            //1 = greater than
            if(frequency > otherNode.frequency)
            {
                diff = 1;
            }
            //0 = same
            else if(frequency == otherNode.frequency)
            {
                diff = 0;
            }
            //-1 - less than
            else
            {
                diff = -1;
            }

            return diff;
        }
    }
}
