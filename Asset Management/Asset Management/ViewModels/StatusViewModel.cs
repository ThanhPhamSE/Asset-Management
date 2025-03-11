using System.ComponentModel.DataAnnotations;

namespace Asset_Management.ViewModels
{
    public class StatusViewModel
    {
        public int StatusId { get; set; }

        [Required(ErrorMessage = "Tên trạng thái là bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Tên trạng thái không được vượt quá 100 ký tự.")]
        public string StatusName { get; set; }

        [MaxLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự.")]
        public string Description { get; set; }
    }
}
