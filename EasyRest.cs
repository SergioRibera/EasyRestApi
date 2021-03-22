using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EasyRestApi
{
    public static class EasyRest
    {
        /// <summary>
        /// Ejecuta la url con el método indicado y con los parámetros indicados
        /// </summary>
        /// <param name="url">Direccion URL</param>
        /// <param name="method">Metodo de Ejecución</param>
        /// <param name="parameters">Parametros que recibe la URL (dejar en null si no se requiere)</param>
        /// <returns></returns>
        public static string Excecute(string url, EasyMethod method, RestParams parameters = null)
        {
            return Task.Run(() => {
                switch (method)
                {
                    case EasyMethod.Get:
                        if (parameters == null)
                            return Get(url);
                        else
                            return Get(url, parameters);
                    case EasyMethod.Post:
                        return Post(url, parameters);
                }
                return Get(url);
            }).Result;
        }
        /// <summary>
        /// Ejecuta la url con el método indicado y con los parámetros indicados, devolviendo la respuesta como el objeto indicado
        /// </summary>
        /// <typeparam name="T">Clase que se obtendrá como respuesta</typeparam>
        /// <param name="url">Direccion URL</param>
        /// <param name="method">Metodo de Ejecución</param>
        /// <param name="parameters">Parametros que recibe la URL (dejar en null si no se requiere)</param>
        /// <returns></returns>
        public static T Excecute<T>(string url, EasyMethod method, RestParams parameters = null)
        {
            return Task.Run(() =>
            {
                switch (method)
                {
                    case EasyMethod.Get:
                        if (parameters == null)
                            return Get<T>(url);
                        else
                            return Get<T>(url, parameters);
                    case EasyMethod.Post:
                        return Post<T>(url, parameters);
                }
                return Get<T>(url);
            }).Result;
        }
        
        static string RemoveQuotes(this string s)
        {
            if (s.Contains("\\"))
                s = s.Replace("\\", "");
            return s;
        }
        static string Get(string url)
        {
            string r = "";
            HttpClient client = new HttpClient();
            var rawResponse = client.GetAsync(url).Result;
            if (rawResponse.StatusCode == HttpStatusCode.OK) {
                var s = rawResponse.Content.ReadAsStringAsync().Result;
                r = s.RemoveQuotes();
            }
            return r;
        }
        static string Get(string url, RestParams parameters)
        {
            string r = "";
            url = url + parameters.ProcessedValuesForApi();
            HttpClient client = new HttpClient();
            var rawResponse = client.GetAsync(url).Result;
            if (rawResponse.StatusCode == HttpStatusCode.OK) {
                var s = rawResponse.Content.ReadAsStringAsync().Result;
                r = s.RemoveQuotes();
            }
            return r;
        }
        static T Get<T>(string url)
        {
            string r = "";
            HttpClient client = new HttpClient();
            var rawResponse = client.GetAsync(url).Result;
                var s = rawResponse.Content.ReadAsStringAsync().Result;
                r = s;
            return Deserializar<T>(r);
        }
        static T Get<T>(string url, RestParams parameters)
        {
            string r = "";
            url = url + parameters.ProcessedValuesForApi();
            HttpClient client = new HttpClient();
            var rawResponse = client.GetAsync(url).Result;
                var s = rawResponse.Content.ReadAsStringAsync().Result;
                r = s.RemoveQuotes();
            return Deserializar<T>(r);
        }
        static string Post(string url, RestParams parameters)
        {
            string r = "";
            url = url + parameters.ProcessedValuesForApi();
            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(parameters.ConvertValues());
            var rawResponse = client.PostAsync(url, content).Result;
            if (rawResponse.StatusCode == HttpStatusCode.OK) {
                var s = rawResponse.Content.ReadAsStringAsync().Result;
                r = s.RemoveQuotes();
            } else
                r = rawResponse.Content.ReadAsStringAsync().Result;
            return r;
        }
        static T Post<T>(string url, RestParams parameters)
        {
            string r = "";
            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(parameters.ConvertValues());
            var rawResponse = client.PostAsync(url, content).Result;
            var s = rawResponse.Content.ReadAsStringAsync().Result;
            r = s;
            return Deserializar<T>(r);
        }


        static T Deserializar<T>(this string s)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(s);
            }
            catch (Exception)
            {
                object obj = JsonConvert.DeserializeObject<object>(s);
                T data = default;
                PropertyInfo[] propiedades = obj.GetType().GetProperties();
                for (int i = 0; i < propiedades.Length; i++)
                {
                    for (int x = 0; x < data.GetType().GetProperties().Length; x++)
                    {
                        if (propiedades[i].Name == data.GetType().GetProperties()[x].Name)
                            data.GetType().GetProperty(propiedades[i].Name).SetValue(propiedades[i].GetType(), propiedades[i].GetValue(propiedades[i].GetType(), new object[] { i }), new object[] { x });

                    }
                }
                return data;
            }
        }
    }
}
