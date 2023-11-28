using System;
using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System.Threading.Tasks;

public class AuthManager : MonoBehaviour
{
    [Header("Firebase")] 
    public DependencyStatus dependencyStatus;
    public static FirebaseAuth auth;
    public static FirebaseUser User;

    [Header("Login")] 
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;

    [Header("Register")] 
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningEmailText;
    public TMP_Text warningNicknameText;
    public TMP_Text warningPasswordText;
    public TMP_Text warningPasswordConfirmText;


    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (auth is not null)
                return;
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
        emailLoginField.text = PlayerPrefs.GetString("Login");
        passwordLoginField.text = PlayerPrefs.GetString("Password");
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
    }

    public void LoginButton()
    {
        PlayerPrefs.SetString("Login", emailLoginField.text);
        PlayerPrefs.SetString("Password", passwordLoginField.text);
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    public void RegisterButton()
    {
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string email, string password)
    {
        Task<AuthResult> loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);
    
        if (loginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {loginTask.Exception}");
            FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx!.ErrorCode;
    
            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            
            warningLoginText.gameObject.SetActive(true);
            warningLoginText.text = message;
        }
        else
        {
            User = loginTask.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            UISwitcher.Instance.CloseAuth();
        }
    }

    private IEnumerator Register(string email, string password, string username)
    {
        warningNicknameText.text = "";
        warningEmailText.text = "";
        warningPasswordText.text = "";
        warningPasswordConfirmText.text = "";
        if (username == "")
        {
            warningNicknameText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            warningPasswordConfirmText.text = "Password Does Not Match!";
        }
        else
        {
            Task<AuthResult> registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {registerTask.Exception}");
                FirebaseException firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx!.ErrorCode;
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        warningEmailText.text = "Missing Email";
                        break;
                    case AuthError.InvalidEmail:
                        warningEmailText.text = "Invalid Email";
                        break;
                    case AuthError.MissingPassword:
                        warningPasswordText.text = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        warningPasswordText.text = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        warningEmailText.text = "Email Already In Use";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                User = registerTask.Result.User;

                if (User != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = username };

                    Task profileTask = User.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate: () => profileTask.IsCompleted);

                    if (profileTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
                        FirebaseException firebaseEx = profileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningNicknameText.text = "Username Set Failed!";
                    }
                    else
                    {
                        // зарегались, теперь нужно войти
                        UISwitcher.Instance.LoginOn();

                        // подтверждение учетки через почту, можно вырезать
                        FirebaseUser user = auth.CurrentUser;
                        if (user != null) {
                            user.SendEmailVerificationAsync().ContinueWith(task => {
                                if (task.IsCanceled) {
                                    Debug.LogError("SendEmailVerificationAsync was canceled.");
                                    return;
                                }
                                if (task.IsFaulted) {
                                    Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                                    return;
                                }

                                Debug.Log("Email sent successfully.");
                            });
                        }
                    }
                }
            }
        }
    }
}
