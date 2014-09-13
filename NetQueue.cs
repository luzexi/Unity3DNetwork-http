

using System;
using System.Collections.Generic;

//  NetQueue.cs
//  Author: Lu Zexi
//  Time: 2013-11-12


namespace Game.Network
{

    /// <summary>
    /// 网络队列类
    /// </summary>
    public sealed class NetQueue<T>
    {
        private readonly object m_cLock; //关键锁
        private T[] m_lstItems; //队列
        private int m_iSize;    //大小
        private int m_iHead;    //队列头

        public int Size
        {
            get { return this.m_iSize; }
        }
        public int Capacity
        {
            get { return this.m_lstItems.Length; }
        }

        public NetQueue(int capacity)
        {
            this.m_cLock = new object();
            this.m_lstItems = new T[capacity];
        }

        /// <summary>
        /// 设置新容量
        /// </summary>
        /// <param name="newSize"></param>
        private void SetCapacity(int newSize)
        {
            if (this.m_iSize == 0)
            {
                this.m_lstItems = new T[newSize];
                this.m_iHead = 0;
                return;
            }

            T[] newlst = new T[newSize];

            if (this.m_iHead + this.m_iSize - 1 < this.m_lstItems.Length)
            {
                Array.Copy(this.m_lstItems, this.m_iHead, newlst, 0, this.m_iSize);
            }
            else
            {
                Array.Copy(this.m_lstItems, this.m_iHead, newlst, 0, this.m_lstItems.Length - this.m_iHead);
                Array.Copy(this.m_lstItems, 0, newlst, this.m_lstItems.Length - this.m_iHead, (this.m_iSize - (this.m_lstItems.Length - this.m_iHead)));
            }

            this.m_lstItems = newlst;
            this.m_iHead = 0;
        }

        /// <summary>
        /// 清除队列
        /// </summary>
        public void Clear()
        {
            lock (this.m_cLock)
            {
                for (int i = 0; i < this.m_lstItems.Length; i++)
                    this.m_lstItems[i] = default(T);
                this.m_iHead = 0;
                this.m_iSize = 0;
            }
        }

        /// <summary>
        /// 推入队列
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            lock (this.m_cLock)
            {
                if (this.m_iSize == this.m_lstItems.Length)
                    SetCapacity(this.m_lstItems.Length + 8);

                int index = (this.m_iHead + this.m_iSize) % this.m_lstItems.Length;
                this.m_lstItems[index] = item;
                this.m_iSize++;
            }
        }

        /// <summary>
        /// 推入所有元素
        /// </summary>
        /// <param name="items"></param>
        public void Enqueue(IEnumerable<T> items)
        {
            lock (this.m_cLock)
            {
                foreach (var item in items)
                {
                    if (this.m_iSize == this.m_lstItems.Length)
                    {
                        SetCapacity(this.m_lstItems.Length + 8);
                        int index = (this.m_iHead + this.m_iSize) % this.m_lstItems.Length;
                        this.m_lstItems[index] = item;
                        this.m_iSize++;
                    }
                }
            }
        }

        /// <summary>
        /// 推入元素到头队列
        /// </summary>
        /// <param name="item"></param>
        public void EnqueueFront(T item)
        {
            lock (this.m_cLock)
            {
                if (this.m_iSize >= this.m_lstItems.Length)
                    SetCapacity(this.m_lstItems.Length + 8);

                this.m_iHead--;
                if (this.m_iHead < 0)
                    this.m_iHead = this.m_lstItems.Length - 1;
                this.m_lstItems[this.m_iHead] = item;
                this.m_iSize++;
            }
        }

        /// <summary>
        /// 推出元素
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Dequeue(out T item)
        {
            if (this.m_iSize == 0)
            {
                item = default(T);
                return false;
            }

            lock (this.m_cLock)
            {
                item = this.m_lstItems[this.m_iHead];
                this.m_lstItems[this.m_iHead] = default(T);
                this.m_iHead = (this.m_iHead + 1) % this.m_lstItems.Length;
                this.m_iSize--;
                return true;
            }
        }

        /// <summary>
        /// 将所有元素推出
        /// </summary>
        /// <param name="addTo"></param>
        /// <returns></returns>
        public int DequeueAll(IList<T> addTo)
        {
            if (this.m_iSize == 0)
                return 0;

            lock (this.m_cLock)
            {
                int added = this.m_iSize;
                while (this.m_iSize > 0)
                {
                    var item = this.m_lstItems[this.m_iHead];
                    addTo.Add(item);
                    this.m_lstItems[this.m_iHead] = default(T);
                    this.m_iHead = (this.m_iHead + 1) % this.m_lstItems.Length;
                    this.m_iSize--;
                }
                return added;
            }
        }

        /// <summary>
        /// 获取指定偏移量的元素
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public T Peek(int offset)
        {
            if (this.m_iSize == 0)
                return default(T);

            lock (this.m_cLock)
            {
                return this.m_lstItems[(this.m_iHead + offset) % this.m_lstItems.Length];
            }
        }

        /// <summary>
        /// 是否存在指定元素
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contain(T item)
        {
            lock (this.m_cLock)
            {
                int ptr = this.m_iHead;
                for (int i = 0; i < this.m_iSize; i++)
                {
                    if (this.m_lstItems[ptr] == null)
                    {
                        if (item == null)
                            return true;
                    }
                    else
                    {
                        if (this.m_lstItems[ptr].Equals(item))
                            return true;
                    }
                    ptr = (ptr + 1) % this.m_lstItems.Length;
                }
                return false;
            }
        }

        /// <summary>
        /// 获取所有元素
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            lock (this.m_cLock)
            {
                T[] res = new T[this.m_iSize];
                int ptr = this.m_iHead;
                for (int i = 0; i < this.m_iSize; i++)
                {
                    res[i] = this.m_lstItems[ptr++];
                    if (ptr >= this.m_lstItems.Length)
                        ptr = 0;
                }
                return res;
            }
        }

    }


}