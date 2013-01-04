using System;
using System.Collections.Generic;
using System.Linq;

namespace DAF.Core
{
    public class Resources : ResourceBase<Resources>
    {
        public ConstString Assert_AreEqual = "Assert_AreEqual";
        public ConstString Assert_AreNotEqual = "Assert_AreNotEqual";
        public ConstString Assert_AreSame = "Assert_AreSame";
        public ConstString Assert_AreNotSame = "Assert_AreNotSame";
        public ConstString Assert_IsTrue = "Assert_IsTrue";
        public ConstString Assert_IsFalse = "Assert_IsFalse";
        public ConstString Assert_IsInstanceOfType = "Assert_IsInstanceOfType";
        public ConstString Assert_IsNotInstanceOfType = "Assert_IsNotInstanceOfType";
        public ConstString Assert_IsNull = "Assert_IsNull";
        public ConstString Assert_IsNotNull = "Assert_IsNotNull";
        public ConstString Assert_IsStringNullOrEmpty = "Assert_IsStringNullOrEmpty";
        public ConstString Assert_IsStringNotNullOrEmpty = "Assert_IsStringNotNullOrEmpty";
        public ConstString Assert_IsInRange = "Assert_IsInRange";
        public ConstString Assert_IsNotInRange = "Assert_IsNotInRange";
        public ConstString Assert_EnumValueIsDefined = "Assert_EnumValueIsDefined";
        public ConstString Assert_TypeIsAssignableFromType = "Assert_TypeIsAssignableFromType";

        public ConstString Data_Requried = "Data_Requried";
        public ConstString Data_DataType = "Data_DataType";
        public ConstString Data_EqualsTo = "Data_EqualsTo";
        public ConstString Data_FloatRange = "Data_FloatRange";
        public ConstString Data_IntegerRange = "Data_IntegerRange";
        public ConstString Data_Regex = "Data_Regex";
        public ConstString Data_StringLength = "Data_StringLength";
        public ConstString Data_StringLengthRange = "Data_StringLengthRange";
        public ConstString Data_IdCard = "Data_IdCard";
        public ConstString Data_Email = "Data_Email";
        public ConstString Data_Phone = "Data_Phone";

        public ConstString SaveSuccessfully = "SaveSuccessfully";
        public ConstString SaveFailure = "SaveFailure";
        public ConstString DeleteSuccessfully = "DeleteSuccessfully";
        public ConstString DeleteFailure = "DeleteFailure";

        public ConstString FileNotFound = "FileNotFound";

        public ConstString MessageSendingFailed = "MessageSendingFailed";
    }
}
