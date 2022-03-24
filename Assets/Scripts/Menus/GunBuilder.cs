using UnityEngine;
using UnityEngine.UI;

public class GunBuilder : MonoBehaviour
{
	private bool Auto;
	private bool Multishot;
	private bool SemiAuto;
	private bool Burst;
	private bool BoltAction;
	private string Element;
	private string Projectile;

    private void OnEnable()
    {
		Element = PlayerPrefs.GetString("Element", "Fire");
		Auto = PlayerPrefs.GetInt("Auto", 0) == 1;
		Multishot = PlayerPrefs.GetInt("Multishot", 0) == 1;
		SemiAuto = PlayerPrefs.GetInt("SemiAuto", 1) == 1;
		Burst = PlayerPrefs.GetInt("Burst", 0) == 1;
		BoltAction = PlayerPrefs.GetInt("BoltAction", 0) == 1;
		Projectile = PlayerPrefs.GetString("Projectile", "Orbital");

		GameObject.Find(Element).GetComponent<Toggle>().isOn = true;
		GameObject.Find("Auto").GetComponent<Toggle>().isOn = Auto;
		GameObject.Find("Multishot").GetComponent<Toggle>().isOn = Multishot;
		GameObject.Find("SemiAuto").GetComponent<Toggle>().isOn = SemiAuto;
		GameObject.Find("Burst").GetComponent<Toggle>().isOn = Burst;
		GameObject.Find("BoltAction").GetComponent<Toggle>().isOn = BoltAction;
		GameObject.Find(Projectile).GetComponent<Toggle>().isOn = true;
	}

    //Elements
    public void RubberElement()
	{
		if (GameObject.Find("Rubber").GetComponent<Toggle>().isOn) PlayerPrefs.SetString("Element","Bounce");
	}

	public void FireElement()
	{
		if (GameObject.Find("Fire").GetComponent<Toggle>().isOn) PlayerPrefs.SetString("Element", "Fire");
	}

	public void IceElement()
	{
		if (GameObject.Find("Ice").GetComponent<Toggle>().isOn) PlayerPrefs.SetString("Element", "Ice");
	}

	public void AcidElement()
	{
		if (GameObject.Find("Acid").GetComponent<Toggle>().isOn) PlayerPrefs.SetString("Element", "Acid");
	}

	//Fire Mode
	public void AutoFireMode()
	{
		PlayerPrefs.SetInt("Auto", GameObject.Find("Auto").GetComponent<Toggle>().isOn ? 1 : 0);
	}

	public void MultishotFireMode()
	{
		PlayerPrefs.SetInt("Multishot", GameObject.Find("Multishot").GetComponent<Toggle>().isOn ? 1 : 0);
	}

	public void SemiAutoFireMode()
	{
		PlayerPrefs.SetInt("SemiAuto", GameObject.Find("SemiAuto").GetComponent<Toggle>().isOn ? 1 : 0);
	}

	public void BurstFireMode()
	{
		PlayerPrefs.SetInt("Burst", GameObject.Find("Burst").GetComponent<Toggle>().isOn ? 1 : 0);
	}

	public void BoltActionFireMode()
	{
		PlayerPrefs.SetInt("BoltAction", GameObject.Find("BoltAction").GetComponent<Toggle>().isOn ? 1 : 0);
	}

	//Projectiles
	public void OrbitingProjectile()
	{
		if (GameObject.Find("Orbital").GetComponent<Toggle>().isOn) PlayerPrefs.SetString("Projectile", "Orbital");
	}

	
	public void ReboundProjectile()
	{
		if (GameObject.Find("Rebound").GetComponent<Toggle>().isOn) PlayerPrefs.SetString("Projectile", "Rebound");
	}

	
	public void SniperProjectile()
	{
		if (GameObject.Find("Sniper").GetComponent<Toggle>().isOn) PlayerPrefs.SetString("Projectile", "Sniper");
	}

	public void SnakeProjectile()
	{
		if (GameObject.Find("Snake").GetComponent<Toggle>().isOn) PlayerPrefs.SetString("Projectile", "Snake");
	}

	public void Save()
    {
		PlayerPrefs.Save();
    }
}
