using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Porovnavanie_suborov_generovanie_xml
{
    internal class Constants
    {
        public static string PROGRAM_ERROR = "Nahláste CHYBU PROGRAMU: ";

        //server pre chybové hlásenia programu
        public static string SERVER_FOR_ERRORS = "https://script.google.com/macros/s/AKfycbwtfFR28rfsSptW9RGUz7w82VbWk6sgc5-gl1oxmLJv8USOdhQUZSGtutZMUWiHG-tT/exec";
    }
}
