using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireEffect : MonoBehaviour
{

	private float defaultFireStartSpeed_;
	private float defaultFireStartSizeMin_;
	private float defaultFireStartSizeMax_;

	private float defaultSmokeStartSpeed_;
	private float defaultSmokeStartSizeMin_;
	private float defaultSmokeStartSizeMax_;

	private float defaultSparkStartSpeedMin_;
	private float defaultSparkStartSpeedMax_;
	private float defaultSparkStartSizeMin_;
	private float defaultSparkStartSizeMax_;

	private float defaultPointLightRange_;

	public GameObject attackFire;
	public GameObject smoke;
	public GameObject sparks;
	public GameObject pointLight;

	private ParticleSystem fireParticleSystem_;
	private ParticleSystem smokeParticleSystem_;
	private ParticleSystem sparksParticleSystem_;
	private Light pointLight_;


	// Use this for initialization
	void Start()
	{
		fireParticleSystem_ = GetComponent<ParticleSystem>();
		smokeParticleSystem_ = smoke.GetComponent<ParticleSystem>();
		sparksParticleSystem_ = sparks.GetComponent<ParticleSystem>();
		pointLight_ = pointLight.GetComponent<Light>();

		defaultFireStartSpeed_ = fireParticleSystem_.main.startSpeed.constant;
		defaultFireStartSizeMin_ = fireParticleSystem_.main.startSize.constantMin;
		defaultFireStartSizeMax_ = fireParticleSystem_.main.startSize.constantMax;

		defaultSmokeStartSpeed_ = smokeParticleSystem_.main.startSpeed.constant;
		defaultSmokeStartSizeMin_ = smokeParticleSystem_.main.startSize.constantMin;
		defaultSmokeStartSizeMax_ = smokeParticleSystem_.main.startSize.constantMax;

		defaultSparkStartSpeedMin_ = sparksParticleSystem_.main.startSpeed.constantMax;
		defaultSparkStartSpeedMax_ = sparksParticleSystem_.main.startSpeed.constantMin;
		defaultSparkStartSizeMin_ = sparksParticleSystem_.main.startSize.constantMin;
		defaultSparkStartSizeMax_ = sparksParticleSystem_.main.startSize.constantMax;

		defaultPointLightRange_ = pointLight_.range;

		attackFire.SetActive(false);
	}

	public void SetFireStrength(float strength)
	{
		strength = Mathf.Clamp(Mathf.Pow(strength, 1.3f), 0, 1);
		// 불이 클 수록 큰폭으로 줄어들도록 보간

		var main = fireParticleSystem_.main;
		main.startSpeed = defaultFireStartSpeed_ * strength;
		main.startSize = new ParticleSystem.MinMaxCurve(defaultFireStartSizeMin_ * strength, defaultFireStartSizeMax_ * strength);

		main = smokeParticleSystem_.main;
		main.startSpeed = defaultSmokeStartSpeed_ * strength;
		main.startSize = new ParticleSystem.MinMaxCurve(defaultSmokeStartSizeMin_ * strength, defaultSmokeStartSizeMax_ * strength);

		main = sparksParticleSystem_.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(defaultSparkStartSpeedMin_ * strength, defaultSparkStartSpeedMax_ * strength);
		main.startSize = new ParticleSystem.MinMaxCurve(defaultSparkStartSizeMin_ * strength, defaultSparkStartSizeMax_ * strength);

		pointLight_.range = defaultPointLightRange_ * strength;
	}

	public void ShowAttackMotion()
	{
		attackFire.SetActive(true);
		Invoke("HideAttackMotion", 1f);
	}
	public void HideAttackMotion()
	{
		attackFire.SetActive(false);
	}
}
