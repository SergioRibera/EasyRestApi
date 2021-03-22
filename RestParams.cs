using System.Collections.Generic;

namespace EasyRestApi
{
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
        public List<KeyValuePair<string, string>> ConvertValues()
        {
            List<KeyValuePair<string, string>> k = new List<KeyValuePair<string, string>>();
            foreach (var p in ProcesedValues)
                k.Add(new KeyValuePair<string, string>(p.Key, (string)p.Value));
            return k;
        }
        public string ProcessedValuesForApi()
        {
            string s = "?";
            foreach (var p in ProcesedValues)
                s += p.Key + "=" + p.Value + "&";
            return s.Remove(s.Length - 1);
        }
        public override string ToString(){
            string cs = "";
            for (int i = 0; i < Keys.Count; i++){
                cs += $"{Keys[i]} => {Values[i]}\n";
            }
            return cs;
        }
        ///
        ///<param name="formatEach">This has format foreach key and value, %k for key and %v for value</param>
        ///
        public string ToString(string formatEach){
            string cs = "";
            formatEach = formatEach ?? "%k => %v";
            for (int i = 0; i < Keys.Count; i++){
                cs += formatEach.Replace("%k", Keys[i]).Replace("%v", (string)Values[i]);
            }
            return cs;
        }
    }
}
