using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core
{
    public class LinkedListHelper
    {
        public static LinkedList<T> Build<T>(IEnumerable<T> list, Func<IEnumerable<T>, T> getFirst, Func<IEnumerable<T>, T, T> getNext)
        {
            LinkedList<T> ll = new LinkedList<T>();
            var last = getFirst(list);
            if (last != null)
            {
                LinkedListNode<T> ln = ll.AddFirst(last);
                last = getNext(list, last);
                while (last != null)
                {
                    ln = ll.AddAfter(ln, last);
                    last = getNext(list, last);
                }
            }

            return ll;
        }
    }
}
