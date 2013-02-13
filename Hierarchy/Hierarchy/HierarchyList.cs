using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace SmartClasses.Hierarchy
{
    public interface IHierarchyList<TKey, TValue>
    {
        Node<TKey, TValue> Root { get; set; }
    }

    public interface INode<TKey, TValue>
    {
        TKey Key { get; set; }
        TValue Value { get; set; }
        object Tag { get; set; }
        Node<TKey, TValue> Parent { get; set; }
        List<Node<TKey, TValue>> Leafs { get; set; }
    }

    public class HierarchyList<TKey, TValue> : IHierarchyList<TKey, TValue>
    {
        public Node<TKey, TValue> Root { get; set; }

        
        public HierarchyList()
        {
            Root = new Node<TKey,TValue>();
        }
        public HierarchyList(TKey RootKey, TValue RootValue)
            : this()
        {
            Root.Key = RootKey;
            Root.Value = RootValue;
        }

    }
    public class Node<TKey, TValue> : INode<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public object Tag { get; set; }
        public Node<TKey, TValue> Parent { get; set; }
        public List<Node<TKey, TValue>> Leafs { get; set; }
        public bool IsLeaf
        {
            get
            {
                return (Leafs.Count == 0);
            }
        }
        public int Level
        {
            get
            {
                int i = 0;
                Node<TKey,TValue> node = this.Parent;
                while (node != null)
                {
                    node = node.Parent;
                    i++;
                };
                return i;
            }
        }

        public Node<TKey, TValue> AddNode(Node<TKey, TValue> Node)
        {
            Node.Parent = this;
            this.Leafs.Add(Node);
            return Node;
        }
        public Node<TKey, TValue> AddNode(TKey Key, TValue Value, object Tag = null)
        {
            Node<TKey, TValue> newNode = new Node<TKey, TValue>()
            {
                Key = Key,
                Value = Value,
                Tag = Tag
            };
            return AddNode(newNode);
        }

        public List<Node<TKey, TValue>> GetLeafs()
        {
            List<Node<TKey, TValue>> leafs = new List<Node<TKey, TValue>>();
            foreach (var node in Leafs)
            {
                if (node.IsLeaf)
                {
                    leafs.Add(node);
                }
                else
                {
                    leafs.AddRange(node.GetLeafs());
                };
            };
            return leafs;
        }
        public List<Node<TKey, TValue>> GetBranches()
        {
            List<Node<TKey, TValue>> branches = new List<Node<TKey, TValue>>();
            foreach (var node in Leafs)
            {
                if (!node.IsLeaf)
                {
                    branches.Add(node);
                    branches.AddRange(node.GetBranches());
                };
            };
            return branches;
        }

        public Node<TKey, TValue> FindLeaf(TKey Key)
        {
            return GetLeafs().Find(x => x.Key.Equals(Key));
        }
        public Node<TKey, TValue> FindBranch(TKey Key)
        {
            return GetBranches().Find(x => x.Key.Equals(Key));
        }

        public string GetKeyPath(string Separator = "/")
        {
            return getPath(true, Separator);
        }
        public string GetValuePath(string Separator = "/")
        {
            return getPath(false, Separator);
        }

        public override string ToString()
        {
            StringBuilder l_sb = new StringBuilder();
            l_sb.Append("(");
            l_sb.Append((this.Key != null) ? this.Key.ToString() : "null");
            l_sb.Append(") ");
            l_sb.Append((this.Value != null) ? this.Value.ToString() : "null");
            l_sb.Append("\n");
            l_sb.Append(getStringStructure(this));
            return l_sb.ToString();
        }

        private string getPath(bool basedOnKey, string separator = "/")
        {
            StringBuilder l_sb = new StringBuilder();
            Node<TKey, TValue> node = this;
            do 
            {
                string l_itm = String.Empty;
                if(basedOnKey) 
                {
                    l_itm = (node.Key != null) ? node.Key.ToString() : "null";
                }
                else
                {
                    l_itm = (node.Value != null) ? node.Value.ToString() : "null";
                };
                l_sb.Insert(0, String.Format("{0}{1}", separator, l_itm));
                node = node.Parent;
            }
            while (node != null);
            return l_sb.ToString();
        }
        private string getStringStructure(Node<TKey, TValue> startNode)
        {
            StringBuilder l_sb = new StringBuilder();
            foreach (var node in startNode.Leafs)
            {
                l_sb.Append(new String(' ', 3*node.Level));
                l_sb.Append("(");
                l_sb.Append((node.Key != null) ? node.Key.ToString() : "null");
                l_sb.Append(") ");
                l_sb.Append((node.Value != null) ? node.Value.ToString() : "null");
                l_sb.Append("\n");

                if (!node.IsLeaf)
                {
                    l_sb.Append(getStringStructure(node)); ;
                };
            };
            return l_sb.ToString();
        }

        

        
        
        public Node()
        {
            Leafs = new List<Node<TKey, TValue>>();
            Parent = null;
        }
    }
}
