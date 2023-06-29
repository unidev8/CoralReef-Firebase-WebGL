using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class videoclipview : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string videoUrl;
    public RawImage rawImage;
    IEnumerator Start()
    {
        if (videoPlayer == null || rawImage == null || string.IsNullOrEmpty(videoUrl))
            yield break;

        videoPlayer.url = videoUrl;
        videoPlayer.renderMode = VideoRenderMode.APIOnly;
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return new WaitForSeconds(1);

        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
