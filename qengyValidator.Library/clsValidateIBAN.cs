using System.Text;
using System.Text.RegularExpressions;

namespace qengyValidator.Library
{
    /// <summary>
    /// Class with methods to facilitate the validation of IBAN
    /// </summary
    public class clsValidateIBAN
    {
        /// <summary>
        /// Function that validate the IBAN is valid
        /// </summary>
        /// <param name="iban">IBAN</param>
        /// <returns>Boolean</returns>
        public bool ValidateDocument(string iban)
        {
            iban = iban.Replace(" ", "").ToUpper();
            string patternIBAN = @"^[A-Z]{2}[0-9]{22}$";
            Regex regexIBAN = new Regex(patternIBAN);
            if (!regexIBAN.IsMatch(iban))
                return false;
            string bank = iban.Substring(4, iban.Length - 4) + iban.Substring(0, 4);
            int asciiShift = 55;
            StringBuilder sb = new StringBuilder();
            foreach (char c in bank)
            {
                int v;
                if (Char.IsLetter(c)) v = c - asciiShift;
                else v = int.Parse(c.ToString());
                sb.Append(v);
            }
            string checkSumString = sb.ToString();
            int checksum = int.Parse(checkSumString.Substring(0, 1));
            for (int i = 1; i < checkSumString.Length; i++)
            {
                int v = int.Parse(checkSumString.Substring(i, 1));
                checksum *= 10;
                checksum += v;
                checksum %= 97;
            }
            return checksum == 1;
        }

    }
}
