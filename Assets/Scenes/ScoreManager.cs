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
    public void setJumpCount(int jump)
    {
        this.jumpCount = jump;
    }
    public void setFallCount(int fall)
    {
        this.fallcount = fall;
    }
    public void setHoursCount(int hours)
    {
        this.hoursCount = hours;
    }
    public void setMinusCount(int minus)
    {
        this.minutesCount = minus;
    }
    public void setSecondCount(int second)
    {
        this.secondsCount = second;
    }
   
    public int getJumpCount()
    {
        return jumpCount;
    }
    public int getFallCount()
    {
        return fallcount;
    }
    public int getHoursCount()
    {
        return hoursCount;
    }
    public int getMinusCount()
    {
        return minutesCount;
    }
    public int getSecondCount()
    {
        return secondsCount;
    }

    public void ResetTime()
    {
        timeCount = 0;
        secondsCount = 0;
        minutesCount = 0;
        hoursCount = 0;
        timeCountText.text = "00:00:00";
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
    