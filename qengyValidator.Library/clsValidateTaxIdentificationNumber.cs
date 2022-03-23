using System.Text.RegularExpressions;

namespace qengyValidator.Library
{
    /// <summary>
    /// Class with methods to facilitate the validation of fiscal identifiers in Spain
    /// </summary>
    public class clsValidateTaxIdentificationNumber
    {
        //Document types
        public enum DocTypes { NIF, NIE, CIF, NO_VALID_DOC }

        /// <summary>
        /// Function that based on the regex of document returns the type of document
        /// </summary>
        /// <param name="document">Document</param>
        /// <returns>Document Type</returns>
        public DocTypes GetDocumentType(string document)
        {
            document = document.Replace(" ", "").ToUpper();
            string patternNIF = @"^[0-9]{8}[A-Z]$";
            Regex regexNIF = new Regex(patternNIF);
            if (regexNIF.IsMatch(document))
                return DocTypes.NIF;
            string patternNIE = @"^[X-Z][0-9]{7}[A-Z]$";
            Regex regexNIE = new Regex(patternNIE);
            if (regexNIE.IsMatch(document))
                return DocTypes.NIE;
            string patternCIF = @"^[ABCDEFGHJKLMNPQRSUVW][0-9]{7}\w$";
            Regex regexCIF = new Regex(patternCIF);
            if (regexCIF.IsMatch(document))
                return DocTypes.CIF;
            return DocTypes.NO_VALID_DOC;
        }

        /// <summary>
        /// Function that validate the document is valid
        /// </summary>
        /// <param name="document">Document</param>
        /// <returns>Tuple of Document Type and Boolean</returns>
        public (DocTypes,bool) ValidateDocument(string document)
        {
            document = document.Replace(" ", "").ToUpper();
            DocTypes docType = GetDocumentType(document);
            switch(docType)
            {
                case DocTypes.NIF: return (docType, ValidNIF(document));
                case DocTypes.NIE: return (docType, ValidNIE(document));
                case DocTypes.CIF: return (docType, ValidCIF(document));
                default: return (docType, false);
            }
        }

        /// <summary>
        /// Function that calculate the NIF control letter
        /// </summary>
        /// <param name="document">Document</param>
        /// <returns>Tuple of Document Type and String</returns>
        public (DocTypes, string) CalculateControlNIF(string document)
        {
            document = document.Replace(" ", "").ToUpper();
            string patternNIF = @"^[0-9]{8}$";
            Regex regexNIF = new Regex(patternNIF);
            if (regexNIF.IsMatch(document))
            {
                string sequenceNIF = "TRWAGMYFPDXBNJZSQVHLCKE";
                int i = int.Parse(document.Substring(0, 8)) % 23;
                return (DocTypes.NIF, sequenceNIF[i].ToString());
            }
            else return (DocTypes.NO_VALID_DOC, "");
        }

        /// <summary>
        /// Function to validate NIF
        /// </summary>
        /// <param name="document">Document</param>
        /// <returns>Boolean</returns>
        private bool ValidNIF(string document)
        {
            string sequenceNIF = "TRWAGMYFPDXBNJZSQVHLCKE";
            int i = int.Parse(document.Substring(0,8)) % 23;
            return sequenceNIF[i]==document[8] ? true : false;
        }

        /// <summary>
        /// Function to validate NIE
        /// </summary>
        /// <param name="document">Document</param>
        /// <returns>Boolean</returns>
        private bool ValidNIE(string document)
        {
            document = document[0].ToString().Replace("X", "0").Replace("Y", "1").Replace("Z", "2") + document.Substring(1);
            return ValidNIF(document);
        }

        /// <summary>
        /// Function to validate CIF
        /// </summary>
        /// <param name="document">Document</param>
        /// <returns>Boolean</returns>
        private bool ValidCIF(string document)
        {
            string sequenceCIF = "JABCDEFGHI";
            string letter = document.Substring(0, 1);
            string control = document.Substring(8);
            string number = document.Substring(1, 7);
            int intA = int.Parse(number[1].ToString()) + int.Parse(number[3].ToString()) + int.Parse(number[5].ToString());
            int intB1 = int.Parse(number[0].ToString()) * 2;
            string strB1 = intB1.ToString("00");
            int intB1_2 = int.Parse(strB1[0].ToString()) + int.Parse(strB1[1].ToString());
            int intB2 = int.Parse(number[2].ToString()) * 2;
            string strB2 = intB2.ToString("00");
            int intB2_2 = int.Parse(strB2[0].ToString()) + int.Parse(strB2[1].ToString());
            int intB3 = int.Parse(number[4].ToString()) * 2;
            string strB3 = intB3.ToString("00");
            int intB3_2 = int.Parse(strB3[0].ToString()) + int.Parse(strB3[1].ToString());
            int intB4 = int.Parse(number[6].ToString()) * 2;
            string strB4 = intB4.ToString("00");
            int intB4_2 = int.Parse(strB4[0].ToString()) + int.Parse(strB4[1].ToString());
            int intB = intB1_2 + intB2_2 + intB3_2 + intB4_2;
            int intC = intA + intB;
            int intControlDigit = 10 - int.Parse(intC.ToString().Substring(1, 1));
            string strControlLetter = sequenceCIF[intControlDigit].ToString();
            string patternDigit = @"^[ABEH]$";
            string patternLetter = @"^[KPQS]$";
            Regex regexDigit = new Regex(patternDigit);
            Regex regexLetter = new Regex(patternLetter);
            if (regexDigit.IsMatch(document))
                return control == intControlDigit.ToString();
            else if (regexLetter.IsMatch(document))
                return control == strControlLetter;
            else
                return control == intControlDigit.ToString() || control == strControlLetter;
        }

    }

}