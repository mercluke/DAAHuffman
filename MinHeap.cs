using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{

    //Run of the mill MinHeap, put things in, pull them out sorted...
    class MinHeap<E> where E : IComparable<E>
    {    
        //size of minheap array
        private int size;
        //currently occupied size
        public int count = 0;
        //array we're storing things in
        private E[] heap;

        //constructor: allocate an array for heap
        public MinHeap(int arrSize)
        {
            size = arrSize;
            heap = new E[size];

        }

        //pull smallest node from the top of the heap
        public E remove()
        {
            //first node is current smallest, this is the one we want
            E retVal = heap[0];
            //swap the last and first node and decrement heap length
            heap[0] = heap[--count];
            
            //index node i'm trickling is at
            int newIndex = 0;
            //index of the child i want to swap with
            int childIndex = chooseChild(newIndex);
            
            
            //make sure the child is in the bounds of the heap, check that child is smaller than node i am trickling down
            while(childIndex < count && heap[newIndex].CompareTo(heap[childIndex]) > 0)
            {
                //swap node with child
                E temp = heap[newIndex];
                heap[newIndex] = heap[childIndex];
                heap[childIndex] = temp;

                //node is now where it's child was
                newIndex = childIndex;
                //figure out which child i want next
                childIndex = chooseChild(newIndex);
            }

            //give back the initial top (smallest value)
            return retVal;
        }


        //place a node at the end of the heap and "trickle up"
        public void insert(E newValue)
        {
            int newIndex = count;

            //insert node at end of the heap and increment count
            heap[count++] = newValue;

            /*trickle up*/
            while(heap[newIndex].CompareTo(heap[parent(newIndex)]) < 0)
            {
                //swap node with it's parent
                E temp = heap[newIndex];
                heap[newIndex] = heap[parent(newIndex)];
                heap[parent(newIndex)] = temp;

                //node is now at parent's index
                newIndex = parent(newIndex);
            }
        }

        //find index of parent
        private int parent(int index)
        {
            return ((index - 1) / 2);
        }

        //find index of left child
        private int leftChild(int index)
        {
            return ((index * 2) + 1);
        }

        //find index of right child
        private int rightChild(int index)
        {
            return ((index * 2) + 2);
        }

        //choose whether to swap with left or right child
        private int chooseChild(int index)
        {
            //is right child even part of the array?  if so, is it smaller than left?  if yes to both, choose right. else, choose left
            return ((rightChild(index) < count) && (heap[leftChild(index)].CompareTo(heap[rightChild(index)]) > 0)) ? rightChild(index) : leftChild(index);
        }
    }


}
