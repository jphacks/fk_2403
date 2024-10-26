using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    
    //アバターの画像のコンポーネントを持ってくる
    public void GetImage() 
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(i+"*"+Avatar.transform.GetChild(i).name);
            costumeImgs.Add(Avatar.transform.GetChild(i).GetComponent<SpriteRenderer>());
        }  
    }
    //ボタン押す方のアイコンを生成する
    public void CreateCostumeIcon()
    {
        string path = "Costumes";
        for(int i = 0; i < 5; i++){
            Sprite[] SpriteDatas = Resources.LoadAll<Sprite>(path + "/" + i);
            foreach(Sprite sprite in SpriteDatas)
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
        for(int i = 0; i < 5; i++){
            string path = tempAvatarStorage.CostumePaths[i];
            Sprite sprite = Resources.Load<Sprite>(path);
            costumeImgs[i].sprite = sprite;
        }
    }

    //ストレージのデータを書き換える
    public void SetCostume(int index, string path){
        tempAvatarStorage.CostumePaths[index] = path;
    }
}
