using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatorLoad : MonoBehaviour
{
    // アバターパーツの参照（目、口、眉毛、髪、アクセサリー）
    [SerializeField] private GameObject avatar;
    [SerializeField] private SpriteRenderer eyes;
    [SerializeField] private SpriteRenderer mouth;
    [SerializeField] private SpriteRenderer eyebrow;
    [SerializeField] private SpriteRenderer hair;
    [SerializeField] private SpriteRenderer accessories;

    public TempAvatarStorage tempAvatarStorage = new TempAvatarStorage();


    void Start()
    {
        LoadCostumeDataFromFirebase(UserDataManager.instance.uid);
    }
    // Firebaseからコスチュームデータを読み込む
    public void LoadCostumeDataFromFirebase(string userId)
    {
        // FirebaseManagerのインスタンスを取得
        FirebaseManager firebaseManager = FirebaseManager.instance;

        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManagerが初期化されていません。");
            return;
        }

        // 各コスチュームのデータをFirebaseから取得
        string[] costumeKeys = { "eyes", "mouth", "eyebrow", "hair", "accessories" };
        int totalKeys = costumeKeys.Length;
        int completedRequests = 0; // 完了したリクエストの数を追跡

        for (int i = 0; i < totalKeys; i++)
        {
            string path = $"{userId}/{costumeKeys[i]}";

            // FirebaseManagerのReadDataを使用してデータを取得
            int index = i; // forループ内でクロージャを防ぐためにインデックスをローカル変数にコピー
            firebaseManager.ReadData(path, (result) =>
            {
                if (result != "error" && result != "NoData")
                {
                    // 取得したコスチュームのデータをtempAvatarStorageに保存
                    tempAvatarStorage.CostumePaths[index] = result;
                    Debug.Log($"コスチュームデータ取得: {costumeKeys[index]} = {result}");
                }
                else
                {
                    Debug.LogWarning($"データ取得失敗またはデータが存在しない: {costumeKeys[index]}");
                }

                // リクエスト完了カウントを増加
                completedRequests++;

                // すべてのリクエストが完了したときに、コスチューム画像を変更
                if (completedRequests == totalKeys)
                {
                    ChangeCostumeImage();
                }
            });
        }
    }

    // メインのアバターの方の画像を変更する
    private void ChangeCostumeImage()
    {
        avatar.SetActive(false);
        string[] paths = tempAvatarStorage.CostumePaths;

        // 目のスプライトを変更
        Sprite eyeSprite = Resources.Load<Sprite>(paths[0]);
        if (eyeSprite != null)
        {
            eyes.sprite = eyeSprite;
        }

        // 口のスプライトを変更
        Sprite mouthSprite = Resources.Load<Sprite>(paths[1]);
        if (mouthSprite != null)
        {
            mouth.sprite = mouthSprite;
        }

        // 眉毛のスプライトを変更
        Sprite eyebrowSprite = Resources.Load<Sprite>(paths[2]);
        if (eyebrowSprite != null)
        {
            eyebrow.sprite = eyebrowSprite;
        }

        // 髪のスプライトを変更
        Sprite hairSprite = Resources.Load<Sprite>(paths[3]);
        if (hairSprite != null)
        {
            hair.sprite = hairSprite;
        }

        // アクセサリーのスプライトを変更
        Sprite accessoriesSprite = Resources.Load<Sprite>(paths[4]);
        if (accessoriesSprite != null)
        {
            accessories.sprite = accessoriesSprite;
        }

        Debug.Log("アバターのスプライトが変更されました。");
        avatar.SetActive(true);
    }
}
