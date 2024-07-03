using System;
namespace SportNow.Model
{


    public class Appointment
    {
        public string id { get; set; }
        public string name { get; set; }
        public string servicename { get; set; }
        public string tipo { get; set; }
        public string date_start { get; set; }
        public string date_end { get; set; }
        public string date_string { get; set; }
        public string duration_hours { get; set; }
        public string duration_minutes { get; set; }
        public string estado { get; set; }
        public string imagem { get; set; }
        public object imagemSource { get; set; }
        public string participationimage { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
