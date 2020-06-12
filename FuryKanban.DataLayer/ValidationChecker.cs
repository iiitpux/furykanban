using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FuryKanban.DataLayer
{
    public static class ValidationChecker
    {
        public static bool Check<T>(T objectToCheck, out List<ValidationResult> results) where T : class
        {
            var context = new ValidationContext(objectToCheck, serviceProvider: null, items: null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(objectToCheck, context, results);
        }
    }
}