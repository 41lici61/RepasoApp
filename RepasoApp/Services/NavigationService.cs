using System;
using System.Collections.Generic;

namespace RepasoApp.Services
{
    public class NavigationService
    {
        private readonly Dictionary<string, Action> _routes = new();

        public void Register(string key, Action showAction)
        {
            _routes[key] = showAction;
        }

        public void Navigate(string key)
        {
            if (_routes.TryGetValue(key, out var action))
            {
                action?.Invoke();
            }
        }
    }
}