using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Core.Entities
{
    [Table("tblFeeStructure")]
    public class FeeStructure
    {
        public int FeeStructureId { get; set; }

        public string Sname { get; set; }

        public bool Status { get; set; }

        public string TenantId { get; set; }
    }
}
