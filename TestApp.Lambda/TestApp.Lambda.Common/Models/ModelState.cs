using System.Collections.Generic;

namespace TestApp.Lambda.Common.Models
{
    public class ModelState
    {
        public Dictionary<string, List<string>> Errors { get; private set; }

        public ModelState()
        {
            this.Errors = new Dictionary<string, List<string>>();
        }

        public void AddModelError(string key, string error)
        {
            if (!this.Errors.ContainsKey(key))
            {
                this.Errors.Add(key, new List<string>());
            }

            this.Errors[key].Add(error);
        }
    }
}
