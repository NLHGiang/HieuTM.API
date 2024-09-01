using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HieuTM.API.B1.ObjectAttribute
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var phoneNumber = Convert.ToString(value);

            //// Check null, empty, white space
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return new ValidationResult("Số điện thoại 2 không được để trống");
            }

            // Check regex
            var regex = new Regex("^0[0-9]{8}$");
            if (!regex.IsMatch(phoneNumber))
            {
                return new ValidationResult("Số điện thoại 2 gồm 10 số, bắt đầu bằng 0");
            }

            return ValidationResult.Success;
        }
    }
}
