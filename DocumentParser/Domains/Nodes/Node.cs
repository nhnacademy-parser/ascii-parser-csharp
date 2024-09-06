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

        public Node()
        {
        }

        public Node(T value)
        {
            Value = value;
        }

        public Node<T> Append(Node<T> node)
        {
            Node<T> last = GetLast();
            last.Next = node;
            node.Previous = last;

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
            return new NodeIterator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        class NodeIterator : IEnumerator<Node<T>>
        {
            private Node<T> _head;
            private Node<T> _current;

            public NodeIterator(Node<T> head)
            {
                _head = head;
            }

            public bool MoveNext()
            {
                if (_current == null)
                {
                    return (_current = _head) != null;
                }

                return (_current = _current.Next) != null;
            }

            public void Reset()
            {
                _current = _head;
            }

            public Node<T> Current => _current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _head = null;
            }
        }
    }
}