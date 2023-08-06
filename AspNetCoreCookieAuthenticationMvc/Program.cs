using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// コンテナにサービスを追加します。
builder.Services.AddControllersWithViews();

// ※ここから追加

// Cookie による認証スキームを追加する
builder.Services
  .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie();

builder.Services.AddAuthorization(options =>
{
  // AllowAnonymous 属性が指定されていないすべての画面、アクションなどに対してユーザー認証が必要となる
  options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
});

// ※ここまで追加

var app = builder.Build();

// HTTP リクエスト パイプラインを構成します。
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // デフォルトの HSTS 値は 30 日です。 運用シナリオではこれを変更することもできます。https://aka.ms/aspnetcore-hsts を参照してください。
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // [追加] 認証
app.UseAuthorization(); // 認可

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
