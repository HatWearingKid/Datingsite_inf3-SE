using UnityEngine;

public class RegisterUser : MonoBehaviour {

    private string email;
    private string password;

    public void setEmail(string _email)
    {
        email = _email;
    }

    public void setPassword(string _password)
    {
        password = _password;
    }

    public void RegisterUserOnClick()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
				Debug.LogError("Something went wrong, please try again later " + task.Exception);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("An account with this username already exists " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
				newUser.DisplayName.ToString(), newUser.UserId.ToString());
        });
    }
}