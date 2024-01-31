using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Data.Dto
{
    public class DocumentMetaDataDto
    {
        public Guid? Id { get; set; }
        public Guid? DocumentId { get; set; }
        public string Metatag { get; set; }
    }
}
