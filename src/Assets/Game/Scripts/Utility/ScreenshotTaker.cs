using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    public string screenshotFileName = "screenshot";
    private int screenshotCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        string fileName = screenshotFileName + "_" + screenshotCount + ".png";

        ScreenCapture.CaptureScreenshot(fileName);

        screenshotCount++;

        Debug.Log("Скриншот сохранен: " + fileName);
    }
}