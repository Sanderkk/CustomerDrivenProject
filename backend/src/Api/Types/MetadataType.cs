using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Api.Types;
using src.Api.Models;

namespace src.Api.Types
{
    public class MetadataType : Metadata
    {
        public int MetadataID { get; set; }
        public int? LocationID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? OutdatedFrom { get; set; }
        
    }
}
