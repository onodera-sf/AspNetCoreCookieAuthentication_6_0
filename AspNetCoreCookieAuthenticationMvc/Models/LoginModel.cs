using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreCookieAuthenticationMvc.Models
{
  public class LoginModel
  {
    /// <summary>ユーザー名。</summary>
    [Required]
    [DisplayName("ユーザー名")]
    public string UserName { get; set; } = "";

    /// <summary>パスワード。</summary>
    [Required]
    [DataType(DataType.Password)]
    [DisplayName("パスワード")]
    public string Password { get; set; } = "";
  }
}