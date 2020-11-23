using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // public const Material defaultMaterial = this.GetComponent<Material>();
    private bool isChoose = false;
    public Material defaultMaterial;
    public bool IsChoose {
        set { isChoose = value; }
        get { return isChoose; }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Rigidbody>() == null) {
            gameObject.AddComponent<Rigidbody>();
        }
        if(this.tag == "Picture") {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        if (GetComponent<Renderer>() && GetComponent<Renderer>().material) {
            defaultMaterial = GetComponent<Renderer>().material;
        } else {
            defaultMaterial = GameController.gameController.MaterialEntity[GameController.indexDefault];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isChoose) {
            GetComponent<MeshRenderer>().material = GameController.gameController.MaterialEntity[GameController.indexChoose];
        } else {
            GetComponent<MeshRenderer>().material = defaultMaterial;
        }
    }

    private void OnCollisionEnter(Collision other) {
        // print('a');
    }
}
