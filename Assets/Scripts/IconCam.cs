using System.IO;
using UnityEngine;

public class IconCam : MonoBehaviour
{
    public int resolution = 512;
    public string folderName = "GeneratedIcons";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakePhoto();
        }
    }

    void TakePhoto()
    {
        Camera cam = GetComponent<Camera>();

        // 1. Create a Render Texture
        RenderTexture rt = new RenderTexture(resolution, resolution, 24);
        cam.targetTexture = rt;

        // 2. Render the camera's view
        Texture2D screenShot = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
        cam.Render();

        // 3. Read the pixels
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null;

        // 4. Save to file
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = "Icon_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

        // Make sure the folder exists
        string path = Application.dataPath + "/" + folderName;

        // FIX: We explicitly add 'System.IO.' here to prevent the error
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }

        // FIX: We explicitly add 'System.IO.' here as well
        System.IO.File.WriteAllBytes(path + "/" + filename, bytes);

        Debug.Log("Saved Icon to: " + path + "/" + filename);

        // Cleanup
        Destroy(rt);
        Destroy(screenShot);
    }
}
