using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ScoreManager:MonoBehaviour
{
    public TextMeshProUGUI jumpCountText, fallCountText, timeCountText;

    private int jumpCount, fallcount,hoursCount,minutesCount,secondsCount,timeCount;
    private string hoursText,minutesText,secondsText;

    public void OnJump()
    {
        jumpCount++;
        jumpCountText.text = "Jump: "+jumpCount.ToString();
    } 
    public void OnFall()
    {
        fallcount++;
        fallCountText.text = "Fall: "+fallcount.ToString();
    }
    public void PlayerTime()
    {
        timeCount++;
        secondsCount = timeCount % 60;
        minutesCount= (timeCount / 60)%60;
        hoursCount= (timeCount / 3600)%60;
        if (secondsCount<10) secondsText = "0" + secondsCount.ToString();
        else secondsText = secondsCount.ToString();
        if (minutesCount < 10) minutesText   = "0" + minutesCount.ToString();
        else minutesText = minutesCount.ToString();
        if (hoursCount < 10) hoursText = "0" + hoursCount.ToString();
        else hoursText = hoursCount.ToString();
        timeCountText.text = hoursText + ":" + minutesText + ":" + secondsText;
    }
}
    