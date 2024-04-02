using System;
using Microsoft.Maui;

namespace SportNow.Model
{
    public class Event_Participation
    {
        public string id { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string membername { get; set; }
        public string evento_id { get; set; }
        public string evento_name { get; set; }
        public string evento_data { get; set; }
        public string evento_detailed_date { get; set; }
        public string evento_local { get; set; }
        public string evento_tipo { get; set; }
        public string permite_acompanhantes { get; set; }
        public string evento_website { get; set; }
        public string imagemNome { get; set; }
        public string imagemSource { get; set; }
        public string estado { get; set; }
        public Color estadoTextColor { get; set; } = Colors.White;
        public string entidade { get; set; }
        public string referencia { get; set; }
        public double valor { get; set; }
        public string numero_acompanhantes { get; set; }

    }


}
