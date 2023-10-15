using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pupil.Model
{
    public class FeeStructureRequestDc
    {
        [JsonPropertyName("feeStructure")] 
        public FeeStructurePropsDc FeeStructure { get; set; } = new FeeStructurePropsDc();

        [JsonPropertyName("selectedFeeTypes")]
        public List<SelectedFeeTypePropsDc> SelectedFeeTypes { get; set; } = new List<SelectedFeeTypePropsDc>();

        [JsonPropertyName("selectedClass")]
        public List<int> SelectedClass { get; set; } = new List<int>();

        [JsonPropertyName("selectedIntallments")]
        public List<SelectedIntallmentPropsDc> SelectedIntallments { get; set; } = new List<SelectedIntallmentPropsDc>();
    }

    public class FeeStructurePropsDc
    {
        public int FeeStructureId { get; set; }
        public string Sname { get; set; } = String.Empty;
        public string ReceiptPrefix { get; set; } = String.Empty;
        public int ReceiptStartingNumber { get; set; }

        [JsonPropertyName("totalAmount")]
        public Decimal TotalAmount { get; set; }

        [JsonPropertyName("totalAmountWithTax")]
        public Decimal TotalAmountWithTax { get; set; }
    }

    public class SelectedFeeTypePropsDc
    {
        public int FeeTypeId { get; set; }
        public string FeeTypeName { get; set; } = String.Empty;
        public decimal Tax { get; set; }
    }
    
    public class SelectedIntallmentPropsDc
    {
        public Guid InstallmentId { get; set; }
        public string FeeTypeName { get; set; } = String.Empty;
        public int FeeTypeId { get; set; }

        public decimal Tax { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateOfInstallment { get; set; }


    }
}
