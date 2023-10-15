using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Model
{
    [Table("tblFeeStructureInstallment")]
    public class FeeStructureInstallment
    {
        public Guid InstallmentId { get; set; }

        public int FeeStructureId { get; set; }

        public int FeeTypeId { get; set; }
        public decimal Tax { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateOfInstallment { get; set; }
    }
}
