using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AspNetCoreCookieAuthenticationRazorPages.Pages.Account
{
  [AllowAnonymous]
  public class LoginModel : PageModel
  {
    /// <summary>���[�U�[���B</summary>
    [BindProperty]
    [Required]
    [DisplayName("���[�U�[��")]
    public string UserName { get; set; } = "";

    /// <summary>�p�X���[�h�B</summary>
    [BindProperty]
    [Required]
    [DataType(DataType.Password)]
    [DisplayName("�p�X���[�h")]
    public string Password { get; set; } = "";

    /// <summary>���̃��[�U�[�f�[�^�x�[�X�Ƃ���B</summary>
    private Dictionary<string, string> UserAccounts { get; set; } = new Dictionary<string, string>
      {
        { "user1", "password1" },
        { "user2", "password2" },
      };


    /// <summary>���O�C�������B</summary>
    public async Task<ActionResult> OnPost()
    {
      // ���͓��e�ɃG���[������ꍇ�͏����𒆒f���ăG���[�\��
      if (ModelState.IsValid == false) return Page();

      // ���[�U�[�̑��݃`�F�b�N�ƃp�X���[�h�`�F�b�N (������)
      // �{ Tips �� Cookie �F�؂��ł��邩�ǂ����̊m�F�ł��邽�ߓ��͓��e��p�X���[�h�̌����ȃ`�F�b�N�͍s���Ă��܂���
      if (UserAccounts.TryGetValue(UserName, out string? getPass) == false || Password != getPass)
      {
        ModelState.AddModelError("", "���[�U�[���܂��̓p�X���[�h����v���܂���B");
        return Page();
      }

      // �T�C���C���ɕK�v�ȃv�����V�p�������
      var claims = new[] { new Claim(ClaimTypes.Name, UserName) };
      var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      var principal = new ClaimsPrincipal(identity);

      // �F�؃N�b�L�[�����X�|���X�ɒǉ�
      await HttpContext.SignInAsync(principal);

      // ���O�C�����K�v�ȉ�ʂɃ��_�C���N�g���܂�
      return RedirectToPage("/Index");
    }

    /// <summary>���O�A�E�g�����B</summary>
    public async Task OnGetLogout()
    {
      // �F�؃N�b�L�[�����X�|���X����폜
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
  }
}
