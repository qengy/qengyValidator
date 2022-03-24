using System.Text.RegularExpressions;

namespace qengyValidator.Library
{
    /// <summary>
    /// Class with methods to facilitate the validation of CUPS in Spain - https://es.wikipedia.org/wiki/C%C3%B3digo_Unificado_de_Punto_de_Suministro
    /// </summary
    public class clsValidateCUPS
    {

        /// <summary>
        /// Function that validate the CUPS is valid
        /// </summary>
        /// <param name="cups">CUPS</param>
        /// <returns>Boolean</returns>
        public bool ValidateDocument(string cups)
        {
            cups = cups.Replace(" ", "").ToUpper();
            string patternCUPS = @"^ES[0-9]{16}[A-Z]{2}[0-9]{0,1}[FPCX]{0,1}$";
            Regex regexCUPS = new Regex(patternCUPS);
            if (!regexCUPS.IsMatch(cups))
                return false;
            string cups_n = cups.Substring(2, 16);
            string control = cups.Substring(18, 2);
            string letters = "TRWAGMYFPDXBNJZSQVHLCKE";
            double fmodv = double.Parse(cups_n) % 529;
            int quotient = (int)(fmodv / 23);
            int remainder = (int)(fmodv % 23);
            return control == letters[quotient].ToString() + letters[remainder].ToString();
        }

    }
}
