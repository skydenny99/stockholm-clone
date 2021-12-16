using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climate : MonoBehaviour {
    public enum Weather
    {
        clean,
        cloudy,
        rainy,
        snowy
    }
    public GameObject mainCamera;

    public Color cleanSky;
    public Color cloudySky;
    public Color rainySky;
    public Color snowySky;
    public float colorChangeTime = 5f;

    //컴포넌트 오브젝트로 넣기
    public GameObject cloud;
    public GameObject snow;
    public GameObject rain;
    public ParticleSystem cloudParticle;
    public ParticleSystem snowParticle;
    public ParticleSystem rainParticle;
    
    private float cloudRateVelocity;
    private float rainRateVelocity;
    private float snowRateVelocity;
    //rain
    public int maxRainRate = 100;
    public int minRainRate = 5;
    public float increaseRainRateSpeed = 10f;
    //snow
    public int maxSnowRate = 100;
    public int minSnowRate = 5;
    public float increaseSnowRateSpeed = 15f;
    //cloud
    public int maxCloudRate = 12;
    public int minCloudRate = 1;
    public float increaseCloudRateSpeed = 20f;
 //   public Color darkCloudColor;

    public Weather currentWeather = Weather.clean;
    public Weather previousWeather = Weather.clean;

    private float cloudTimer = 0;
    private float colorTimer = 0;
    private Transform _transform;

    
	// Use this for initialization
	void Start()
	{
		mainCamera = Camera.main.gameObject;
		Debug.Assert(mainCamera);
		Debug.Assert(cloud);
		Debug.Assert(snow);
		Debug.Assert(rain);
		_transform = transform;
		ParticleSystem.EmissionModule emi;
		cloudParticle = cloud.GetComponent<ParticleSystem>();
		emi = cloudParticle.emission;
		emi.rateOverTime = minCloudRate;
		snowParticle = snow.GetComponent<ParticleSystem>();
		emi = snowParticle.emission;
		emi.rateOverTime = minSnowRate;
		rainParticle = rain.GetComponent<ParticleSystem>();
		emi = rainParticle.emission;
		emi.rateOverTime = minRainRate;
	}

	// Update is called once per frame
	void Update () {
        if(currentWeather == Weather.cloudy)
            cloudTimer += Time.deltaTime;
        _transform.position = mainCamera.transform.position;
        createWeather();

        /*if (Input.GetKeyDown(KeyCode.Space))
            changeWeather(Weather.snowy);
        if (Input.GetKeyDown(KeyCode.Z))
            changeWeather(Weather.clean);*/
	}

    public void changeWeather(Weather w)
    {
        if (currentWeather == w)
            return;
        if(cloudTimer < increaseCloudRateSpeed/2 && (w == Weather.rainy || w == Weather.snowy))
        {
            changeWeather(Weather.cloudy);
            return;
        }

        if(currentWeather != Weather.cloudy)
            cloudTimer = 0;
        previousWeather = currentWeather;
        currentWeather = w;
        ParticleSystem.EmissionModule emi;
        int temp = 0;
        switch (currentWeather)
        {
            case Weather.clean:
                return;
            case Weather.cloudy:
                temp = minCloudRate;
                emi = cloudParticle.emission;
                break;
            case Weather.rainy:
                temp = minRainRate;
                cloudTimer = 0;
                emi = rainParticle.emission;
                rainParticle.Play();
                break;
            case Weather.snowy:
                temp = minSnowRate;
                cloudTimer = 0;
                emi = snowParticle.emission;
                snowParticle.Play();
                break;
        }
        emi.rateOverTime = new ParticleSystem.MinMaxCurve(temp);
        
    }

    void createWeather()
    {
        if (currentWeather == Weather.cloudy)
        {
            clouding();
        }
        else if (currentWeather == Weather.rainy)
        {
            clouding();
            raining();
        }
        else if (currentWeather == Weather.snowy)
        {
            clouding();
            snowing();
        }
        else
            clean();
    }
    private void clean()
    {
        ParticleSystem.EmissionModule emi = cloudParticle.emission;
        if(emi.rateOverTime.constant > minCloudRate)
        {
            emi.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.SmoothDamp(emi.rateOverTime.constant, minCloudRate - 0.5f, ref cloudRateVelocity, increaseCloudRateSpeed/7));
        }
        emi = rainParticle.emission;
        if (emi.rateOverTime.constant > minRainRate)
        {
            emi.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.SmoothDamp(emi.rateOverTime.constant, minRainRate - 0.5f, ref rainRateVelocity, increaseCloudRateSpeed/10));
        }
        else if(rainParticle.isPlaying)
        {
            rainParticle.Stop();
        }
        emi = snowParticle.emission;
        if (emi.rateOverTime.constant > minSnowRate)
        {
            emi.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.SmoothDamp(emi.rateOverTime.constant, minSnowRate - 0.5f, ref snowRateVelocity, increaseCloudRateSpeed/10));
        }
        else if(snowParticle.isPlaying)
        {
            snowParticle.Stop();
        }
    }

    private void raining()
    {
        ParticleSystem.EmissionModule emi = rainParticle.emission;
        if(emi.rateOverTime.constant < maxRainRate)
            emi.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.SmoothDamp(emi.rateOverTime.constant, maxRainRate+0.5f, ref rainRateVelocity, increaseRainRateSpeed));
    }

    private void snowing()
    {
        ParticleSystem.EmissionModule emi = snowParticle.emission;
        if (emi.rateOverTime.constant < maxSnowRate)
            emi.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.SmoothDamp(emi.rateOverTime.constant, maxSnowRate + 0.5f, ref snowRateVelocity, increaseSnowRateSpeed));

    }

    private void clouding()
    {
        ParticleSystem.EmissionModule emi = cloudParticle.emission;
        if (emi.rateOverTime.constant < maxCloudRate)
            emi.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.SmoothDamp(emi.rateOverTime.constant, maxCloudRate + 0.5f, ref cloudRateVelocity, increaseCloudRateSpeed));
    }

    public Color weatherColor()
    {
        Color c = Color.white;
        switch (currentWeather)
        {
            case Weather.clean:
                c = cleanSky;
                break;
            case Weather.cloudy:
                c = cloudySky;
                break;
            case Weather.rainy:
                c = rainySky;
                break;
            case Weather.snowy:
                c = snowySky;
                break;
        }
        if (previousWeather == currentWeather)
        {
            return c;
        }
        else
        {
            colorTimer += Time.deltaTime;
            if(colorChangeTime < colorTimer)
            {
                previousWeather = currentWeather;
                colorTimer = 0;
                return c;
            }
            Color preC = Color.white;
            switch (previousWeather)
            {
                case Weather.clean:
                    preC = cleanSky;
                    break;
                case Weather.cloudy:
                    preC = cloudySky;
                    break;
                case Weather.rainy:
                    preC = rainySky;
                    break;
                case Weather.snowy:
                    preC = snowySky;
                    break;
            }
            Color result = Color.Lerp(preC, c, colorTimer / colorChangeTime);
            return result;
        }
        
    }
}
