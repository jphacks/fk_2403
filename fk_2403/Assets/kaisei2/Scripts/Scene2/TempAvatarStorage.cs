using UnityEngine;

public class TempAvatarStorage
{
    public string[] CostumePaths = { "Costumes/0/eye 1", "Costumes/1/kuti 1", "Costumes/2/brow 1", "Costumes/3/hear 1", "Costumes/4/banso-ko-" };

    public void SetUserData()
    {
        // UserDataManager.instanceがnullでないかを確認
        if (UserDataManager.instance != null)
        {
            UserDataManager.instance.SetAvatarDataToServer(CostumePaths);
        }
        else
        {
            Debug.LogError("UserDataManagerのインスタンスが初期化されていません。");
        }
    }
}
