using System.ComponentModel.DataAnnotations;

namespace Protivity.Assignment.CustomerApi.Common
{
    public class InvalidModelException : Exception
    {

        /// <summary>
        /// hold errors collection
        /// </summary>
        public IEnumerable<ValidationResult> Errors { get; }

        public InvalidModelException(IEnumerable<ValidationResult> errors)
        {
            Errors = errors;
        }
    }
}
