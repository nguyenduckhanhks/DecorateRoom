using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpEntity : MonoBehaviour
{
    private bool isTrigger = false;
    private bool isPictureOnWall = true;
    private MeshRenderer meshRender;
    public GameObject Entity;
    public bool IsTrigger {
        set { isTrigger = value; }
        get { return isTrigger; }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<MeshCollider>() == null) {
            gameObject.AddComponent<MeshCollider>();
        }
        GetComponent<MeshCollider>().convex = true;
        GetComponent<MeshCollider>().isTrigger = true;
        meshRender = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.LookRotation(new Vector3(0,1,0));
        if(this.tag == "TmpPicture") {
            if(Player.player.IsChooseWall) {
                isPictureOnWall = true;
                this.transform.position = Player.player.ChoosedWall.point;
                this.transform.rotation = Player.player.ChoosedWall.collider.gameObject.GetComponent<Transform>().rotation;
            } else {
                isPictureOnWall = false;
                resetPosition();
            }
        }
        if(isTrigger || !isPictureOnWall) {
            meshRender.material = GameController.gameController.MaterialEntity[GameController.indexErr];
        } else {
            meshRender.material = GameController.gameController.MaterialEntity[GameController.indexSuccess];
        }
        if (Input.GetButtonDown("Fire1") && !GameController.gameController.EditMode && !GameController.gameController.ExitMode && !GameController.gameController.MenuMode)
            renderEntity();
        if (Input.GetButtonDown("Quit") && !GameController.gameController.EditMode && !GameController.gameController.ExitMode && !GameController.gameController.MenuMode)
            Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other) {
        if(other.tag == "Entity" || other.tag == "Picture") {
            isTrigger = true;
        }
        if(this.tag != "TmpPicture") {
            if(other.tag == "Wall") {
                isTrigger = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Entity" || other.tag == "Picture") {
            isTrigger = false;
        }
        if(this.tag != "TmpPicture") {
            if(other.tag == "Wall") {
                isTrigger = false;
            }
        }
    }

    public void renderEntity(){
        if(!isTrigger){
            if(this.tag == "TmpPicture") {
                if(!Player.player.IsChooseWall) {
                    return;
                }
            }
            GameObject entity = Instantiate(Entity, this.gameObject.transform.position, Quaternion.identity);
            entity.GetComponent<Transform>().rotation = this.transform.rotation;
        }
    }

    public void resetPosition() {
        this.transform.localPosition = GameController.gameController.DefaultTmpEntityPosition;
    }
}
