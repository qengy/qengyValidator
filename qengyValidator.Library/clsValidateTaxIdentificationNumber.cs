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
            string patternCIF = @"^[X-Z][0-9]{7}\w$";
            Regex regexCIF = new Regex(patternCIF);
            if (regexCIF.IsMatch(document))
                return DocTypes.CIF;
            return DocTypes.NO_VALID_DOC;
        }

    }

}