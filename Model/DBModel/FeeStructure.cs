using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace Pupil.Model
{
    [Table("tblFeeStructure")]
    public class FeeStructure
    {
        public int FeeStructureId { get; set; }

        public string Sname { get; set; } = string.Empty;

        public bool Status { get; set; }

        public string ReceiptPrefix { get; set; } = string.Empty;

        public int ReceiptStartingNumber { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalAmountWithTax { get; set; }
    }
}
