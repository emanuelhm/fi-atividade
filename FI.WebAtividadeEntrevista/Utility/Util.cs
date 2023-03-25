using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebAtividadeEntrevista.Utility
{
    public static class Util
    {
        /// <summary>
        /// Taken from: https://nadikun.com/how-to-validate-cpf-number-using-custom-method-in-jquery-validate-plugin/
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsCPFValid(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return false;

            var regex = new Regex("([~!@#$%^&*()_+=`{}\\[\\]\\-|\\\\:;'<>,.\\/? ])+", RegexOptions.Multiline);
            cpf = regex.Replace(cpf, "");

            // Checking value to have 11 digits only
            if (cpf.Length != 11)
            {
                return false;
            }

            var array = cpf.Select(c => int.Parse(c.ToString())).ToList();

            int sum = 0, i;
            var firstCN = array[9];
            var secondCN = array[10];

            bool checkResult(int s, int cn)
            {
                var result = (s * 10) % 11;
                if ((result == 10) || (result == 11)) { result = 0; }
                return (result == cn);
            }

            // Checking for dump data
            if (!array.Any(v => v != array[0]))
            {
                return false;
            }

            // Step 1 - using first Check Number:
            for (i = 1; i <= 9; i++)
            {
                sum += array[i - 1] * (11 - i);
            }

            // If first Check Number (CN) is valid, move to Step 2 - using second Check Number:
            if (checkResult(sum, firstCN))
            {
                sum = 0;
                for (i = 1; i <= 10; i++)
                {
                    sum += array[i - 1] * (12 - i);
                }
                return checkResult(sum, secondCN);
            }

            return false;
        }

        internal static bool IsCPFValid(object cPF)
        {
            throw new NotImplementedException();
        }
    }
}
