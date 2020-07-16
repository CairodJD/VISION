using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureDivider : MonoBehaviour {


    // Use this for initialization
    void Start() {

        
    }


    public void divide(Texture2D source , int tilesize) {
        GameObject spritesRoot = GameObject.Find("SpritesRoot");
        List<Sprite> test = new List<Sprite>();

        for (int i = 0; i < 20; i++) {
            for (int j = 0; j < 15; j++) {
                Sprite newSprite = Sprite.Create(source, new Rect(i * tilesize, j * tilesize, tilesize, tilesize), new Vector2(0.5f, 0.5f));
                
                GameObject n = new GameObject();
                SpriteRenderer sr = n.AddComponent<SpriteRenderer>();
                sr.sprite = newSprite;
                n.transform.localScale = Vector3.one * tilesize;
                n.transform.position = new Vector3(i * 2, j * 2, 0);
                n.transform.parent = spritesRoot.transform;
                test.Add(newSprite);
            }
        }

        Debug.Log(test.Count);
    }
}