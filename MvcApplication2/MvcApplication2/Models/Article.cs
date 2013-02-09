
using System;
using System.Collections.Generic;

namespace MvcApplication2.Models.Generated
{

    [MetadataType(typeof(ArticleMetaData))]
    public partial class Article:IValidatableObject
    {

    }
    public class ArticleMetaData
    {
        [DataType(DataType.Date)]
        public System.DateTime DateCreated { get; set; }
    }
}
