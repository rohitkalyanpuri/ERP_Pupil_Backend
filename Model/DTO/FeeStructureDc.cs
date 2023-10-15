using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Model
{
    public class FeeStructureDc
    {
        public int FeeStructureId { get; set; }

        public string StructureName { get; set; } = string.Empty;

        public bool Status { get; set; }

        public string ReceiptPrefix { get; set; } = string.Empty;

        public int ReceiptStartingNumber { get; set; }
    }
}
