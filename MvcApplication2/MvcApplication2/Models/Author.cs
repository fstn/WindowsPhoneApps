
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
namespace MvcApplication2.Models.Generated
{
    [MetadataType(typeof(AuthorMetaData))]

    public partial class Author : IValidatableObject
    {

        private Database1Entities context = new Database1Entities();

        public string FirstName { get; set; }
        public string LastName { get; set; }


        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            int nbAuthor=(from m in context.Author where m.FirstName==this.FirstName && m.LastName==this.LastName select m).Count();
            if (nbAuthor >= 1)
            {
                 yield return new ValidationResult("already exist");
            }
        }
    }
    public class AuthorMetaData
    {
    }
}
