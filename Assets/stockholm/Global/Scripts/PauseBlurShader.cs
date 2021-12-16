using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 일시정지 했을 때 카메라 블러 효과를 주는 스크립트입니다.
/// </summary>
[ExecuteInEditMode]
public class PauseBlurShader : MonoBehaviour
{
	public Shader blurShader;
	public bool blurOn = false;
	[Range(0, 10)]
	public int iter = 0;
	[Range(0.0f, 1.0f)]
	public float blurSpread = 0f;

	public Shader pixelShader;
	public bool pixelOn = false;
	[Range(0.0001f, 1.3f)]
	public float cellSize = 0.0001f;
	[Range(0, 8)]
	public int colorBits = 8;

	private Material mat_;
	private Material mat2_;

	// 유니티 내장 함수
	void Awake()
	{
		if (pixelShader != null)
		{
			mat_ = new Material(pixelShader);
			mat_.hideFlags = HideFlags.HideAndDontSave;
		}

		if (blurShader != null)
		{
			mat2_ = new Material(blurShader);
			mat2_.hideFlags = HideFlags.DontSave;
		}
	}

	// 유니티 내장 함수
	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		// Blur
		RenderTexture bluredBuf;
		if (blurShader != null && blurOn == true)
		{
			int rtW = src.width;
			int rtH = src.height;
			bluredBuf = RenderTexture.GetTemporary(rtW, rtH, 0);

			//DownSample4x(src, bluredBuf);
			Graphics.Blit(src, bluredBuf);

			for (int i = 0; i < iter; i++)
			{
				RenderTexture tempBuf = RenderTexture.GetTemporary(rtW, rtH, 0);
				FourTapCone(bluredBuf, tempBuf, i);
				RenderTexture.ReleaseTemporary(bluredBuf);
				bluredBuf = tempBuf;
			}

		}
		else
		{
			bluredBuf = RenderTexture.GetTemporary(src.width, src.height, 0);
			Graphics.Blit(src, bluredBuf);
		}

		// Pixel
		RenderTexture pixeledBuf = RenderTexture.GetTemporary(bluredBuf.width, bluredBuf.height, 0);
		if (pixelShader != null && pixelOn == true)
		{

			mat_.SetFloat("_CellSize", cellSize * 0.01f);
			mat_.SetFloat("_ColorBits", colorBits);

			Graphics.Blit(bluredBuf, pixeledBuf, mat_);
		}
		else
		{
			Graphics.Blit(bluredBuf, pixeledBuf);
		}
		RenderTexture.ReleaseTemporary(bluredBuf);

		Graphics.Blit(pixeledBuf, dest);
		RenderTexture.ReleaseTemporary(pixeledBuf);
	}

	/// <summary>
	/// PauseBlur를 적용합니다.
	/// </summary>
	/// <param name="duration">적용하는데 걸리는 시간입니다.</param>
	public void ApplyPauseBlur(float duration = 0.3f)
	{
		blurOn = true;
		pixelOn = true;

		DOTween.To(() => iter, x => iter = x, 10, duration / 2);
		DOTween.To(() => cellSize, x => cellSize = x, 1.3f, duration / 2);
	}

	/// <summary>
	/// PauseBlur를 적용해제합니다.
	/// </summary>
	/// <param name="duration">적용해제하는데 걸리는 시간입니다.</param>
	public void DiscardPauseBlur(float duration = 0.3f)
	{
		DOTween.To(() => iter, x => iter = x, 0, duration / 2).OnComplete(() => blurOn = false);
		DOTween.To(() => cellSize, x => cellSize = x, 0.0001f, duration / 2).OnComplete(() => pixelOn = false);
	}

	#region Blur 관련 함수들
	// ----- Blur 관련 함수들 -----
	// Performs one blur iteration.
	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		float off = 0.5f + iteration * blurSpread;
		Graphics.BlitMultiTap(source, dest, mat2_,
							   new Vector2(-off, -off),
							   new Vector2(-off, off),
							   new Vector2(off, off),
							   new Vector2(off, -off)
			);
	}

	// Downsamples the texture to a quarter resolution.
	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		float off = 1.0f;
		Graphics.BlitMultiTap(source, dest, mat2_,
							   new Vector2(-off, -off),
							   new Vector2(-off, off),
							   new Vector2(off, off),
							   new Vector2(off, -off)
			);
	}
	#endregion
}
