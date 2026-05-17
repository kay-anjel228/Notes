using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyNoteProject.Data;
using StudyNoteProject.Models;
using StudyNoteProject.Services;

namespace StudyNoteProject.Pages.Shared
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly ICurrentUserService _currentUserService;

        public IndexModel(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _currentUserService = currentUserService;
        }

        [BindProperty]
        public string RegisterName { get; set; } = string.Empty;
        [BindProperty]
        public string RegisterLogin { get; set; } = string.Empty;
        [BindProperty]
        public string RegisterPassword { get; set; } = string.Empty;
        [BindProperty]
        public string RegisterRepeatPassword { get; set; } = string.Empty;

        [BindProperty]
        public string LoginLogin { get; set; } = string.Empty;
        [BindProperty]
        public string LoginPassword { get; set; } = string.Empty;

        public bool IsAuthorized { get; set; }
        public string CurrentUserName { get; set; } = string.Empty;
        public string CurrentUserLogin { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
            LoadUser();
        }

        public IActionResult OnPostRegister()
        {
            LoadUser();
            if (string.IsNullOrEmpty(RegisterName)
                || string.IsNullOrEmpty(RegisterLogin)
                || string.IsNullOrEmpty(RegisterPassword))
            {
                Message = "Заполните все поля регистрации";
                return Page();
            }

            if (_context.User.Any(u => u.Login == RegisterLogin))
            {
                Message = "Пользователь с таким логином уже существует";
                return Page();
            }
            if (RegisterPassword != RegisterRepeatPassword || string.IsNullOrEmpty(RegisterRepeatPassword))
            {
                Message = "Пароли не сходятся";
                return Page();
            }
            var user = new User
            {
                Name = RegisterName,
                Login = RegisterLogin
            };

            user.HashPassword = _passwordHasher.HashPassword(user, RegisterPassword);

            _context.User.Add(user);
            _context.SaveChanges();

            _currentUserService.SignIn(HttpContext, user.Id);

            return RedirectToPage();
        }

        public IActionResult OnPostLogin()
        {
            LoadUser();
            if (string.IsNullOrEmpty(LoginLogin) || string.IsNullOrEmpty(LoginPassword))
            {
                Message = "Введите логин и пароль";
                return Page();
            }
            var user = _context.User.FirstOrDefault(u => u.Login == LoginLogin);

            if (user == null)
            {
                Message = "Неверный логин или пароль";
                return Page();
            }
            var res = _passwordHasher.VerifyHashedPassword(
                    user,
                    user.HashPassword,
                    LoginPassword
                );

            if (res == PasswordVerificationResult.Failed)
            {
                Message = "Неверный логин или пароль";
                return Page();
            }
            _currentUserService.SignIn(HttpContext, user.Id);

            return RedirectToPage();
        }

        public IActionResult OnPostLogout()
        {
            _currentUserService.SignOut(HttpContext);
            return RedirectToPage();
        }

        private void LoadUser()
        {
            var user = _currentUserService.GetCurrentUser(HttpContext);

            if (user == null)
            {
                IsAuthorized = false;
                HttpContext.Session.Clear();
                return;
            }
            IsAuthorized = true;
            CurrentUserName = user.Name;
            CurrentUserLogin = user.Login;
        }
    }
}