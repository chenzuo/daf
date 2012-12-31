using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core
{
    public class ConstString
    {
        public ConstString(string value)
        {
            this.Value = value;
        }

        public static implicit operator string(ConstString item)
        {
            return item.Value;
        }

        public static implicit operator ConstString(string value)
        {
            ConstString obj = new ConstString(value);
            return obj;
        }

        public string Value { get; private set; }
    }

    public class ConstInt
    {
        public ConstInt(int value)
        {
            this.Value = value;
        }

        public static implicit operator int(ConstInt item)
        {
            return item.Value;
        }

        public static implicit operator ConstInt(int value)
        {
            ConstInt obj = new ConstInt(value);
            return obj;
        }

        public int Value { get; private set; }
    }

    public class ConstBool
    {
        public ConstBool(bool value)
        {
            this.Value = value;
        }

        public static implicit operator bool(ConstBool item)
        {
            return item.Value;
        }

        public static implicit operator ConstBool(bool value)
        {
            ConstBool obj = new ConstBool(value);
            return obj;
        }

        public bool Value { get; private set; }
    }

    public class ConstItem<T>
    {
        public ConstItem(T value)
        {
            this.Value = value;
        }

        public static implicit operator T(ConstItem<T> item)
        {
            return item.Value;
        }

        public static implicit operator ConstItem<T>(T value)
        {
            ConstItem<T> obj = new ConstItem<T>(value);
            return obj;
        }

        public T Value { get; private set; }
    }
}
