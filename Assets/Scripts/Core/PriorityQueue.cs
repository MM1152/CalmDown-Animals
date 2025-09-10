using System.Collections;
using System.Collections.Generic;

public class PriorityQueue<T> : IEnumerable<T>
{
    public class Friend
    {
        protected List<T> GetQueue(PriorityQueue<T> target) => target.priority_queue;
    }

    private List<T> priority_queue = new List<T>();
    private IComparer<T> comparer;

    public PriorityQueue(IComparer<T> comparer)
    {
        this.comparer = comparer;
    }

    public void EnQueue(T item)
    {
        priority_queue.Add(item);
        if (priority_queue.Count <= 1) return;

        int curIdx = priority_queue.Count - 1;
        while (curIdx > 0)
        {
            int parentIdx = (curIdx - 1) / 2;

            if (comparer.Compare(priority_queue[curIdx], priority_queue[parentIdx]) < 0)
            {
                T temp = priority_queue[curIdx];
                priority_queue[curIdx] = priority_queue[parentIdx];
                priority_queue[parentIdx] = temp;

                curIdx = parentIdx;
            }

            if (curIdx != parentIdx) break;
        }
    }

    public T Dequeue()
    {
        T returnValue = default(T);
        if (priority_queue.Count == 0) return returnValue;

        returnValue = priority_queue[0];
        priority_queue[0] = priority_queue[priority_queue.Count - 1];
        priority_queue.RemoveAt(priority_queue.Count - 1);

        int curIdx = 0;
        while (curIdx < priority_queue.Count)
        {
            int parentIdx = curIdx;
            int leftIdx = curIdx * 2 + 1;
            int rightIdx = curIdx * 2 + 2;

            if (leftIdx < priority_queue.Count
            && comparer.Compare(priority_queue[leftIdx], priority_queue[parentIdx]) < 0) parentIdx = leftIdx;
            if (rightIdx < priority_queue.Count
            && comparer.Compare(priority_queue[rightIdx], priority_queue[parentIdx]) < 0) parentIdx = rightIdx;

            if (curIdx == parentIdx) break;

            T temp = priority_queue[parentIdx];
            priority_queue[parentIdx] = priority_queue[curIdx];
            priority_queue[curIdx] = temp;

            curIdx = parentIdx;
        }

        return returnValue;
    }

    public void Clear()
    {
        priority_queue.Clear();
    }

    public bool Empty()
    {
        return priority_queue.Count == 0;
    }
    public IEnumerator<T> GetEnumerator()
    {
        return new PriorityQueueEnumer<T>(priority_queue);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class PriorityQueueEnumer<T> : IEnumerator<T>
{
    private List<T> list;
    private int position = -1;

    public T Current => list[position];
    object IEnumerator.Current => Current;

    public PriorityQueueEnumer(List<T> pqlist)
    {
        list = pqlist;
    }

    public void Dispose()
    {
        position = -1;
    }

    public bool MoveNext()
    {
        position++;
        return position < list.Count;
    }

    public void Reset()
    {
        position = -1;
    }
}