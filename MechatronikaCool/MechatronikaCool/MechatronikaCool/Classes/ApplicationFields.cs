using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechatronikaCool.Classes
{
    public class ApplicationFields
    {
        private static string prefix = "http://www.automobilova-mechatronika.fei.stuba.sk";

        public static Dictionary<string, string> Fields = new Dictionary<string, string>()
        {
            {"Aplikovaná mechatronika", prefix + "/webstranka/?q=aplikovan%C3%A1-mechatronika"},
            {"Automatizácia", prefix + "/webstranka/?q=automatiz%C3%A1cia"},
            {"Biomechatronika", prefix + "/webstranka/?q=biomechatronika"},
            {"Digitálna továreň", prefix + "/webstranka/?q=digit%C3%A1lna-tov%C3%A1re%C5%88"},
            {"Elektromobilita", prefix + "/webstranka/?q=elektromobilita"},
            {"Elektronika a mikroprocesory", prefix + "/webstranka/?q=elektronika-mikroprocesory"},
            {"IoT a multimédiá", prefix + "/webstranka/?q=internet-things-multim%C3%A9di%C3%A1-v-mechatronike"},
            {"Počítačové modelovanie", prefix + "/webstranka/?q=mechanika-po%C4%8D%C3%ADta%C4%8Dov%C3%A9-modelovanie"},
            {"Virtuálna realita", prefix + "/webstranka/?q=virtu%C3%A1lna-realita"},
            {"Výpočtová inteligencia", prefix + "/webstranka/?q=v%C3%BDpo%C4%8Dtov%C3%A1-inteligencia-v-mechatronike"}
        };
    }
}
