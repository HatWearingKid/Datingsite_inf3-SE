using UnityEngine;

public class RegisterUser : MonoBehaviour {

    private string email;
    private string password;
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;


    public void setEmail(string _email)
    {
        email = _email;
    }

    public void setPassword(string _password)
    {
        password = _password;
    }

    public void Register()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
				Debug.LogError(task.Exception.InnerExceptions[0].Message);
                return;
            }
            if (task.IsFaulted)
            {
				Debug.LogError(task.Exception.InnerExceptions[0].Message);
                return;
            }

            // Firebase user has been created.
            user = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
				user.DisplayName.ToString(), user.UserId.ToString());

			SendEmail();
			auth.SignOut();
        });
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

				Debug.Log ("Email sent successfully.");
			});
		} else {
			Debug.Log ("Could not send email, something went wrong.");
		}
	}
}