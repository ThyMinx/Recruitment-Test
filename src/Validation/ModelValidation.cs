using System.ComponentModel.DataAnnotations;
using Vuture.Exceptions.ExceptionResponses;
using Vuture.Models.Dtos;

namespace Vuture.Validation
{
    public static class ModelValidation
    {
        public static void ValidateCreateContactDto(CreateContactDto entity)
        {
            var validationResults = new List<ValidationResult>();
            //ValidateEntityPropertyIsNotNull(nameof(entity.FirstName), entity.FirstName, validationResults);
            //ValidateEntityPropertyIsNotNull(nameof(entity.LastName), entity.LastName, validationResults);
            //ValidateEntityPropertyIsNotNull(nameof(entity.EmailAddress), entity.EmailAddress, validationResults);
            ValidateEntityPropertyIsNotNullOrEmpty(nameof(entity.FirstName), entity.FirstName, validationResults);
            ValidateEntityPropertyIsNotNullOrEmpty(nameof(entity.LastName), entity.LastName, validationResults);
            ValidateEntityPropertyIsNotNullOrEmpty(nameof(entity.EmailAddress), entity.EmailAddress, validationResults);

            if (validationResults.Count > 0)
            {
                throw new BadRequestExceptionResponse(
                  "Invalid model, these properties are missing - "
                  + string.Join(", ", validationResults.Select(s => s.ErrorMessage).ToArray())
                );
            }
        }

        public static void ValidateUpdateContactDto(UpdateContactDto entity)
        {
            var validationResults = new List<ValidationResult>();
            ValidateEntityPropertyIsNotNullOrEmpty(nameof(entity.FirstName), entity.FirstName, validationResults);
            ValidateEntityPropertyIsNotNullOrEmpty(nameof(entity.LastName), entity.LastName, validationResults);
            ValidateEntityPropertyIsNotNullOrEmpty(nameof(entity.EmailAddress), entity.EmailAddress, validationResults);

            if (validationResults.Count > 0)
            {
                throw new BadRequestExceptionResponse(
                  "Invalid model, these properties are missing - "
                  + string.Join(", ", validationResults.Select(s => s.ErrorMessage).ToArray())
                );
            }
        }

        private static void ValidateEntityPropertyIsNotNull<T>(string entityName, T value, ICollection<ValidationResult> validationResults)
        {
            if (value == null)
            {
                validationResults.Add(new ValidationResult(entityName));
            }
        }
        private static void ValidateEntityPropertyIsNotNullOrEmpty<T>(string entityName, T value, ICollection<ValidationResult> validationResults)
        {
            if (value == null)
            {
                validationResults.Add(new ValidationResult(entityName));
            }
            else if (Type.GetTypeCode(value.GetType()) == TypeCode.String && string.IsNullOrEmpty(value.ToString()))
            {
                validationResults.Add(new ValidationResult(entityName));
            }
        }
    }
}