using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.Collections;

namespace DAF.Core
{
    public class HierarchyHelper
    {
        public static void DoDescendants<T>(IEnumerable<T> tree, Func<T, IEnumerable<T>> getChildren, Predicate<T> isMatch, Action<T> action)
        {
            if (tree == null || tree.Count() <= 0)
                return;
            Stack<T> st = new Stack<T>();
            tree.ForEach((o, i) => st.Push(o));
            while (st.Count() > 0)
            {
                var stItem = st.Pop();
                if (isMatch(stItem))
                {
                    action(stItem);
                }
                var items = getChildren(stItem);
                if (items != null && items.Count() > 0)
                {
                    items.ForEach((o, i) => st.Push(o));
                }
            }
        }

        public static ICollection<T> FindDescendants<T>(IEnumerable<T> tree, Func<T, IEnumerable<T>> getChildren, Predicate<T> isMatch)
        {
            if (tree == null || tree.Count() <= 0)
                return null;
            List<T> matchedItems = new List<T>();
            Stack<T> st = new Stack<T>();
            tree.ForEach((o, i) => st.Push(o));
            while (st.Count() > 0)
            {
                var stItem = st.Pop();
                if (isMatch(stItem))
                {
                    matchedItems.Add(stItem);
                }
                var items = getChildren(stItem);
                if (items != null && items.Count() > 0)
                {
                    items.ForEach((o, i) => st.Push(o));
                }
            }

            return matchedItems;
        }

        public static T FindDescendant<T>(IEnumerable<T> tree, Func<T, IEnumerable<T>> getChildren, Predicate<T> isMatch)
        {
            if (tree == null || tree.Count() <= 0)
                return default(T);

            Stack<T> st = new Stack<T>();
            tree.ForEach((o, i) => st.Push(o));
            while (st.Count() > 0)
            {
                var stItem = st.Pop();
                if (isMatch(stItem))
                {
                    return stItem;
                }
                var items = getChildren(stItem);
                if (items != null && items.Count() > 0)
                {
                    items.ForEach((o, i) => st.Push(o));
                }
            }

            return default(T);
        }

        public static void DoAncestors<T>(IEnumerable<T> tree, Func<T, T> getParent, Predicate<T> isMatch, Action<T> action)
        {
            if (tree == null || tree.Count() <= 0)
                return;
            foreach (var item in tree)
            {
                T parent = item;
                while (parent != null)
                {
                    if (isMatch(parent))
                        action(parent);

                    parent = getParent(parent);
                }
            }
        }

        public static ICollection<T> FindAncestors<T>(IEnumerable<T> tree, Func<T, T> getParent, Predicate<T> isMatch)
        {
            if (tree == null || tree.Count() <= 0)
                return null;
            List<T> matchedItems = new List<T>();
            foreach (var item in tree)
            {
                T parent = item;
                while (parent != null)
                {
                    if (isMatch(parent))
                        matchedItems.Add(parent);

                    parent = getParent(parent);
                }
            }

            return matchedItems;
        }

        public static T FindAncestor<T>(IEnumerable<T> tree, Func<T, T> getParent, Predicate<T> isMatch)
        {
            if (tree == null || tree.Count() <= 0)
                return default(T);

            foreach (var item in tree)
            {
                T parent = item;
                while (parent != null)
                {
                    if (isMatch(parent))
                        return parent;

                    parent = getParent(parent);
                }
            }

            return default(T);
        }

        public static ICollection<TT> Build<TF, TT>(IEnumerable<TF> roots, Func<TF, TT> cast, Func<TF, IEnumerable<TF>> getChildren, Action<TT, TT> addChild)
        {
            List<TT> tree = new List<TT>();

            Stack<Tuple<TT, TF>> st = new Stack<Tuple<TT, TF>>();
            foreach (var item in roots)
            {
                TT root = cast(item);
                tree.Add(root);
                st.Push(new Tuple<TT, TF>(root, item));
            }

            while (st.Count() > 0)
            {
                var parent = st.Pop();
                var children = getChildren(parent.Item2);
                if (children != null)
                {
                    foreach (var child in children)
                    {
                        TT childNode = cast(child);
                        addChild(parent.Item1, childNode);
                        st.Push(new Tuple<TT, TF>(childNode, child));
                    }
                }
            }

            return tree;
        }

        public static void Singlize<T>(IEnumerable<T> tree, Func<T, IEnumerable<T>> getChildren, Func<T, T, bool> isMatch, Action<T, T> addChild, Action<IEnumerable<T>, T> removeChild)
        {
            if (tree == null || tree.Count() <= 0)
                return;

            Stack<T> st = new Stack<T>();

            int j = tree.Count();
            for (int i = 0; i < j; i++)
            {
                var item = tree.ElementAt(i);
                for (int k = i + 1; k < j; k++)
                {
                    var node = tree.ElementAt(k);
                    if (isMatch(node, item))
                    {
                        var children = getChildren(node);
                        if (children != null && children.Count() > 0)
                        {
                            foreach (var child in children)
                                addChild(item, child);
                        }
                        removeChild(tree, node);
                        j--;
                        k--;
                    }
                }
                st.Push(item);
            }

            while (st.Count > 0)
            {
                var parent = st.Pop();
                var items = getChildren(parent);
                if (items != null && items.Count() > 0)
                {
                    j = items.Count();
                    for (int i = 0; i < j; i++)
                    {
                        var item = items.ElementAt(i);
                        for (int k = i + 1; k < j; k++)
                        {
                            var node = items.ElementAt(k);
                            if (isMatch(node, item))
                            {
                                var children = getChildren(node);
                                if (children != null && children.Count() > 0)
                                {
                                    foreach (var child in children)
                                        addChild(item, child);
                                }
                                removeChild(items, node);
                                j--;
                                k--;
                            }
                        }
                        st.Push(item);
                    }
                }
            }
        }

        public static IEnumerable<T> Merge<T>(IEnumerable<T> tree1, IEnumerable<T> tree2, Func<T, IEnumerable<T>> getChildren, Func<T, T, bool> isMatch, Action<T, T> addChild, Action<IEnumerable<T>, T> removeChild)
        {
            if (tree1 == null || tree1.Count() <= 0)
                return tree2;
            if (tree2 == null || tree2.Count() <= 0)
                return tree1;

            List<T> tree = new List<T>();
            tree.AddRange(tree1);
            tree.AddRange(tree2);

            Singlize(tree, getChildren, isMatch, addChild, removeChild);

            return tree;
        }

        public static ICollection<TreeNode<T>> BuildT<T>(IEnumerable<T> roots, Func<T, string> getKey, Func<T, string> getCaption, Func<T, IEnumerable<T>> getChildren)
        {
            Func<T, TreeNode<T>> cast = o =>
                {
                    TreeNode<T> node = new TreeNode<T>(getKey(o), getCaption(o), o);
                    return node;
                };

            Action<TreeNode<T>, TreeNode<T>> addChild = (p, c) => p.AddNode(c);

            return Build<T, TreeNode<T>>(roots, cast, getChildren, addChild);
        }

        public static ICollection<TreeNode> Build<T>(IEnumerable<T> roots, Func<T, string> getKey, Func<T, string> getCaption, Func<T, IEnumerable<T>> getChildren)
        {
            Func<T, TreeNode> cast = o =>
            {
                TreeNode node = new TreeNode(getKey(o), getCaption(o), o);
                return node;
            };

            Action<TreeNode, TreeNode> addChild = (p, c) => p.AddNode(c);

            return Build<T, TreeNode>(roots, cast, getChildren, addChild);
        }
    }
}
