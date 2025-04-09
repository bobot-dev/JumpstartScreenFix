using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColourBlindGetWithHslStuff: MonoBehaviour
{
	void Start()
	{
		UpdateColor();
	}

	void OnEnable()
	{
		UpdateColor();
	}

	public void UpdateColor()
	{
		if (!_gotTarget)
		{
			GetTarget();
		}
		var color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[variationNumber];

		Color.RGBToHSV(color, out float h, out float s, out float l);
		color = Color.HSVToRGB(h * hMult, s * sMult, l* lMult);
		
		if (_rend)
		{
			_rend.GetPropertyBlock(_block);
			_block.SetColor("_CustomColor1", color);
			_rend.SetPropertyBlock(_block);
			return;
		}
		if (_img)
		{
			_img.color = color;
		}
		if (_txt)
		{
			_txt.color = color;
		}
		if (_lit)
		{
			_lit.color = color;
		}
		if (_sr)
		{
			_sr.color = color;
		}
	}

	void GetTarget()
	{
		_gotTarget = true;
		if (customColorRenderer)
		{
			_rend = GetComponent<Renderer>();
			_block = new MaterialPropertyBlock();
		}
		_img = GetComponent<Image>();
		_txt = GetComponent<TMP_Text>();
		_lit = GetComponent<Light>();
		_sr = GetComponent<SpriteRenderer>();
	}

	public float hMult = 1;
	public float sMult = 1;
	public float lMult = 1;


	Image _img;
	Light _lit;
	SpriteRenderer _sr;
	TMP_Text _txt;

	bool _gotTarget;

	public int variationNumber;

	public bool customColorRenderer;

	Renderer _rend;
	MaterialPropertyBlock _block;
}

