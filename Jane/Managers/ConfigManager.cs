using Jane.Models;
using Jane.Models.Enum;
using Jane.Utilities;
using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Regex = Jane.Models.Regex;

namespace Jane.Managers
{
    public class ConfigManager
    {
        private readonly Config _config;
        public ConfigManager(Config config)
        {
            _config = config;
        }
        public ResponseStatus Send(string username)
        {
            Status status = Status.NoKeyFound;
            string url = string.Empty;
            IDictionary<string, string> variables = new Dictionary<string, string>();
            using (HttpRequest client = new HttpRequest())
            {
                #region Preparing client
                client.IgnoreProtocolErrors = true;
                client.Cookies = new CookieStorage();
                #endregion
                foreach (var request in _config.Requests)
                {
                    #region Preparing Request
                    if (string.IsNullOrEmpty(request.UserAgent))
                        client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
                    else
                        client.UserAgent = request.UserAgent;
                    if (request.Headers != null)
                    {
                        foreach (var header in request.Headers)
                            client.AddHeader(header.Key, Format(header.Value, username, ref variables));                        
                    }
                    if (request.Cookies != null)
                    {
                        foreach (var cookie in request.Cookies)
                            client.Cookies.Add(new System.Net.Cookie(cookie.Key, Format(cookie.Value, username, ref variables), "*/*", TextUtilities.UrlToHost(request.Url)));
                    }
                    #endregion
                    #region Request And Response
                    try
                    {
                        var response = client.Raw(RequestUtilities.GetMethod(request.Method), Format(request.Url, username, ref variables), new StringContent(Format(request.Content, username, ref variables)) { ContentType = request.ContentType });
                        string responseContent = response.ToString();
                        if (request.CaptureRaw)
                            responseContent = $"HTTP/{response.ProtocolVersion.ToString()} {Convert.ToInt32(response.StatusCode)} {response.StatusCode.ToString()}{Environment.NewLine}{RequestUtilities.GetHeaders(response.EnumerateHeaders())}{"Cookie: " + response.Cookies.GetCookieHeader(new Uri(Format(request.Url, username, ref variables)))}{Environment.NewLine}{Environment.NewLine}{responseContent}";
                        #region Getting Variables
                        if (request.GetbetweenVariables != null)
                        {
                            foreach (var x in request.GetbetweenVariables)
                            {
                                var variable = GetVariable(x, responseContent, username, ref variables, out bool found);
                                if (found)
                                    variables.Add(variable);
                            }
                        }
                        if (request.RegexVariables != null)
                        {
                            foreach (var x in request.RegexVariables)
                            {
                                var variable = GetVariable(x, responseContent, username, ref variables, out bool found);
                                if (found)
                                    variables.Add(variable);
                            }
                        }
                        #endregion
                        #region Checking For Keys
                        if (HasKey(responseContent, request.FailureKeys))
                            return new ResponseStatus() { Status = Status.Unsuccessful, Variables = new Dictionary<string, string>(variables), Url = response.Address.ToString() };
                        if (HasKey(responseContent, request.SuccessKeys))
                        {
                            if (status == Status.NoKeyFound)
                            {
                                status = Status.Success;
                                url = response.Address.ToString();
                            }
                        }
                        #endregion
                    }
                    catch
                    {
                        return new ResponseStatus() { Status = Status.RequestFailed, Variables = new Dictionary<string, string>(variables), Url = Format(request.Url, username, ref variables) };

                    }
                    #endregion
                }
            }
            return new ResponseStatus() { Status = status, Variables = new Dictionary<string, string>(variables), Url = url };
        }
        private bool HasKey(string response, List<string> keys)
        {
            if (keys == null)
                return false;
            foreach (var key in keys)
            {
                if (response.Contains(key))
                    return true;
            }
            return false;
        }
        #region Get Variables Regex & Get Between
        private KeyValuePair<string, string> GetVariable(GetBetween source, string response, string username, ref IDictionary<string, string> variables, out bool found)
        {
            string result = TextUtilities.GetStringInBetween(source.Left, source.Right, response, source.IncludeLeft, source.IncludeRight);
            found = true;
            if (string.IsNullOrEmpty(result))
                found = false;
            string match = $"{source.Prefix}{result}{source.Suffix}";
            return new KeyValuePair<string, string>(source.Name, Format(match, username, ref variables));
        }
        private KeyValuePair<string, string> GetVariable(Regex source, string response, string username, ref IDictionary<string, string> variables, out bool found)
        {
            MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(response, source.Pattern);
            string result = string.Join(", ", matches.Cast<Match>().Select(m => m.Value).ToArray());
            found = true;
            if (string.IsNullOrEmpty(result))
                found = false;
            return new KeyValuePair<string, string>(source.Name, Format($"{source.Prefix}{result}{source.Suffix}", username, ref variables));
        }
        #endregion
        public string Format(string text, string username, ref IDictionary<string, string> variables)
        {
            string response = text;
            response = response.Replace("[USERNAME]", username);
            foreach (var key in variables)
            {
                response = response.Replace($"[{key.Key}]", key.Value);
            }
            return response;
        }

    }
}
