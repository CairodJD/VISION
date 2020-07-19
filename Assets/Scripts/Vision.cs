using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;



public class Vision : MonoBehaviour {



    VideoCapture webcam;
    Mat image;

    private void Start() {
        webcam = new VideoCapture();
        
    }


    private void Update() {

        if (GameManager.gamestarted) {
            image = webcam.QueryFrame();
            CvInvoke.Imshow("wecam", image);
            CvInvoke.WaitKey(24);
        }
        

        //CvInvoke.Imshow("wecam filtre 2", filtre(ref imgHSV, 2));
        //if (Input.GetKeyDown(KeyCode.Space)) {
        //    Debug.Log(image.Width + "  " + image.Height);
        //    Texture2D test = GetTexture();
        //    //prite[] splitted = splitText(test, image);
        //    //divider.divide(test,32);
        //}


    }

    Mat filtre(ref Mat src ,int type =0) {
        Mat output = new Mat();

        switch (type) {
            case 0:
                CvInvoke.Blur(src, output, src.Size, new Point(-1, -1));
                break;
            case 1:
                CvInvoke.MedianBlur(src, output,11);
                break;
            case 2:
                CvInvoke.GaussianBlur(src, output, src.Size,3.159d);
                break;
        }

        return output;
    } 




    Sprite[] splitText(Texture2D tosplit, Mat img) {
        List<Sprite> splits = new List<Sprite>(16);
        int tilezise = 1;
        for (int x = 0; x < 20; x++) {
            for (int y = 0; y <15; y++) {
                Sprite newSprite = Sprite.Create(tosplit,
                    new Rect(x * tilezise, y * tilezise, tilezise, tilezise),
                    new Vector2(0.5f, 0.5f));
                splits.Add(newSprite);
            }
        }
        


        return splits.ToArray();
    }


    public Texture2D GetTexture(int scale = 2) {
        CvInvoke.Resize(image, image, new Size(image.Width / scale, image.Height/scale));

        CvInvoke.Flip(image, image, FlipType.Horizontal);
        Texture2D texture = new Texture2D(image.Width, image.Height, TextureFormat.RGBA32, false);
        texture.LoadRawTextureData(image.ToImage<Rgba, Byte>().Bytes);
        texture.Apply();

        return texture;
    }


    private void OnDestroy() {
        webcam.Dispose();
        CvInvoke.DestroyAllWindows();
    }
}