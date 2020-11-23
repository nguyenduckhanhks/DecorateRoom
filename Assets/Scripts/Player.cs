using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player player;
    private GameObject TmpEntity;
    public GameObject target;
    private RaycastHit choosedEntity;
    private RaycastHit choosedWall;
    private RaycastHit[] hitEntity;
    private bool isChooseWall = false;
    private bool isChooseEntity = false;
    public RaycastHit ChoosedEntity{
        get { return choosedEntity; }
    }
    public RaycastHit ChoosedWall {
        get { return choosedWall; }
    }
    public bool IsChooseWall {
        get { return isChooseWall; }
    }
    public bool IsChooseEntity {
        get { return isChooseEntity; }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(player == null) {
            player = this.GetComponent<Player>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        isChooseWall = false;
        isChooseEntity = false;
        hitEntity = Physics.RaycastAll(transform.position, target.GetComponent<Transform>().position - gameObject.transform.position, 100f);
        if(hitEntity.Length > 0) {
            if((hitEntity[0].collider.gameObject.tag == "Entity" || hitEntity[0].collider.gameObject.tag == "Picture")) {
                isChooseEntity = true;
            }
            for (int i = 0; i < hitEntity.Length; i++) {
                if(hitEntity[i].collider.gameObject.tag == "Wall") {
                    choosedWall = hitEntity[i];
                    isChooseWall = true;
                    break;
                }
            }
        }
        if(Input.GetButtonDown("Fire1") && !GameController.gameController.EditMode && !GameController.gameController.ExitMode && !GameController.gameController.MenuMode && TmpEntity == null) {
            if(choosedEntity.collider != null) {
                choosedEntity.collider.gameObject.GetComponent<Entity>().IsChoose = false;
            }
            if (isChooseEntity && (hitEntity[0].collider.gameObject.tag == "Entity" || hitEntity[0].collider.gameObject.tag == "Picture")) {
                choosedEntity = hitEntity[0];
                choosedEntity.collider.gameObject.GetComponent<Entity>().IsChoose = isChooseEntity;
            }
        }
        if(Input.GetButtonDown("Delete") && !GameController.gameController.EditMode && !GameController.gameController.ExitMode && !GameController.gameController.MenuMode && TmpEntity == null) {
            if(choosedEntity.collider.gameObject.GetComponent<Entity>().IsChoose) {
                Destroy(choosedEntity.collider.gameObject);
            }
        }
    }
    public void renderTmpEntity(GameObject tmpEntity) {
        if(TmpEntity != null) {
            Destroy(TmpEntity);
        }
        Vector3 position = GameController.gameController.DefaultTmpEntityPosition;
        if(tmpEntity.tag == "TmpPicture" && choosedWall.collider != null) {
            Vector3 vectorz = new Vector3(0,0,1);
            position = choosedWall.collider.gameObject.GetComponent<Transform>().position - new Vector3(0,0,choosedWall.collider.gameObject.GetComponent<Transform>().localScale.z * 0.5f);
        }
        TmpEntity = Instantiate(tmpEntity, GameController.gameController.DefaultTmpEntityPosition, Quaternion.identity);
        TmpEntity.transform.parent =  Camera.main.transform;
        TmpEntity.GetComponent<Transform>().localPosition = position;
        GameController.gameController.perspectiveMode();
    }
}
