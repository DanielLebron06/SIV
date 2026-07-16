using FluentValidation;

namespace SIV.Application.Common.Extensions
{
    /// <summary>
    /// Métodos de extensión para FluentValidation que permiten reutilizar reglas de validación comunes.
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Valida que la contraseña cumpla con los requisitos mínimos de seguridad.
        /// Debe tener al menos 8 caracteres, una mayúscula, una minúscula y un número.
        /// </summary>
        public static IRuleBuilderOptions<T, string> PasswordSeguro<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
                .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
                .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número.");
        }

        /// <summary>
        /// Valida que una propiedad de texto no esté vacía ni contenga solo espacios en blanco.
        /// </summary>
        public static IRuleBuilderOptions<T, string> Requerido<T>(this IRuleBuilder<T, string> ruleBuilder, string nombreCampo)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"El campo '{nombreCampo}' es obligatorio.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage($"El campo '{nombreCampo}' no puede contener solo espacios en blanco.");
        }
    }
}
