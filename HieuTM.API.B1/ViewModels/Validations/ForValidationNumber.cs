using System.ComponentModel.DataAnnotations;

namespace HieuTM.API.B1.ViewModels.Validations
{
    public partial class ForValidation
    {
        [Range(10, 100, ErrorMessage = "Giá trị từ 10-100")]
        public int Amount { get; set; }

        [Compare("Amount", ErrorMessage = "Giá trị và giá trị đã kiểm tra không trùng nhau")]
        public int CheckedAmount { get; set; }
    }
}
