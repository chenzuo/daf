using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DAF.Core.IOC;
using DAF.Core.Localization;

namespace DAF.Core
{
    public class Assert
    {
        public static void AreEqual(object expected, object actual, string parameterName = null)
        {
            if (!expected.Equals(actual))
                throw new ArgumentException(Resources.Locale(o => o.Assert_AreEqual, parameterName, expected, actual), parameterName);
        }

        public static void AreNotEqual(object notExpected, object actual, string parameterName = null)
        {
            if (notExpected.Equals(actual))
                throw new ArgumentException(Resources.Locale(o => o.Assert_AreNotEqual, parameterName, notExpected, actual), parameterName);
        }

        public static void AreSame(object expected, object actual, string parameterName = null)
        {
            if (!object.ReferenceEquals(expected, actual))
                throw new ArgumentException(Resources.Locale(o => o.Assert_AreSame, parameterName, expected, actual), parameterName);
        }

        public static void AreNotSame(object notExpected, object actual, string parameterName = null)
        {
            if (object.ReferenceEquals(notExpected, actual))
                throw new ArgumentException(Resources.Locale(o => o.Assert_AreNotSame, parameterName, notExpected, actual), parameterName);
        }

        public static void IsTrue(bool condition, string parameterName = null)
        {
            if (!condition)
                throw new ArgumentException(Resources.Locale(o => o.Assert_IsTrue, parameterName), parameterName);
        }

        public static void IsFalse(bool condition, string parameterName = null)
        {
            if (condition)
                throw new ArgumentException(Resources.Locale(o => o.Assert_IsFalse, parameterName), parameterName);
        }

        public static void IsInstanceOfType(object value, Type expectedType, string parameterName = null)
        {
            if (!expectedType.IsInstanceOfType(value))
                throw new ArgumentException(Resources.Locale(o => o.Assert_IsInstanceOfType, parameterName, expectedType, value.GetType()), parameterName);
        }

        public static void IsNotInstanceOfType(object value, Type wrongType, string parameterName = null)
        {
            if (wrongType.IsInstanceOfType(value))
                throw new ArgumentException(Resources.Locale(o => o.Assert_IsNotInstanceOfType, parameterName, wrongType, value.GetType()), parameterName);
        }

        public static void IsNull(object value, string parameterName = null)
        {
            if (value != null)
                throw new ArgumentException(Resources.Locale(o => o.Assert_IsNull, parameterName), parameterName);
        }

        public static void IsNotNull(object value, string parameterName = null)
        {
            if (value == null)
                throw new ArgumentException(Resources.Locale(o => o.Assert_IsNotNull, parameterName), parameterName);
        }

        public static void IsStringNullOrEmpty(string value, string parameterName = null)
        {
            if (!string.IsNullOrEmpty(value))
                throw new ArgumentException(Resources.Locale(o => o.Assert_IsStringNullOrEmpty, parameterName), parameterName);
        }

        public static void IsStringNotNullOrEmpty(string value, string parameterName = null)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(Resources.Locale(o => o.Assert_IsStringNotNullOrEmpty, parameterName), parameterName);
        }

        public static void IsInRange(int value, int min, int max, string parameterName = null)
        {
            if (!(min <= value && value <= max))
                throw new ArgumentException(Resources.Locale(o => o.Assert_IsInRange, parameterName, value, min, max), parameterName);
        }

        public static void IsNotInRange(int value, int min, int max, string parameterName = null)
        {
            if (min <= value && value <= max)
                throw new ArgumentException(Resources.Locale(o => o.Assert_IsNotInRange, parameterName, value, min, max), parameterName);
        }

        public static void IsInRange(double value, double min, double max, string parameterName = null)
        {
            if (!(min <= value && value <= max))
                throw new ArgumentException(Resources.Locale(o => o.Assert_IsInRange, parameterName, value, min, max), parameterName);
        }

        public static void IsNotInRange(double value, double min, double max, string parameterName = null)
        {
            if (min <= value && value <= max)
                throw new ArgumentException(Resources.Locale(o => o.Assert_IsNotInRange, parameterName, value, min, max), parameterName);
        }

        public static void IsInRange(decimal value, decimal min, decimal max, string parameterName = null)
        {
            if (!(min <= value && value <= max))
                throw new ArgumentException(Resources.Locale(o => o.Assert_IsInRange, parameterName, value, min, max), parameterName);
        }

        public static void IsNotInRange(decimal value, decimal min, decimal max, string parameterName = null)
        {
            if (min <= value && value <= max)
                throw new ArgumentException(Resources.Locale(o => o.Assert_IsNotInRange, parameterName, value, min, max), parameterName);
        }

        public static void EnumValueIsDefined(Type enumType, object value, string parameterName = null)
        {
            if (!Enum.IsDefined(enumType, value))
                throw new ArgumentException(Resources.Locale(o => o.Assert_EnumValueIsDefined, parameterName, enumType, value), parameterName);
        }

        public static void TypeIsAssignableFromType(Type objType, Type fromType, string parameterName = null)
        {
            if (!fromType.IsAssignableFrom(objType))
                throw new ArgumentException(Resources.Locale(o => o.Assert_TypeIsAssignableFromType, parameterName, fromType, objType), parameterName);
        }
    }
}
