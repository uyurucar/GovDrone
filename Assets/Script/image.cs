using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class image : MonoBehaviour
{
    // Take a shot immediately
    public void Screenshot()
    {
        StartCoroutine(UploadPNG());
    }

    IEnumerator UploadPNG()
    {
        // We should only read the screen buffer after rendering is complete
        yield return new WaitForEndOfFrame();

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = ImageConversion.EncodeToPNG(tex);
        Object.Destroy(tex);

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes("/sdcard/Download/" + "SavedScreen.png", bytes);

    }
}