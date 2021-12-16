using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public enum DayCycle
    {
        morning,
        noon,
        sunset,
        night
    }
    public GameObject mainCamera;
    public float horizonThickness = 2; // unity unit

    public GameObject stars;
    public GameObject sun;
    public GameObject background;
    public float sec2hour = 10;
    public float startTime = 0;
    public float currentTime = 0; // 0 ~ 24
    public DayCycle cycle = DayCycle.night;

    public float starSpeedRate = 2;
    private float _starRotateSpeed;
    private float _timer = 0;
    private Transform _transform;

    public Gradient dayNight;
    public int morningStartTime;
    public int noonStartTime;
    public int sunsetStartTime;
    public int nightStartTime;
    public SpriteRenderer spriteRenderer;

    public GameObject sunset;
    public float sunsetHeight;
    public float sunsetAngle;


    //climate
    public GameObject climateObject;
    private Climate _climate;
    public float currentLightPenetrationEffect = 0f;
    public float maxLightPenetrationEffect = 0.85f;
    public float minLightPenetrationEffect = 0.3f;
    private float lightPenetrationEffectV;

    //Red Moon
    public Color redMoonNight = Color.red;
    public float redMoonColorRate = 0.8f;
    public int date;
    public int redMoonDate = -1;
    public bool isRedMoon = false;
    public GameObject moon;
    public Sprite redMoonSprite;
    public Sprite normalMoonSprite;

    // Use this for initialization
    void Start()
    {
        _timer = startTime * sec2hour;
        redMoonDate = -9;
		_transform = transform;
		mainCamera = Camera.main.gameObject;
		Debug.Assert(mainCamera);
		Debug.Assert(stars);
		Debug.Assert(sun);
		Debug.Assert(background);
		spriteRenderer = background.GetComponent<SpriteRenderer>();
		Debug.Assert(spriteRenderer);
		Debug.Assert(sunset);
		Debug.Assert(climateObject);
		_climate = climateObject.GetComponent<Climate>();
		noonStartTime = morningStartTime + (int)(sunsetAngle / 15) + 1;
		nightStartTime = sunsetStartTime + (int)(sunsetAngle / 15) + 1;
		_starRotateSpeed = sec2hour * starSpeedRate;
		_transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y - (mainCamera.GetComponent<Camera>().orthographicSize - horizonThickness), transform.position.z);
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            setRedMoon();
        _transform.position = new Vector3(mainCamera.transform.position.x, _transform.position.y, _transform.position.z);
        background.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, _transform.position.z);
        _timer += Time.deltaTime;

        date = ModuleManager.GetInstance().GetModule<GameModule>().dayCount;
        if(isRedMoon&&isRedMoon&&date > redMoonDate)
        {
            isRedMoon = false;
            moon.GetComponent<SpriteRenderer>().sprite = normalMoonSprite;
        } else if(!isRedMoon&& redMoonDate == date)
        {
            isRedMoon = true;
            moon.GetComponent<SpriteRenderer>().sprite = redMoonSprite;
        }
        selfRotate();
        changeColor();
        sunsetElevate();
        dayCycle();
    }

    /// <summary>
    /// 하늘의 색깔을 바꾸는 함수입니다.
    /// </summary>
    void changeColor()
    {
        if(cycle == DayCycle.night)
            currentLightPenetrationEffect = maxLightPenetrationEffect;
        else if(cycle == DayCycle.noon)
            currentLightPenetrationEffect = minLightPenetrationEffect;
        else if (cycle == DayCycle.morning)
        {
           currentLightPenetrationEffect = Mathf.SmoothDamp(currentLightPenetrationEffect ,minLightPenetrationEffect, ref lightPenetrationEffectV, ((noonStartTime - morningStartTime) * sec2hour)/3);
        }
        else if(cycle == DayCycle.sunset)
        {
            currentLightPenetrationEffect = Mathf.SmoothDamp(currentLightPenetrationEffect, maxLightPenetrationEffect, ref lightPenetrationEffectV, ((nightStartTime - sunsetStartTime) * sec2hour)/3);
        }
        //sprite.color = dayNight.Evaluate(map(currentTime, 0, 24, 0, 1));
        Color sky = Color.Lerp(_climate.weatherColor(), dayNight.Evaluate(map(currentTime, 0, 24, 0, 1)), currentLightPenetrationEffect);
        
        //TODO : 이부분을 코루틴으로 바꿔서 자연스럽게 보간
        if (isRedMoon&&cycle == DayCycle.night)
        {
            sky = Color.Lerp(sky, redMoonNight, redMoonColorRate);
        }
        spriteRenderer.color = sky;
    }

    /// <summary>
    /// 해와 별이 돌게 함수입니다.
    /// </summary>
    void selfRotate()
    {
        currentTime = (_timer / sec2hour) % 24;
        sun.transform.rotation = Quaternion.Euler(0, 0, -currentTime * 15);
        stars.transform.rotation = Quaternion.Euler(0, 0, -(_timer / _starRotateSpeed) * 15);
    }

    /// <summary>
    /// 지금이 아침, 낮, 저녁, 밤인지 바꿔주는 함수입니다.
    /// </summary>
    void dayCycle()
    {
        if (currentTime < morningStartTime || currentTime >= nightStartTime)
            cycle = DayCycle.night;
        else if (currentTime >= sunsetStartTime)
            cycle = DayCycle.sunset;
        else if (currentTime >= noonStartTime)
            cycle = DayCycle.noon;
        else
            cycle = DayCycle.morning;
    }

    /// <summary>
    /// 아침과 저녁에 석양이 지게만드는 함수입니다.
    /// </summary>
    void sunsetElevate()
    {
        float temp = 0;
        if (Mathf.Abs(morningStartTime - currentTime) < sunsetAngle / 15)
        {
            temp = morningStartTime;
        }
        else if (Mathf.Abs(sunsetStartTime - currentTime) < sunsetAngle / 15)
        {
            temp = sunsetStartTime;
        }
        else
            return;
        sunset.transform.localPosition = new Vector3(sunset.transform.localPosition.x, Mathf.Lerp(sunsetHeight, 0, (Mathf.Abs(temp - currentTime)/sunsetAngle)*15), sunset.transform.localPosition.z); ;
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }   

    

    /// <summary>
    /// 디버깅용 시간설정 함수입니다.
    /// </summary>
    /// <param name="time">바꾸고싶은 시간으로 넣습니다.</param>
    public void setCurrentTime(float time)
    {
        currentTime = time;
    }

    /// <summary>
    /// 현재 시간을 가져오는 함수입니다.
    /// </summary>
    /// <returns>현재 시간을 hour로 가져옵니다.</returns>
    public float getCurrentTime()
    {
        return currentTime;
    }

    public bool isDayCycle(DayCycle day)
    {
        if (cycle == day)
            return true;
        return false;
    }

    public void setRedMoon()
    {
        if(!(cycle == DayCycle.night))
        {
            redMoonDate = date;
        }
        else
        {
            redMoonDate = date + 1;
        }
        

    }
    
}
