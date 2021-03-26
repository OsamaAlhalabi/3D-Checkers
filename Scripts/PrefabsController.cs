using System.Collections;
using UnityEngine;

public class PrefabsController : MonoBehaviour
{
	protected bool ready = false;
	protected Material tileMaterial;
	protected new Renderer renderer;

	public bool IsReady()
	{
		return ready;
	}
	public Material getTileMaterial()
	{
		return renderer.material;
	}
	public void Start()
	{
		ready = false;
		renderer = GetComponent<Renderer>();
		tileMaterial = GetComponent<Renderer>().material;
	}
	public void UnHighLightSquare()
	{
		setMaterial(tileMaterial);
	}
	public void setMaterial(Material material)
	{
		renderer.material = material;
	}

	
	public void ScaleIn(float startAfter, float speed, Vector3 endScales)
	{
		StartCoroutine(IEScaleIn(startAfter, speed, endScales));
	}
	public IEnumerator ScaleOut()
	{
		yield return StartCoroutine(IEDelete());
		Destroy(gameObject);
	}
	public IEnumerator IEScaleIn(float startAfter, float speed, Vector3 endScales)
	{
		yield return new WaitForSeconds(startAfter);
		ready = false;
		float t = Time.deltaTime * speed;
		float scale = 1f;
		while (t < Mathf.PI / 2)
		{
			t += Time.deltaTime * speed;
			scale = 1f * Mathf.Sin(t);
			transform.localScale = endScales * scale;
			yield return null;
		}
		transform.localScale = endScales * scale;
		ready = true;
	}
	public IEnumerator IEDelete()
	{
		float t = Mathf.PI / 2;
		float scale = 1f;
		Vector3 orignalScale = transform.localScale;
		while (t < Mathf.PI)
		{
			t += Time.deltaTime * 3.0f;
			scale = 1f * Mathf.Sin(t);
			transform.localScale = orignalScale * scale;
			yield return null;
		}
	}
}
