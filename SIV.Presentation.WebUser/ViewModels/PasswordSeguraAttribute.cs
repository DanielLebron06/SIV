using System.ComponentModel.DataAnnotations;

namespace SIV.Presentation.WebUser.ViewModels
{
    public sealed class PasswordSeguraAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var password = value as string;
            if (string.IsNullOrEmpty(password))
            {
                return ValidationResult.Success;
            }

            if (password.Length < 8)
            {
                return new ValidationResult("La contraseña debe tener al menos 8 caracteres.");
            }
            if (!password.Any(char.IsUpper))
            {
                return new ValidationResult("La contraseña debe contener al menos una letra mayúscula.");
            }
            if (!password.Any(char.IsLower))
            {
                return new ValidationResult("La contraseña debe contener al menos una letra minúscula.");
            }
            if (!password.Any(char.IsDigit))
            {
                return new ValidationResult("La contraseña debe contener al menos un número.");
            }

            return ValidationResult.Success;
        }
    }
}
