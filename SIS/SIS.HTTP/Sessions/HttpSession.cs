using SIS.HTTP.Common;
using System.Collections.Generic;

namespace SIS.HTTP.Sessions
{
    public class HttpSession : IHttpSession
    {
        private readonly Dictionary<string, object> sessionParameters;

        public HttpSession(string id)
        {
            Validator.ThrowIfNull(id, nameof(id));

            Id = id;
            sessionParameters = new Dictionary<string, object>();
        }

        public string Id { get; }

        public void AddParameter(string name, object parameter)
        {
            Validator.ThrowIfNullOrEmpty(name, nameof(name));
            Validator.ThrowIfNull(parameter, nameof(parameter));
            sessionParameters.Add(name, parameter);
        }

        public void ClearParameters()
        {
            sessionParameters.Clear();
        }

        public bool ContainsParameter(string name)
        {
            Validator.ThrowIfNullOrEmpty(name, nameof(name));
            return sessionParameters.ContainsKey(name);
        }

        public object GetParameter(string name)
        {
            Validator.ThrowIfNullOrEmpty(name, nameof(name));
            return sessionParameters.GetValueOrDefault(name, null);
        }
    }
}
