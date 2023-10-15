using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Model
{
    [Table("tblFeeStructureClass")]
    public class FeeStructureClass
    {
        public Guid FeeStructureClassId { get; set; }

        public int FeeStructureId { get; set; }

        public int ClassId { get; set; }
        
    }
}
