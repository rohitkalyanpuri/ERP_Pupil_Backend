using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Model
{
    [Table("tblFee")]
    public class Fee
    {
        public int FeeId { get; set; }

        public int StudentId { get; set; }

        public int FeeStructureId { get; set; }

        public decimal Amount { get; set; }

        public int FeeModeId { get; set; }

        public string TransactionNo { get; set; }

        public int Installment { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
