using System.ComponentModel.DataAnnotations;

namespace Netplanety.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class CPFAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        // In future this method could be used
        // to validate integers but not just yet
        if (value?.GetType() != typeof(string))
        {
            throw new InvalidOperationException(nameof(value));
        }

        // Removes all instances of dots and hifens that could be present in the string
        // reverses its order to facilitate the calculation and converts into an array
        ReadOnlySpan<char> cpf = ((string)value).Replace(".", string.Empty)
            .Replace("-", string.Empty).Reverse().ToArray();

        // Checks if there are preciselly
        // eleven characters left
        if (cpf.Length is not 11)
        {
            return false;
        }

        // v1 and v2 are the first and second verification digits, respectively
        // c0 is the first character in the array after the verification
        ReadOnlySpan<char> c0 = cpf.Slice(2, 1);
        bool isRepeatingPattern = true;
        int v1 = 0, v2 = 0;

        for (int i = 0; i < cpf.Length - 2; i++)
        {
            // ci is the current character
            ReadOnlySpan<char> ci = cpf.Slice(i + 2, 1);

            // If there are non digits exit early
            if (char.IsDigit(ci[0]) is false)
            {
                return false;
            }

            // Checks for repeating patterns
            if (isRepeatingPattern && ci.CompareTo(c0, StringComparison.InvariantCulture) is not 0)
            {
                isRepeatingPattern = false;
            }

            // di is the current digit
            int di = int.Parse(ci);
            v1 += di * (9 - (i % 10));
            v2 += di * (9 - ((i + 1) % 10));
        }

        // Repeating patterns are not
        // considered valid CPF instances
        if (isRepeatingPattern)
        {
            return false;
        }

        // d10 is the first verification digit
        int d10 = int.Parse(cpf.Slice(1, 1));
        v1 = v1 % 11 % 10;

        // Validates the first digit
        if (v1 != d10)
        {
            return false;
        }

        // d11 is the second and last verification digit
        int d11 = int.Parse(cpf.Slice(0, 1));
        v2 = (v2 + (v1 * 9)) % 11 % 10;

        // Validates the second digit
        return v2 == d11;
    }
}