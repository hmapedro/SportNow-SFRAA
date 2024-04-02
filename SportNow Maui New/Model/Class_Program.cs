using System;
using Microsoft.Maui;

namespace SportNow.Model
{
    public class Class_Program
    {
        public string id { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public string classid { get; set; }
        public string classname { get; set; }
        public string assigned_user_id { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
