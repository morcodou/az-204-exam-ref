using LogicAppCustomConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicAppCustomConnector.Helpers
{

    public class CallbacksSingleton
    {
        private static readonly CallbacksSingleton m_instance = null;

        private static readonly List<Callback> _callbacks;
        public static CallbacksSingleton Instance
        {
            get
            {
                return m_instance;
            }
        }

        static CallbacksSingleton()
        {
            m_instance = new CallbacksSingleton();
            _callbacks = new List<Callback>();
        }

        private CallbacksSingleton()
        {
        }

        public void AddCallback(Callback callback)
        {
            if (callback != null)
            {
                //avoid duplicates
                Callback callbackToBeAdded = _callbacks
                    .FirstOrDefault(c => Uri.Compare(
                        c.Uri,
                        callback.Uri,
                        UriComponents.AbsoluteUri,
                        UriFormat.Unescaped,
                        StringComparison.CurrentCultureIgnoreCase) == 0);
                if (callbackToBeAdded == null)
                    _callbacks.Add(callback);
            }
        }

        public void ModifyCallback(Callback callback)
        {
            var callbackToModify = _callbacks.SingleOrDefault(b => b.Id.Equals(callback.Id));
            if (callbackToModify != null)
            {

            }
        }

        public IEnumerable<Callback> GetCallbacks()
        {
            return _callbacks;
        }

        public bool DeleteCallbackById(string id)
        {
            bool deleted = false;
            Guid guidToRemove = Guid.Parse(id);
            var callbackToRemove = _callbacks.SingleOrDefault(b => b.Id.Equals(guidToRemove));
            if (callbackToRemove != null)
            {
                _callbacks.Remove(callbackToRemove);
                deleted = true;
            }

            return deleted;
        }

        public Callback GetCallbackById(string id)
        {
            Guid guid = Guid.Parse(id);
            return _callbacks.SingleOrDefault(b => b.Id.Equals(guid));
        }
    }
}