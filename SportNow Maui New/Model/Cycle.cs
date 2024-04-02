using System;
using Microsoft.Maui;

namespace SportNow.Model
{
    public class Cycle
    {
        public string id { get; set; }
        public string name { get; set; }
        public string tipo { get; set; }
        public string objetivos { get; set; }
        public string data_inicio { get; set; }
        public string data_fim{ get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
