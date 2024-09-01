using HieuTM.API.B1.ObjectAttribute;
using System.ComponentModel.DataAnnotations;

namespace HieuTM.API.B1.ViewModels.Validations
{
    public class ForValidation
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Định dạng của email không chính xác")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 20, ErrorMessage = "Ghi chú có độ dài từ 20-100 kí tự")]
        public string Note { get; set; }

        [Range(10, 100, ErrorMessage = "Giá trị từ 10-100")]
        public int Amount { get; set; }

        [Compare("Amount", ErrorMessage = "Giá trị và giá trị đã kiểm tra không trùng nhau")]
        public int CheckedAmount { get; set; }

        [RegularExpression("^0[0-9]{8}$", ErrorMessage = "Số điện thoại gồm 10 số, bắt đầu bằng 0")]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string? PhoneNumber1 { get; set; }

        [PhoneNumber]
        public string? PhoneNumber2 { get; set; }
    }
}
