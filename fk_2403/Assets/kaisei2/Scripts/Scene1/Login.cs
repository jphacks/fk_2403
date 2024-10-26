using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;

public class Login : MonoBehaviour
{
    // ログインをするためのスクリプト
    public async Task<bool> SignIn(FirebaseAuth auth, string email, string password)
    {
        try
        {
            var userCredential = await auth.SignInWithEmailAndPasswordAsync(email, password);
            var user = userCredential?.User; // サインインしたユーザー情報を取得
            if (user != null && user.Email == email)
            {
                Debug.Log($"サインインに成功しました。ユーザーID: {user.UserId}");
                UserDataManager.instance.uid = user.UserId;
                return true; // 成功した場合、trueを返す
            }
            else
            {
                Debug.LogWarning("サインイン情報が一致しませんでした。");
                return false; // 失敗した場合、falseを返す
            }
        }
        catch (System.Exception ex)
        {
            //Debug.LogError("サインインに失敗しました: " + ex.Message);
            return false; // 例外が発生した場合も、falseを返す
        }
    }
}
