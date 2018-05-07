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

	public void RegisterUserOnClick(){
		Register();
		GetCurrentUser();
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
        });
    }

	public void GetCurrentUser(){
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		Firebase.Auth.FirebaseUser user = auth.CurrentUser;
		auth.StateChanged += AuthStateChanged;
		AuthStateChanged(this, null);
	}

	void AuthStateChanged(object sender, System.EventArgs eventArgs) {
		if (auth.CurrentUser != user) {
			bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
			if (!signedIn && user != null) {
				Debug.Log("Signed out " + user.UserId);
			}
			user = auth.CurrentUser;
			if (signedIn) {
				Debug.Log("Signed in " + user.IsEmailVerified);
			}
		}
	}
}