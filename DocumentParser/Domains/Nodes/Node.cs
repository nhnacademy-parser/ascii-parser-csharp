using System;
using System.Collections;
using System.Collections.Generic;

namespace DocumentParser.Domains.Nodes
{
    public class Node<T> : IEnumerable<Node<T>>
    {
        private Node<T> Next { get; set; }
        private Node<T> Previous { get; set; }

        public T Value { get; set; }

        public Node<T> Append(Node<T> node)
        {
            Node<T> last = GetLast();
            last.Next = node;
            Next.Previous = last;

            return this;
        }

        public Node<T> GetFirst()
        {
            Node<T> current = this;

            while (current.Previous != null)
            {
                current = current.Previous;
            }

            return current;
        }

        public Node<T> GetLast()
        {
            Node<T> current = this;

            foreach (Node<T> node in this)
            {
                current = node;
            }

            return current;
        }

        public IEnumerator<Node<T>> GetEnumerator()
        {
            Queue<Node<T>> queue = new Queue<Node<T>>();

            Node<T> current = this;

            while (current != null)
            {
                queue.Enqueue(current);
                current = current.Next;
            }

            return queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}