using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Maui;

namespace SportNow.Model.Charts
{
	public class Attendance_Stats
	{

		public List<Attendance_Stat> Data { get; set; }


		public Attendance_Stats(List<Class_Schedule> class_schedules)
		{
            List<Attendance_Stat> Data_aux = new List<Attendance_Stat>();
			Attendance_Stat attendance_stat_all = new Attendance_Stat("Todas");
            Data_aux.Add(attendance_stat_all);
			foreach (Class_Schedule class_schedule in class_schedules)
			{
				Attendance_Stat attendance_stat = checkTypeExists(Data_aux, class_schedule.name);
				if (class_schedule.classattendancestatus == "fechada")
				{
					attendance_stat.class_count_presente++;
					attendance_stat_all.class_count_presente++;
				}
				else
				{
					attendance_stat.class_count_ausente++;
					attendance_stat_all.class_count_ausente++;
				}
				attendance_stat.class_count_total++;
				attendance_stat_all.class_count_total++;
			}

			foreach (Attendance_Stat attendance_stat in Data_aux)
			{
				attendance_stat.attendance_percentage = (attendance_stat.class_count_presente / attendance_stat.class_count_total)*100;
			}

			//Retira o Todas
			List<Attendance_Stat> Data_aux1 = new List<Attendance_Stat>();
            foreach (Attendance_Stat attendance_stat in Data_aux)
            {
				if (attendance_stat.name != "Todas")
				{
					Data_aux1.Add(attendance_stat);
                }
                
            }

			//Ordena a lista sem o Todas
            List<Attendance_Stat> Data_order = Data_aux1.OrderBy(d => d.name).ToList();

            Data = new List<Attendance_Stat>();
			Data.Add(Data_aux.ElementAt(0));
            foreach (Attendance_Stat attendance_stat in Data_order)
            {
				Data.Add(attendance_stat);
            }

            this.Print();
			//return competition_results;

		}

		public Attendance_Stat checkTypeExists(List<Attendance_Stat> Data, string class_type)
		{
			for (int i=0; i< Data.Count; i++)
			{
				Attendance_Stat attendance_Stat = Data[i];
				//Debug.Print("CheckExists compare " + attendance_Stat.name + " " + class_type);
				if (attendance_Stat.name==class_type) 
				{
					//Debug.Print("CheckExists true ");
					return attendance_Stat;
				}
			}
			Attendance_Stat attendance_stat = new Attendance_Stat(class_type);
			Data.Add(attendance_stat);
			return attendance_stat;

		}


		public void Print()
		{
			foreach (Attendance_Stat attendance_stat in Data)
			{
				Debug.Print("AQUII2 " + attendance_stat.name + " " + attendance_stat.class_count_presente+" " + attendance_stat.class_count_ausente);
			}


		}
	}


}
