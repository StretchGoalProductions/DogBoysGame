using UnityEngine;
using System.Collections;
using System;

public class Cls_Heap<T> where T : IHeapItem<T> {
    // T[] is an array of some type of items (nodes, integers, etc.)
    T[] items;
    int currentItemCount;
    
    // This is the constructor used for creating a new Heap object
    public Cls_Heap(int maxHeapSize) {
        // We need to know the maximum size to create our array because
        // it is difficult to change the size of an array
        items = new T[maxHeapSize];
    }
    
    // Add an item to the list and sort the list
    public void Add(T item) {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    // Remove the first item from the heap
    public T RemoveFirst() {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    // Did an item in the Open Set get updated with a better f cost? Sort it

    public void UpdateItem(T item) {
        SortUp(item);
    }

    // How many items in the heap?
    public int Count {
        get {
            return currentItemCount;
        }
    }

    // Does the heap contain a specific item?
    public bool Contains(T item) {
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T item) {
        while (true) { // while (true) is always true, of course
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount) {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount) {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
                        swapIndex = childIndexRight;
                    }
                }
                if (item.CompareTo(items[swapIndex]) < 0) {
                    Swap (item,items[swapIndex]);
                }
                else {
                    return;
                }
            }
            else {
                return;
            }
        }
    }
    
    void SortUp(T item) {
        int parentIndex = (item.HeapIndex-1)/2;
        while (true) {
            T parentItem = items[parentIndex];
            // CompareTo compares priorities (f costs) of these two given items
            // If f cost less than parent, return 1
            // If equal, return 0
            // If f cost greater than parent, return -1
            if (item.CompareTo(parentItem) > 0) {
                Swap (item,parentItem);
            }
            else {
                break;
            }
            // To find parent index, use n-1/2 where n is equal to index
            parentIndex = (item.HeapIndex-1)/2;
        }
    }
    
    void Swap(T itemA, T itemB) {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

// Using an interface allows the programmer to add new parameters without
// modifying the original items.
// Now we can read (get) or write (set) an item's HeapIndex
public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex {
        get;
        set;
    }
}

