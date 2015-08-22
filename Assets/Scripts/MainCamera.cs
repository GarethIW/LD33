using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{
    //public Hero Hero;
    public Vector3 Offset;
    public Vector3 Target;
    public float Speed;

    private float starFade = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    //if (Room != null && Room.Active)
	    //{
	    //    Target = Room.transform.position + Offset;
	    //}

	    transform.position = Vector3.Lerp(transform.position, Target, Time.deltaTime*Speed);

	    //starFade = Mathf.Lerp(starFade, Hero.IsInside ? 0f : 1f, Time.deltaTime* 3f);
        FadeStarfield(starFade);
	}

    private void FadeStarfield(float fade)
    {
        for (int s = 0; s < transform.FindChild("Starfield").childCount; s++)
        {
            Transform star = transform.FindChild("Starfield").GetChild(s);
            MeshRenderer[] rends = star.GetComponentsInChildren<MeshRenderer>();

            foreach(MeshRenderer r in rends)
                r.material.SetColor("_EmissionColor", new Color(fade,fade,fade));
        }
    }
}
