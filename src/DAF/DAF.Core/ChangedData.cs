using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.Data;

namespace DAF.Core
{
    public class ChangedData<T>
    {
        public T[] NewItems { get; set; }
        public T[] ModifiedItems { get; set; }
        public T[] DeletedItems { get; set; }
    }

    public static class ChangedDataExtensions
    {
        public static ServerResponse Save<T>(this ChangedData<T> data, Func<ChangedData<T>, bool> action, string msgTrue = null, string msgFalse = null)
        {
            if(string.IsNullOrEmpty(msgTrue))
                msgTrue = LocaleHelper.Localizer.Get("SaveSuccessfully");
            if (string.IsNullOrEmpty(msgFalse))
                msgFalse = LocaleHelper.Localizer.Get("SaveFailure");

            ServerResponse response = new ServerResponse();
            return response.On(() => { return action(data); }, msgTrue, msgFalse);
        }

        public static ServerResponse Save<T>(this T data, Func<T, bool> action, string msgTrue = null, string msgFalse = null)
        {
            if (string.IsNullOrEmpty(msgTrue))
                msgTrue = LocaleHelper.Localizer.Get("SaveSuccessfully");
            if (string.IsNullOrEmpty(msgFalse))
                msgFalse = LocaleHelper.Localizer.Get("SaveFailure");

            ServerResponse response = new ServerResponse();
            return response.On(() => { return action(data); }, msgTrue, msgFalse);
        }
    }
}
