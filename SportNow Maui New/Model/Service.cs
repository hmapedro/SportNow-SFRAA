
namespace SportNow.Model
{


    public class Service
    {
        public string id { get; set; }
        public string nome { get; set; }
        public string tipo { get; set; }
        public string imagem { get; set; }
        public object imagemSource { get; set; }
        public string descricao { get; set; }
        public string responsavel { get; set; }
        public string responsavel_email { get; set; }
        public string valores { get; set; }
        public string horario { get; set; }
        public string local { get; set; }
        public string observacoes { get; set; }
        public string nickname { get; set; }
        public string aula_nome { get; set; }
        public string tipo_evento { get; set; }

        public override string ToString()
        {
            return nome;
        }
    }
}
