using Microsoft.Maui.Controls;
using System.Windows.Input;
using ToDoListApp.Services;
using ToDoListApp.ViewModels;
using ToDoListApp.Views;

namespace ToDoListApp
{
    public partial class AppShell : Shell
    {
        private readonly AuthService _authService;

        public ICommand LogoutCommand { get; private set; }
        
        public string Username => _authService.CurrentUser?.Username; 
        public bool IsAdmin => _authService.IsAdmin; 

        public AppShell(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            BindingContext = this;
            
            // obserwator zmian w AuthService
            _authService.PropertyChanged += (sender, args) => 
            {
                // Aktualizacjowanie właściwości po zmianie stanu uwierzytelniania
                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(IsAdmin));
            };
            
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(TasksPage), typeof(TasksPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(TaskDetailPage), typeof(TaskDetailPage));
            Routing.RegisterRoute(nameof(AdminPage), typeof(AdminPage));

            LogoutCommand = new Command(async () =>
            {
                _authService.Logout();
                
                var loginViewModel = Application.Current.Handler.MauiContext.Services.GetService<LoginViewModel>();
                var loginPage = new LoginPage(loginViewModel);
                
                Application.Current.MainPage = loginPage;
                
                Current.FlyoutIsPresented = false;
            });
        }
    }
}