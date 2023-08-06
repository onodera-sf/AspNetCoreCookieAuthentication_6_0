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
    /// <summary>ユーザー名。</summary>
    [BindProperty]
    [Required]
    [DisplayName("ユーザー名")]
    public string UserName { get; set; } = "";

    /// <summary>パスワード。</summary>
    [BindProperty]
    [Required]
    [DataType(DataType.Password)]
    [DisplayName("パスワード")]
    public string Password { get; set; } = "";

    /// <summary>仮のユーザーデータベースとする。</summary>
    private Dictionary<string, string> UserAccounts { get; set; } = new Dictionary<string, string>
      {
        { "user1", "password1" },
        { "user2", "password2" },
      };


    /// <summary>ログイン処理。</summary>
    public async Task<ActionResult> OnPost()
    {
      // 入力内容にエラーがある場合は処理を中断してエラー表示
      if (ModelState.IsValid == false) return Page();

      // ユーザーの存在チェックとパスワードチェック (仮実装)
      // 本 Tips は Cookie 認証ができるかどうかの確認であるため入力内容やパスワードの厳密なチェックは行っていません
      if (UserAccounts.TryGetValue(UserName, out string? getPass) == false || Password != getPass)
      {
        ModelState.AddModelError("", "ユーザー名またはパスワードが一致しません。");
        return Page();
      }

      // サインインに必要なプリンシパルを作る
      var claims = new[] { new Claim(ClaimTypes.Name, UserName) };
      var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      var principal = new ClaimsPrincipal(identity);

      // 認証クッキーをレスポンスに追加
      await HttpContext.SignInAsync(principal);

      // ログインが必要な画面にリダイレクトします
      return RedirectToPage("/Index");
    }

    /// <summary>ログアウト処理。</summary>
    public async Task OnGetLogout()
    {
      // 認証クッキーをレスポンスから削除
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
  }
}
