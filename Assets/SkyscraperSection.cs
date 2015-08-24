using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class SkyscraperSection : MonoBehaviour
{
    public float Health = 10f;
    public int DamageCost = 1000;
    public int Score = 5;
    public bool hasBillBoard = false;
    public Material BaseDmgMaterial;
    public Material MidDmgMaterial;
    public Material TopDmgMaterialPlain;
    public Material TopDmgMaterialBillBoard;
    public Material BillBoardDmgMaterialTM;
    public Material BillBoardDmgMaterialLD;
    public Material BillBoardDmgMaterialSH;
    public Material BillBoardWrkMaterialTM;
    public Material BillBoardWrkMaterialLD;
    public Material BillBoardWrkMaterialSH;
    public Material BaseWrkMaterial;
    public Material MidWrkMaterial;
    public Material TopWrkMaterialPlain;
    public Material TopWrkMaterialBillBoard;


   
    private PlaygroundEventC playgroundEvent;

    // Use this for initialization
    void Start()
    {
        playgroundEvent = PlaygroundC.GetEvent(0, PlaygroundC.GetParticles(0));
        playgroundEvent.particleEvent += OnEvent;

      

    }


    void ApplyMaterials(Material baseMaterial, Material midMaterial, Material topMaterial)
    {
        Renderer renderer = GetComponent<Renderer>();

        Color colour = renderer.material.color;

        string materialName = renderer.sharedMaterial.name;
        if (materialName.Contains("base"))
        {
            renderer.sharedMaterial = baseMaterial;


        }
        else if (materialName.Contains("middle"))
        {
            renderer.sharedMaterial = midMaterial;
        }
        else if (materialName.Contains("top"))
        {

            renderer.sharedMaterial = topMaterial;


        }
        renderer.material.SetColor("_Color", colour);
        //Debug.Log("Changing material on " + transform.gameObject.name + " to " + renderer.sharedMaterial.name);

    }




    // Update is called once per frame
    void Update()
    {

        if (Health <= 0f)
        {
            DropAudioPlayer();
            GameManager.Instance.DamageCost += DamageCost;
            GameManager.Instance.score += Score;


            var ex = ExplosionManager.Instance.GetOne("Explosion");
            if (ex != null)
            {
                ex.transform.position = transform.position + new Vector3(0f, 0.1f, -0.5f);
                ex.GetComponent<Explosion>().Init(2f);
            }

            gameObject.SetActive(false);
        }
        else if (Health <= 0.3f)
        {
            Material topMaterial;

            topMaterial = getTopMaterial();



            ApplyMaterials(BaseWrkMaterial, MidWrkMaterial, topMaterial);
            applyBillboardMaterial();
        }
        else if (Health <= 0.6f)
        {
            // GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(Health,Health, Health));  
            Material topMaterial;

            topMaterial = getTopMaterial();

            ApplyMaterials(BaseDmgMaterial, MidDmgMaterial, topMaterial);
            applyBillboardMaterial();


        }
        


    }

    private void DropAudioPlayer()
    {
        GameObject dap = EnemyManager.Instance.GetOne("DroppableAudioPlayer");
        dap.transform.position = transform.position;
      
        dap.SetActive(true);
        dap.GetComponent<AudioSource>().Play();
    }

    private Material getTopMaterial()
    {
        Material topMaterial;
        if (hasBillBoard)
        {
            topMaterial = TopDmgMaterialBillBoard;
        }
        else
        {
            topMaterial = TopWrkMaterialPlain;
        }

        return topMaterial;
    }

    private void applyBillboardMaterial()
    {



        if (hasBillBoard)
        {
            Renderer billBoardRenderer = GetComponentInChildren<Renderer>();
            Color colour = billBoardRenderer.material.color;
            string name = billBoardRenderer.sharedMaterial.name;

            if (name.Contains("dmg-TM"))
            {
                billBoardRenderer.sharedMaterial = BillBoardDmgMaterialTM;
            }
            else if (name.Contains("dmg-LD"))
            {
                billBoardRenderer.sharedMaterial = BillBoardDmgMaterialLD;
            }
            else if (name.Contains("dmg-SH"))
            {
                billBoardRenderer.sharedMaterial = BillBoardDmgMaterialSH;
            }
            else if (name.Contains("wrk-TM"))
            {
                billBoardRenderer.sharedMaterial = BillBoardWrkMaterialTM;
            }
            else if (name.Contains("wrk-LD"))
            {
                billBoardRenderer.sharedMaterial = BillBoardWrkMaterialLD;
            }
            else
            {
                billBoardRenderer.sharedMaterial = BillBoardWrkMaterialSH;
            }
            billBoardRenderer.material.SetColor("_Color", colour);

        }






    }


    void OnEvent(PlaygroundEventParticle particle)
    {
        if (this == null) return;
        if (particle.collisionCollider.gameObject == this.gameObject)
        {
            Health -= 0.00001f;
            if (Random.Range(0, 500) == 0)
            {
                var fire = FireManager.Instance.GetOne("Fire");
                if (fire != null)
                {
                    fire.GetComponent<Fire>().Init(transform, new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.25f, 0.25f), -0.01f));
                    AudioSource fireSource = GetComponent<AudioSource>();
                    if (!fireSource.isPlaying)
                    {
                        fireSource.Play();
                    }
                }
            }
        }
    }

    public void ClimbedOn()
    {
        Health -= 0.1f;
        if (Random.Range(0, 5) == 0)
        {
            var fire = FireManager.Instance.GetOne("Fire");
            if (fire != null)
            {
                fire.GetComponent<Fire>().Init(transform, new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.25f, 0.25f), -0.01f));
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponentInParent<Skyscraper>() != null)
        {
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Skyscraper>() != null)
        {
        }
    }
}
