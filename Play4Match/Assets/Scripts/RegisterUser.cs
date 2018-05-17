using UnityEngine;

public class RegisterUser : MonoBehaviour {

    private string email;
    private string password;
	private string confirmPassword;
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	Toast toast = new Toast();


    public void setEmail(string _email)
    {
        email = _email;
    }

    public void setPassword(string _password)
    {
        password = _password;
    }

	public void setConfirmationPassword(string _confirmPassword)
	{
		confirmPassword = _confirmPassword;
	}

    public void Register()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		if (password.Equals (confirmPassword)) {
			auth.CreateUserWithEmailAndPasswordAsync (email, password).ContinueWith (task => {
				if (task.IsCanceled) {
					toast.MyShowToastMethod (task.Exception.InnerExceptions [0].Message);
					return;
				}
				if (task.IsFaulted) {
					toast.MyShowToastMethod (task.Exception.InnerExceptions [0].Message);
					return;
				}

				// Firebase user has been created.
				user = task.Result;
				SendEmail ();
				auth.SignOut ();
			});
		} else {
			toast.MyShowToastMethod ("The passwords do not match.");
			Debug.Log ("The passwords do not match.");
		}
    }

	public void SendEmail(){
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;
		if (user != null) {
			user.SendEmailVerificationAsync ().ContinueWith (task => {
				if (task.IsCanceled) {
					Debug.LogError (task.Exception.InnerExceptions [0].Message);
					return;
				}
				if (task.IsFaulted) {
					Debug.LogError (task.Exception.InnerExceptions [0].Message);
					return;
				}

				toast.MyShowToastMethod ("A verification email has been sent, please activate your account.");
			});
		} else {
			toast.MyShowToastMethod ("Could not send email, something went wrong.");
		}
	}
}