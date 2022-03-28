using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace redis_api
{
    //TODO Her hangi objeyi JSON stringine çevirir
    public static class ObjectExtension
    {
        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }

    //TODO Her hangi stringi belirilen objeye çevirir
    public static class StringExtension
    {
        public static T ToObject<T>(this string value)where T : class
        {
            return string.IsNullOrEmpty(value) ? null : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
