using System.ComponentModel.DataAnnotations;

namespace Notes.Identity.Models
{
    public class LogoutViewModel
    {
        public string LogoutId { get; set; }
        public string ReturnUrl { get; set; }
    }
}
