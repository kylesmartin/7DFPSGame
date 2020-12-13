using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Public variables
    public static UIManager instance;
    public float[] rows;
    public float[] columns;
    public GameObject importantPicBackground;
    public Texture2D square;
    public GameObject menu; // default off
    public GameObject gallery; // default off
    public Button galleryBackButton;
    public Transform images;  
    public Button prevBtn;
    public Button nextBtn;
    public GameObject photoCard; // starts off
    public Image enlargedPic;
    public Text keyObjectText;
    public Button closePhotoCardBtn;
    public GameObject start; // starts on
    public Button viewObjectivesBtn;
    public Button viewPicturesBtn;
    public GameObject objectives; // starts off
    public Text objectiveList;
    public Button objectivesBackButton;
    public Image interactor;
    public GameObject desktop;
    public GameObject startScreen;
    public GameObject passwordEntry;

    // Private variables
    private List<GameObject> pictures;
    private int currentRow = 0;
    private int currentColumn = 0;
    private int numPhotos = 0;
    private int numPages = 1;
    private int currentPage = 0;
    private int numPicsPerPage = 18;
    private Vector3 pictureScale;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start() 
    {
        pictures = new List<GameObject>();
        ResetMenu(4);
        ResetDesktop(false, true, false);
        nextBtn.interactable = false;
        prevBtn.interactable = false;
    }

    // Update is called once per frame
    void Update() {
        // manage prev btn
        if (currentPage == 0)
        {
            prevBtn.interactable = false;
        } else
        {
            prevBtn.interactable = true;
        }
        // manage next btn
        if (currentPage == numPages - 1)
        {
            nextBtn.interactable = false;
        } else
        {
            nextBtn.interactable = true;
        }
        // keep track of picture scale
        if (pictures.Count > 0)
        {
            pictureScale = pictures[0].GetComponent<RectTransform>().localScale;
            foreach (GameObject _pic in pictures)
            {
                _pic.GetComponent<RectTransform>().localScale = pictureScale;
                if (_pic.GetComponent<Picture>().background != null) _pic.GetComponent<Picture>().background.GetComponent<RectTransform>().localScale = pictureScale;
            }
        }
        // write objectives
        objectiveList.text = "";
        foreach (KeyObject _object in GameManager.instance.keyObjectsFound) objectiveList.text += _object.keyObjectName + "\n\n";
        // set interactor color
        SetInteractor();
    }

    public void TakePicture(KeyObject _keyObject)
    {
        // The Render Texture in RenderTexture.active is the one that will be read by ReadPixels.
        RenderTexture _currentRT = RenderTexture.active;
        RenderTexture.active = GameManager.instance.cam.targetTexture;
        // Render the camera's view.
        GameManager.instance.cam.Render();
        // Make a new texture and read the active Render Texture into it.
        Texture2D _image = new Texture2D(GameManager.instance.cam.targetTexture.width, GameManager.instance.cam.targetTexture.height);
        _image.ReadPixels(new Rect(0, 0, GameManager.instance.cam.targetTexture.width, GameManager.instance.cam.targetTexture.height), 0, 0);
        _image.Apply();
        // Replace the original active Render Texture.
        RenderTexture.active = _currentRT;
        // Read new texture into sprite
        Sprite _sprite = Sprite.Create(_image, new Rect(0, 0, _image.width, _image.height), new Vector2(0.5f, 0.5f));
        // Create image from sprite
        GameObject _background = null;
        if (_keyObject != null)
        {
            GameManager.instance.keyObjectsFound.Add(_keyObject);
            _background = new GameObject();
            _background.transform.SetParent(images);
            Sprite _backsprite = Sprite.Create(square, new Rect(0, 0, square.width, square.height), new Vector2(0.5f, 0.5f));
            _background.AddComponent<Image>();
            _background.GetComponent<Image>().sprite = _backsprite;
            _background.GetComponent<Image>().color = Color.green;
            _background.GetComponent<RectTransform>().sizeDelta = new Vector2(115f, 115f);
            _background.transform.localPosition = new Vector3(columns[currentColumn], rows[currentRow], 0);
        }
        GameObject _picture = new GameObject();
        _picture.transform.SetParent(images);
        _picture.transform.localPosition = new Vector3(columns[currentColumn], rows[currentRow], 0);
        IncrementRowAndColumn();
        _picture.AddComponent<Image>();
        _picture.GetComponent<Image>().sprite = _sprite;
        _picture.AddComponent<Button>();
        _picture.GetComponent<Button>().onClick.AddListener(delegate { OpenPhotoCard(_picture); });
        _picture.AddComponent<Picture>();
        _picture.GetComponent<Picture>().InitPicture(numPhotos, _keyObject, _background);
        pictures.Add(_picture);
        numPages = System.Convert.ToInt32(Mathf.Floor((numPhotos / numPicsPerPage) + 1));
        numPhotos++;
    }

    public void IncrementRowAndColumn()
    {
        if (currentColumn == columns.Length - 1 && currentRow == rows.Length - 1)
        {
            currentColumn = 0;
            currentRow = 0;
            numPages++;
        } else if (currentColumn == columns.Length - 1)
        {
            currentColumn = 0;
            currentRow++;
        } else
        {
            currentColumn++;
        }
    }

    public void ChangePage(int _dir)
    {
        DisplayPage(currentPage + _dir);
    }

    public void DisplayPage(int _pageId)
    {
        currentPage = _pageId;
        foreach (GameObject _pic in pictures)
        {
            _pic.SetActive(false);
            if (_pic.GetComponent<Picture>().background != null) _pic.GetComponent<Picture>().background.SetActive(false);
        }
        int _start = currentPage * numPicsPerPage;
        for (int i = _start; i < _start + numPicsPerPage; i++)
        {
            if (i < pictures.Count)
            {
                pictures[i].SetActive(true);
                if (pictures[i].GetComponent<Picture>().background != null) pictures[i].GetComponent<Picture>().background.SetActive(true);
            }
        }
    }

    public void OpenPhotoCard(GameObject _pic)
    {
        enlargedPic.sprite = _pic.GetComponent<Image>().sprite;
        if (_pic.GetComponent<Picture>().keyObject != null)
        {
            keyObjectText.text = _pic.GetComponent<Picture>().keyObject.keyObjectName;
        } 
        else
        {
            keyObjectText.text = "";
        }
        photoCard.SetActive(true);
    }

    public void ClosePhotoCard()
    {
        photoCard.SetActive(false);
    }

    public void ResetMenu(int _mask)
    {
        // bits: menu, start, gallery, objectives
        bool _menuCond = (_mask & 8) >> 3 == 1;
        bool _startCond = (_mask & 4) >> 2 == 1;
        bool _galleryCond = (_mask & 2) >> 1 == 1;
        bool _objectivesCond = (_mask & 1) == 1;
        // menu settings
        menu.SetActive(_menuCond);
        // start settings
        start.SetActive(_startCond);
        // gallery settings
        gallery.SetActive(_galleryCond);
        photoCard.SetActive(false);
        DisplayPage(0);
        // objectives settings
        objectives.SetActive(_objectivesCond);
    }

    public void SetInteractor()
    { 
        if (GameManager.instance.isAiming || GameManager.instance.isPaused)
        {
            interactor.color = Color.clear;
        }
        else
        {
            interactor.color = Color.white;
            int _layerMask = 1 << 9;
            _layerMask = ~_layerMask;
            RaycastHit _hit;
            if (Physics.Raycast(GameManager.instance.mainCamera.transform.position, GameManager.instance.mainCamera.transform.forward, out _hit, 5f, _layerMask))
            {
                if (_hit.collider.tag == "openable")
                {
                    interactor.color = Color.green;
                    if (Input.GetKeyDown(KeyCode.E)) _hit.collider.gameObject.GetComponentInParent<OpenAnimation>().FlipOpenState();
                }
                if (_hit.collider.name == "BasementKey")
                {
                    interactor.color = Color.green;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        GameManager.instance.inventory.Add(_hit.collider.gameObject);
                        _hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
                if (_hit.collider.name == "ComputerScreen")
                {
                    interactor.color = Color.green;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Debug.Log("Turning on computer light");
                        TurnOnComputer();
                    }
                }
            }
        }
    }

    public void TurnOnComputer()
    {
        Debug.Log("Launching Computer UI");
        ResetDesktop(true, true, false);
        GameManager.instance.isPaused = true;
    }

    public void ResetDesktop(bool _desktop, bool _passwordEntry, bool _startScreen)
    {
        desktop.SetActive(_desktop);
        passwordEntry.SetActive(_passwordEntry);
        startScreen.SetActive(_startScreen);
    }
}
