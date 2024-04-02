using System;
namespace SportNow.Model
{
    public class Objective
    {
        public string id { get; set; }
        public string name { get; set; }
        public string epoca { get; set; }
        public string objectivos { get; set; }

        public override string ToString()
        {
            return name;
        }

    }
}
