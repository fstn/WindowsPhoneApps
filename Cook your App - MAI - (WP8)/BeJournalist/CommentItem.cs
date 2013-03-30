using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BeJournalist
{
    public class CommentItem
    {
        public int Id { get; set; }

        [DataMember(Name = "sport")]
        public string Sport { get; set; }

        [DataMember(Name = "match")]
        public string Match { get; set; }

        [DataMember(Name = "comment")]
        public string Contenu { get; set; }
    }
}
