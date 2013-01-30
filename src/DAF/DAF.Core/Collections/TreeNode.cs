using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DAF.Core.Collections
{
    [Serializable]
    [DataContract]
    public class TreeNode : IDisposable
    {
        public TreeNode()
        {
            Depth = 0;
            Selected = false;
            Parent = null;
            ChildNodes = new List<TreeNode>();
        }

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

        [IgnoreDataMember]
        public TreeNode Parent { get; internal set; }
        [DataMember]
        public List<TreeNode> ChildNodes { get; internal set; }
        [DataMember]
        public int Depth { get; internal set; }

        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string Caption { get; set; }
        [DataMember]
        public object Data { get; set; }
        [DataMember]
        public bool Selected { get; set; }

        [DataMember]
        public bool IsRoot
        {
            get
            {
                return Parent == null;
            }
        }

        [DataMember]
        public bool IsLeaf
        {
            get
            {
                return ChildNodes == null || ChildNodes.Count == 0;
            }
        }

        [DataMember]
        public bool IsFirstChild
        {
            get
            {
                if (Parent != null && Parent.ChildNodes.IndexOf(this) == 0)
                    return true;
                return false;
            }
        }

        [DataMember]
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
        public TreeNode()
            : base()
        {
        }

        public TreeNode(string key, string caption, T data)
            : base(key, caption, data)
        {
        }

        [IgnoreDataMember]
        public T Object { get { return (T)Data; } set { Data = value; } }
    }
}
