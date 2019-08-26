using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jane.Utilities
{
    public class RequestUtilities
    {
        public static string GetHeaders(Dictionary<string, string>.Enumerator dic)
        {
            string response = string.Empty;
            while (dic.MoveNext())
            {
                response += dic.Current.Key + ": " + dic.Current.Value + Environment.NewLine;
            }
            return response;
        }
        public static HttpMethod GetMethod(string method)
        {
            switch (method)
            {
                case "GET":
                    return HttpMethod.GET;
                case "POST":
                    return HttpMethod.POST;
                case "DELETE":
                    return HttpMethod.DELETE;
                case "HEAD":
                    return HttpMethod.HEAD;
                case "OPTION":
                    return HttpMethod.OPTIONS;
                case "PATCH":
                    return HttpMethod.PATCH;
                case "PROPFIND":
                    return HttpMethod.PROPFIND;
                case "PUT":
                    return HttpMethod.PUT;
                default:
                    return HttpMethod.GET;
            }
        }
    }
}
