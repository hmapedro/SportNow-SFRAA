using System;
using System.Collections.Generic;

namespace SportNow.Model
{
    public class Member
    {
        public Member() { }

        public string id { get; set; }
        public string number_member { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string grade { get; set; }
        public string dojoid { get; set; }
        public string dojo { get; set; }
        public string dojo_faturavel { get; set; }
        public string dojo_seguro { get; set; }
        //public DateTime? birthdate { get; set; }
        public string birthdate { get; set; }
        public DateTime? registrationdate { get; set; }
        public string estado { get; set; }


        public string nif { get; set; }
        public string cc_number { get; set; }
        public string number_fnkp { get; set; }
        public string number_awikp { get; set; }

        public string address { get; set; }
        public string city { get; set; }
        public string postalcode { get; set; }


        public string name_enc1 { get; set; }
        public string mail_enc1 { get; set; }
        public string phone_enc1 { get; set; }
        public string name_enc2 { get; set; }
        public string mail_enc2 { get; set; }
        public string phone_enc2 { get; set; }

        public string member_type { get; set; }
        public string isInstructor { get; set; }
        public string isMonitor { get; set; }
        public string isExaminador{ get; set; }
        public string isTreinador { get; set; }

        public string country { get; set; }
        public string gender { get; set; }
        public string job { get; set; }

        public string emergencyContact { get; set; }
        public string emergencyPhone { get; set; }

        public string consentimento_regulamento { get; set; }

        public string isInstrutorResponsavel { get; set; }
        public string isResponsavelAdministrativo { get; set; }

        //public string image { get; set; }

        public List<Examination> examinations { get; set; }

        public Fee currentFee { get; set; }

        public List<Fee> pastFees { get; set; }

        public List<Member> students { get; set; }
        public List<Member> members_to_approve { get; set; }

        public string treinador_personal { get; set; }
        public string valor_hora_minino { get; set; }
        public string valor_hora_maximo { get; set; }
        public string valor_intervalo { get; set; }
        public string personal_cv { get; set; }
        public string disponibilidade { get; set; }

        public string faturacao_nome { get; set; }
        public string faturacao_morada { get; set; }
        public string faturacao_cidade { get; set; }
        public string faturacao_codpostal { get; set; }
        public string faturacao_nif { get; set; }


        public object imagesourceObject { get; set; }

        public int students_count { get; set; }

        public List<Objective> objectives { get; set; }

        public override string ToString()
            {
                return name;
            }
        }
}
