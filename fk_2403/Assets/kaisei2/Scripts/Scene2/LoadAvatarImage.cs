using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadAvatarImage : MonoBehaviour
{
    [SerializeField] GameObject Avatar;
    [SerializeField] GameObject IconPrefab;

    [SerializeField] Transform[] parent;

    List<SpriteRenderer> costumeImgs = new List<SpriteRenderer>();

    public TempAvatarStorage tempAvatarStorage = new TempAvatarStorage();

    void Start()
    {
        //エラー出てる
        LoadCostumeDataFromFirebase(UserDataManager.instance.uid);
    }
    //アバターの画像のコンポーネントを持ってくる
    public void GetImage()
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(i + "*" + Avatar.transform.GetChild(i).name);
            costumeImgs.Add(Avatar.transform.GetChild(i).GetComponent<SpriteRenderer>());
        }
    }
    //ボタン押す方のアイコンを生成する
    public void CreateCostumeIcon()
    {
        string path = "Costumes";
        for (int i = 0; i < 5; i++)
        {
            Sprite[] SpriteDatas = Resources.LoadAll<Sprite>(path + "/" + i);
            foreach (Sprite sprite in SpriteDatas)
            {
                GameObject ins = Instantiate(IconPrefab, parent[i]);
                ins.GetComponent<Image>().sprite = sprite;
                ins.GetComponent<CostumePath>().SpritePath = path + "/" + i + "/" + sprite.name;
                ins.GetComponent<CostumePath>().ID = i;
            }
        }
    }
    //メインのアバターの方の画像を変更する
    public void ChangeCostumeImage()
    {
        for (int i = 0; i < 5; i++)
        {
            string path = tempAvatarStorage.CostumePaths[i];
            Sprite sprite = Resources.Load<Sprite>(path);
            costumeImgs[i].sprite = sprite;
        }
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
        for (int i = 0; i < costumeKeys.Length; i++)
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

                // 全データ取得後に画面に反映
                ChangeCostumeImage();
            });
        }
    }

}
