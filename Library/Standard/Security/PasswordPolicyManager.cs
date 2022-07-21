
// Type: Cashmere.Library.Standard.Security.PasswordPolicyManager


using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cashmere.Library.Standard.Security
{
    public class PasswordPolicyManager
    {
        public static IList<PasswordPolicyResult> Validate(
          string Password,
          PasswordPolicyItems Policy)
        {
            IList<PasswordPolicyResult> passwordPolicyResultList = new List<PasswordPolicyResult>();
            if (Password.Length < Policy.MinimumLength)
                passwordPolicyResultList.Add(PasswordPolicyResult.MINIMUM_LENGTH);
            if (UpperCaseCount(Password) < Policy.UpperCaseLength)
                passwordPolicyResultList.Add(PasswordPolicyResult.UPPER_CASE_LENGTH);
            if (LowerCaseCount(Password) < Policy.LowerCaseLength)
                passwordPolicyResultList.Add(PasswordPolicyResult.LOWER_CASE_LENGTH);
            if (NumericCount(Password) < Policy.NumericLength)
                passwordPolicyResultList.Add(PasswordPolicyResult.NUMERIC_LENGTH);
            if (NonAlphaCount(Password) < Policy.SpecialLength)
                passwordPolicyResultList.Add(PasswordPolicyResult.SPECIAL_LENGTH);
            return passwordPolicyResultList.Count > 0 ? passwordPolicyResultList : null;
        }

        public static int UpperCaseCount(string Password)
        {
            try
            {
                return Regex.Matches(Password, "[A-Z]").Count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int LowerCaseCount(string Password)
        {
            try
            {
                return Regex.Matches(Password, "[a-z]").Count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int NumericCount(string Password)
        {
            try
            {
                return Regex.Matches(Password, "[0-9]").Count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int NonAlphaCount(string Password)
        {
            try
            {
                return Regex.Matches(Password, "[^0-9a-zA-Z\\._]").Count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int SpecificSpecialCount(string Password, string specials)
        {
            string pattern = string.Format("/^[{0}\\w\\s]*$/", specials);
            return Regex.Matches(Password, pattern).Count;
        }
    }
}
