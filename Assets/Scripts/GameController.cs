using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    public static GameController gameController;
    private bool editMode = false;
    private bool exitMode = false;
    private bool menuMode = false;
    public GameObject menuCanvas;
    public GameObject editCanvas;
    public GameObject editX;
    public GameObject editY;
    public GameObject editZ;
    public GameObject editRotation;
    public GameObject editPictureWidth;
    public GameObject editPictureHeight;
    public GameObject editImage;
    private float x = 0;
    private float y = 0;
    private float z = 0;
    private float rotate = 0;
    private float pictureWidth = 0;
    private float pictureHeight = 0;
    public float X {
        get { return x; }
    }
    public float Y {
        get { return y; }
    }
    public float Z {
        get { return z; }
    }
    public float Rotate {
        get { return rotate; }
    }
    public float PictureWidth {
        get { return pictureWidth; }
    }
    public float PictureHeight {
        get { return pictureHeight; }
    }
    
    private Vector3 defaultTmpEntityPosition = new Vector3(0,0,3);
    public Material[] MaterialEntity;
    public const int indexChoose = 0;
    public const int indexSuccess = 1;
    public const int indexErr = 2;
    public const int indexDefault = 3;
    public bool EditMode {
        set { editMode = value;}
        get { return editMode; }
    }
    public bool ExitMode {
        set { exitMode = value;}
        get { return exitMode; }
    }
    public bool MenuMode {
        set { menuMode = value;}
        get { return menuMode; }
    }
    public Vector3 DefaultTmpEntityPosition {
        get { return defaultTmpEntityPosition; }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(gameController == null) {
            gameController = GetComponent<GameController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && exitMode) {
            perspectiveMode();
		}
        if(Input.GetButtonDown("Cancel") ) {
			exitMode = true;
            editMode = menuMode = false;
		}
        if(Input.GetButtonDown("Menu")){
			menuMode = !menuMode;
            exitMode = editMode = false;
		}
        if(Input.GetButtonDown("Edit") && Player.player.ChoosedEntity.collider != null && Player.player.ChoosedEntity.collider.gameObject.GetComponent<Entity>().IsChoose){
			editMode = !editMode;
            exitMode = menuMode = false;
            if(Player.player.ChoosedEntity.collider.gameObject.tag == "Picture") {
                // Bật edit scale
                editCanvas.GetComponent<Transform>().Find("Panel").Find("Panel").Find("PanelScale").gameObject.SetActive(true);
                var pictureWidth = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().localScale.x;
                var pictureHeight = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().localScale.y;
                editPictureWidth.GetComponent<Transform>().Find("SliderWidth").gameObject.GetComponent<Slider>().value = pictureWidth;
                editPictureHeight.GetComponent<Transform>().Find("SliderHeight").gameObject.GetComponent<Slider>().value = pictureHeight;
                editPictureWidth.GetComponent<Transform>().Find("InputWidth").gameObject.GetComponent<InputField>().text = pictureWidth.ToString();
                editPictureHeight.GetComponent<Transform>().Find("InputHeight").gameObject.GetComponent<InputField>().text = pictureHeight.ToString();

                // Tắt input không dùng
                editZ.GetComponent<Transform>().Find("SliderZ").gameObject.GetComponent<Slider>().enabled = false;
                editRotation.GetComponent<Transform>().Find("SliderRotation").gameObject.GetComponent<Slider>().enabled = false;
                editY.GetComponent<Transform>().Find("SliderY").gameObject.GetComponent<Slider>().enabled = true;
            } else {
                editCanvas.GetComponent<Transform>().Find("Panel").Find("Panel").Find("PanelScale").gameObject.SetActive(false);
                editY.GetComponent<Transform>().Find("SliderY").gameObject.GetComponent<Slider>().enabled = false;
                editZ.GetComponent<Transform>().Find("SliderZ").gameObject.GetComponent<Slider>().enabled = true;
                editRotation.GetComponent<Transform>().Find("SliderRotation").gameObject.GetComponent<Slider>().enabled = true;
            }
            x = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().position.x;
            y = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().position.y;
            z = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().position.z;
            rotate = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().rotation.y;
            editX.GetComponent<Transform>().Find("SliderX").gameObject.GetComponent<Slider>().value = x;
            editY.GetComponent<Transform>().Find("SliderY").gameObject.GetComponent<Slider>().value = y;
            editZ.GetComponent<Transform>().Find("SliderZ").gameObject.GetComponent<Slider>().value = z;
            editRotation.GetComponent<Transform>().Find("SliderRotation").gameObject.GetComponent<Slider>().value = rotate;

            editX.GetComponent<Transform>().Find("InputFieldX").gameObject.GetComponent<InputField>().text = x.ToString();
            editY.GetComponent<Transform>().Find("InputFieldY").gameObject.GetComponent<InputField>().text = y.ToString();
            editZ.GetComponent<Transform>().Find("InputFieldZ").gameObject.GetComponent<InputField>().text = z.ToString();
            editRotation.GetComponent<Transform>().Find("InputRotation").gameObject.GetComponent<InputField>().text = rotate.ToString();
		}
        menuCanvas.SetActive(menuMode);
        editCanvas.SetActive(editMode);
        if(editMode && Player.player.ChoosedEntity.collider != null && Player.player.ChoosedEntity.collider.gameObject.GetComponent<Entity>().IsChoose) {
            // rotate = editRotation.GetComponent<Transform>().Find("SliderRotation").gameObject.GetComponent<Slider>().value;
            // Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().localEulerAngles  = new Vector3(0,rotate,0);
            changePosition();
        }
    }
    public void perspectiveMode() {
        editMode = exitMode = menuMode = false;
    }
    public void toggleMenuMode() {
        menuMode = !menuMode;
        exitMode = editMode = false;
    }
    public void toggleEditMode() {
        editMode = !editMode;
        exitMode = menuMode = false;
    }

    public void setEditInputValue() {
        if(Player.player.ChoosedEntity.collider.gameObject.tag == "Picture") {
            pictureWidth = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().localScale.x;
            pictureHeight = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().localScale.y;

            editPictureWidth.GetComponent<Transform>().Find("InputWidth").gameObject.GetComponent<InputField>().text = pictureWidth.ToString();
            editPictureHeight.GetComponent<Transform>().Find("InputHeight").gameObject.GetComponent<InputField>().text = pictureHeight.ToString();
        }
        x = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().position.x;
        y = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().position.y;
        z = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().position.z;
        rotate = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().rotation.y;

        // editX.GetComponent<Transform>().Find("SliderX").gameObject.GetComponent<Slider>().value = x;
        // editY.GetComponent<Transform>().Find("SliderY").gameObject.GetComponent<Slider>().value = y;
        // editZ.GetComponent<Transform>().Find("SliderZ").gameObject.GetComponent<Slider>().value = z;
        // editRotation.GetComponent<Transform>().Find("SliderRotation").gameObject.GetComponent<Slider>().value = rotate;

        editX.GetComponent<Transform>().Find("InputFieldX").gameObject.GetComponent<InputField>().text = x.ToString();
        editY.GetComponent<Transform>().Find("InputFieldY").gameObject.GetComponent<InputField>().text = y.ToString();
        editZ.GetComponent<Transform>().Find("InputFieldZ").gameObject.GetComponent<InputField>().text = z.ToString();
        editRotation.GetComponent<Transform>().Find("InputRotation").gameObject.GetComponent<InputField>().text = rotate.ToString();
    }
    public void changePosition() {
        if(editMode && Player.player.ChoosedEntity.collider != null && Player.player.ChoosedEntity.collider.gameObject.GetComponent<Entity>().IsChoose) {
            if(Player.player.ChoosedEntity.collider.gameObject.tag == "Picture") {
                // Bật edit scale
                pictureWidth = editPictureWidth.GetComponent<Transform>().Find("SliderWidth").gameObject.GetComponent<Slider>().value;
                pictureHeight = editPictureHeight.GetComponent<Transform>().Find("SliderHeight").gameObject.GetComponent<Slider>().value;
                Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().localScale = new Vector3(pictureWidth,pictureHeight,Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().localScale.z);
            } else {
                 rotate = editRotation.GetComponent<Transform>().Find("SliderRotation").gameObject.GetComponent<Slider>().value;
                // Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().position = Vector3.MoveTowards(Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().position, new Vector3(x,y,z), 3 * Time.deltaTime);
                var rx = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().eulerAngles.x;
                var rz = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().eulerAngles.z;
                Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().localEulerAngles  = new Vector3(rx,rotate,rz);
            }
            x = editX.GetComponent<Transform>().Find("SliderX").gameObject.GetComponent<Slider>().value;
            y = editY.GetComponent<Transform>().Find("SliderY").gameObject.GetComponent<Slider>().value;
            z = editZ.GetComponent<Transform>().Find("SliderZ").gameObject.GetComponent<Slider>().value;
            var px = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().position.x;
            var py = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().position.y;
            var pz = Player.player.ChoosedEntity.collider.gameObject.GetComponent<Transform>().position.z;

            Player.player.ChoosedEntity.collider.gameObject.GetComponent<Rigidbody>().velocity = (new Vector3(x,0,z) - new Vector3(px, 0, pz));
            setEditInputValue();
        }
    }
}
