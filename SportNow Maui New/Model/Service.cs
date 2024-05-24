
namespace SportNow.Model
{


    public class Service
    {
        public string id { get; set; }
        public string nome { get; set; }
        public string tipo { get; set; }
        public string imagem { get; set; }
        public object imagemSource { get; set; }

        public override string ToString()
        {
            return nome;
        }
    }
}
