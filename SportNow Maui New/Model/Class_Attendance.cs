using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SportNow.Model
{
    public class Class_Attendance: INotifyPropertyChanged
    {
        public string classattendanceid { get; set; }
        public string classid { get; set; }
        public string classname { get; set; }
        public string memberid { get; set; }
        public string membername { get; set; }
        public string membernickname { get; set; }
        public string imagesource { get; set; }
        
        public string status { get; set; }
        public string date { get; set; }
        public bool statuschanged { get; set; } = false;

        public Color Color { get; set; }
        public Color color
        {
            get
            {
                return Color;
            }
            set
            {
                //Debug.Print("AQUIIIII MADOU");
                if (Color != value)
                {
                    //Debug.Print("AQUIIIII MADOU1");
                    Color = value;
                    //Debug.Print("AQUIIIII MADOU2");
                    NotifyPropertyChanged();
                }
            }
        }


        public string ColorImage { get; set; }
        public string colorImage
        {
            get
            {
                return ColorImage;
            }
            set
            {
                //Debug.Print("AQUIIIII MADOU");
                if (ColorImage != value)
                {
                    //Debug.Print("AQUIIIII MADOU1");
                    ColorImage = value;
                    //Debug.Print("AQUIIIII MADOU2");
                    NotifyPropertyChanged();
                }
            }
        }



        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            //Debug.Print("AQUIIIII MADOU3");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
