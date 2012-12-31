using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Collections
{
    public class TreeNode : IDisposable
    {
        public TreeNode(string key, string caption, object data)
        {
            Key = key;
            Caption = caption;
            Data = data;
            Depth = 0;
            Selected = false;
            Parent = null;
            ChildNodes = new List<TreeNode>();
        }

        public void AddNode(TreeNode node)
        {
            node.Parent = this;
            node.Depth = Depth + 1;
            this.ChildNodes.Add(node);
        }

        public void RemoveNode(TreeNode node)
        {
            node.Parent = null;
            this.ChildNodes.Remove(node);
        }

        public void Clear()
        {
            ChildNodes.ForEach(o => o.Dispose());
            ChildNodes.Clear();
        }

        public void Dispose()
        {
            if (Data != null && Data is IDisposable)
                ((IDisposable)Data).Dispose();
            Parent = null;
            Clear();
        }

        public TreeNode Parent { get; internal set; }
        public List<TreeNode> ChildNodes { get; internal set; }
        public int Depth { get; internal set; }

        public string Key { get; set; }
        public string Value { get; set; }
        public string Caption { get; set; }
        public object Data { get; set; }
        public bool Selected { get; set; }

        public bool IsRoot
        {
            get
            {
                return Parent == null;
            }
        }

        public bool IsLeaf
        {
            get
            {
                return ChildNodes == null || ChildNodes.Count == 0;
            }
        }

        public bool IsFirstChild
        {
            get
            {
                if (Parent != null && Parent.ChildNodes.IndexOf(this) == 0)
                    return true;
                return false;
            }
        }

        public bool IsLastChild
        {
            get
            {
                if (Parent != null && Parent.ChildNodes.IndexOf(this) == Parent.ChildNodes.Count - 1)
                    return true;
                return false;
            }
        }
    }

    public class TreeNode<T> : TreeNode
    {
        public TreeNode(string key, string caption, T data)
            : base(key, caption, data)
        {
        }

        public T Object { get { return (T)Data; } set { Data = value; } }
    }
}
