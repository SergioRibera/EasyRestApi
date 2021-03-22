using System.Collections.Generic;

namespace EasyRestApi
{
    public enum EasyMethod
    {
        Get,
        Post
    }
    public class RestParams
    {
        List<string> Keys;
        List<object> Values;

        public Dictionary<string, object> ProcesedValues { 
            get {
                Dictionary<string, object> d = new Dictionary<string, object>();
                for (int i = 0; i < Keys.Count; i++)
                    d.Add(Keys[i], Values[i]);
                return d;
            } 
        }

        public RestParams()
        {
            Keys = new List<string>();
            Values = new List<object>();
        }
        public RestParams(List<string> names, List<object> values)
        {
            Keys = new List<string>();
            Values = new List<object>();
            Keys = names;
            Values = values;
        }
        public RestParams(Dictionary<string, object> Parameters)
        {
            Keys = new List<string>();
            Values = new List<object>();
            foreach (var p in Parameters)
            {
                Keys.Add(p.Key);
                Values.Add(p.Value);
            }
        }
        public void ClearParams(){
            Keys.Clear();
            Values.Clear();
        }
        public void AddParam(string name, object value)
        {
            Keys.Add(name);
            Values.Add(value);
        }
        public void AddParam(List<string> names, List<object> values)
        {
            Keys.AddRange(names);
            Values.AddRange(values);
        }
        public void AddParam(Dictionary<string, object> Parameters)
        {
            foreach (var p in Parameters)
            {
                Keys.Add(p.Key);
                Values.Add(p.Value);
            }
        }
    }
}
