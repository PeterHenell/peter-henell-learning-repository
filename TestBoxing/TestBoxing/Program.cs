using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBoxing
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.ms");
            var dt = DateTime.Parse("2013-01-15T14:04:51.611373");
            Console.WriteLine(dt.ToString("yyyy-MM-ddThh:mm:ss.ffffff"));
            Console.ReadKey();
            //Boxer b = new Boxer();
            //b.TheField = 1234567890778899;
            //Console.WriteLine(b.TheField.GetType());
            //b.TheField = "Peter";
            //Console.WriteLine(b.TheField.GetType());
        }

        class Boxer
        {

            object _theField;
            public object TheField
            {
                get
                {
                    return _theField;
                }
                set
                {
                    if (_theField != value)
                    {
                        if (_theField != null &&_theField.GetType() != value.GetType())
                        {
                            Console.WriteLine("not same type");
                            return;
                        }

                        _theField = value;
                    }
                }
            }

        }
    }
}
