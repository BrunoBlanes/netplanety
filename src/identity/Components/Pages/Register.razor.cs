using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;

using Netplanety.Identity.Extensions;

using Netplanety.Identity.Models;
using Netplanety.Shared.Interfaces;
using Netplanety.Shared.Models;
using Netplanety.Shared.Services.Api;

namespace Netplanety.Identity.Components.Pages;

public sealed partial class Register : ComponentBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly NavigationManager _navigationManager;
    private readonly IApiService _apiService;
    private readonly ApplicationUser _user;
    private readonly IJSRuntime _jSRuntime;

    private ValidationMessageStore _validationMessageStore;
    private RegistrationStage _registrationStage;
    private IJSObjectReference? _module;
    private bool _eventIsHandled;

    [SupplyParameterFromForm]
    private EmailInputModel Input { get; set; } = null!;

    [SupplyParameterFromQuery]
    private string? ReturnUrl { get; set; }
    private bool HideValidation { get; set; }
    private bool ShowBackButton { get; set; }
    private Client? Client { get; set; }
    private EditContext EditContext { get; set; }
    private ElementReference CpfInput { get; set; }

    public Register(
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        NavigationManager navigationManager,
        IApiService apiService,
        IJSRuntime jSRuntime)
    {
        _navigationManager = navigationManager;
        _userManager = userManager;
        _apiService = apiService;
        _userStore = userStore;
        _jSRuntime = jSRuntime;

        HideValidation = false;
        ShowBackButton = false;
        _eventIsHandled = false;
        _user = new ApplicationUser();
        Input = new EmailInputModel();
        _registrationStage = RegistrationStage.Email;
        EditContext = new EditContext(Input);
        _validationMessageStore = new ValidationMessageStore(EditContext);
    }

    protected override void OnInitialized()
    {
        _navigationManager.LocationChanged += LocationChangedEventHandler;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./components/pages/register.razor.js");
        }
    }

    private async Task CreateUserAsync(EditContext _)
    {
        HideValidation = false;
        string uri = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);

        if (_registrationStage is RegistrationStage.Email)
        {
            if (await _userManager.FindByEmailAsync(Input.Email) is not null)
            {
                _validationMessageStore.Clear();
                _validationMessageStore.Add(() => Input.Email, "This email address is already in use. Please try a different one.");
                EditContext.NotifyValidationStateChanged();
                return;
            }

            ShowBackButton = true;
            Input = new PasswordInputModel(Input);
            ResetFormEditContext();

            await _userStore.SetUserNameAsync(_user, Input.Email, CancellationToken.None);
            await ((IUserEmailStore<ApplicationUser>)_userStore).SetEmailAsync(_user, Input.Email, CancellationToken.None);
            _navigationManager.NavigateTo(uri, new NavigationOptions { HistoryEntryState = HistoryEntryStates.Password });
            _registrationStage = RegistrationStage.Password;
        }

        else if (_registrationStage is RegistrationStage.Password)
        {
            Input = new CpfInputModel((PasswordInputModel)Input);
            ResetFormEditContext();

            _navigationManager.NavigateTo(uri, new NavigationOptions { HistoryEntryState = HistoryEntryStates.Identification });
            _registrationStage = RegistrationStage.Identification;
        }

        else if (_registrationStage is RegistrationStage.Identification)
        {
            var input = (CpfInputModel)Input;

            if (await _userManager.GetUserByCpfAsync(input.CPF, CancellationToken.None) is not null)
            {
                _validationMessageStore.Clear();
                _validationMessageStore.Add(() => input.CPF, "This CPF is already in use, please log in.");
                EditContext.NotifyValidationStateChanged();
                return;
            }

            if (await _apiService.GetClientByCpfAsync(input.CPF, CancellationToken.None) is Client client)
            {
                Client = client;
                _registrationStage = RegistrationStage.Confirmation;
                StateHasChanged();
            }
        }

        else if (_registrationStage is RegistrationStage.Confirmation)
        {

        }
    }

    private void EmailOnInputEventHandler(ChangeEventArgs _)
    {
        if (EditContext.GetValidationMessages(() => Input.Email).Any())
        {
            _validationMessageStore.Clear();
            StateHasChanged();
        }
    }

    private async void LocationChangedEventHandler(object? _, LocationChangedEventArgs args)
    {
        // Local page navigation
        if (args.Location == _navigationManager.Uri)
        {
            if (_module is not null && _eventIsHandled is false && CpfInput.Context is not null)
            {
                _eventIsHandled = true;
                await _module.InvokeVoidAsync("attachCpfMaskEventHandler", CpfInput);
            }

            // Navigating back from password setup
            if (args.HistoryEntryState is null && _registrationStage is RegistrationStage.Password)
            {
                HideValidation = false;
                ShowBackButton = false;
                _registrationStage = RegistrationStage.Email;
                Input = new EmailInputModel(Input.Email);
                ResetFormEditContext();
                StateHasChanged();
            }

            // Navigating back from identification page
            else if (args.HistoryEntryState == HistoryEntryStates.Password && _registrationStage is RegistrationStage.Identification)
            {
                HideValidation = false;
                _registrationStage = RegistrationStage.Password;
                Input = new PasswordInputModel(Input);
                ResetFormEditContext();
                StateHasChanged();
            }
        }
    }

    private void OnGoBackButtonClick(MouseEventArgs _)
    {
        HideValidation = true;
        _navigationManager.NavigateTo("javascript:history.back()");
    }

    private void ResetFormEditContext()
    {
        EditContext = new EditContext(Input);
        _validationMessageStore = new ValidationMessageStore(EditContext);
    }

    public async ValueTask DisposeAsync()
    {
        _navigationManager.LocationChanged -= LocationChangedEventHandler;

        if (_module is not null)
        {
            try
            {
                await _module.DisposeAsync();
            }

            catch (JSDisconnectedException) { }
        }
    }

    internal enum RegistrationStage
    {
        Email,
        Password,
        Identification,
        Confirmation
    }

    internal readonly struct HistoryEntryStates
    {
        public static string Password = "Setup a password";
        public static string Identification = "Identity yourself";
        public static string Confirmation = "Confirm your account";
    }
}
