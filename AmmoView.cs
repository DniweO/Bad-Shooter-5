using System;
using TMPro;
using UnityEngine;
using Zenject;

public class AmmoView : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _ammoText;

	private Pistol _pistol;

	[Inject]
	private void Construct(Pistol pistol)
	{
		_pistol = pistol;
	}

	private void OnEnable()
	{
		Pistol pistol = _pistol;
		pistol.OnBulletCountChanged = (Action)Delegate.Combine(pistol.OnBulletCountChanged, new Action(OnBulletCountChanged));
		OnBulletCountChanged();
	}

	private void OnDisable()
	{
		Pistol pistol = _pistol;
		pistol.OnBulletCountChanged = (Action)Delegate.Remove(pistol.OnBulletCountChanged, new Action(OnBulletCountChanged));
	}

	private void OnBulletCountChanged()
	{
		string text = "<sprite=0>\n";
		string text2 = "";
		for (int i = 0; i < _pistol.BulletsCount; i++)
		{
			text2 += text;
		}
		_ammoText.text = text2;
	}
}
