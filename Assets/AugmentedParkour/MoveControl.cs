using System;
using UnityEngine;
using UnityEngine.UI;
using ArowMain.Runtime;
using ArowSample.Scripts.Runtime;

public class MoveControl : MonoBehaviour
{
    enum STATE_AROW_MAP
    {
        NONE,
        DOWNLOAD_AROW_MAP,		// サーバーからarowmapデータをダウンロード.
        SETTING_MAP,
        LOADING_AROW_MAP,		// ダウンロードしたデータの読み込み.
        CREATE_ROAD,			// 道の生成.
        CREATE_BUILDING,		// 建物の生成.
        SETTING_POI_OBSERVSER,
        SETTING_PARKOUR_OBJECT, // このゲームで用いるオブジェクトの配置
        PLAYING_GAME,			//
    };

    private bool[] isStateEnd;

    private bool isCreate3DGround = true;
    private float groundHeightScale = 5f;
    private bool isCreateRoadObjOrTex = true;
    private bool isHeightColor = false;
    private bool isToonLighting = false;
    private bool isCreateBuilding = true;
    private bool useConfigAsset = true;

    private bool useLandmarkConfig = true;
    private bool usePoiConfig = true;

    private Game game;

    private STATE_AROW_MAP state = STATE_AROW_MAP.NONE;

    // Setting Inspector
    public Text StatusText;
    public GameObject WarkerObj;

    // Use this for initialization
    void Start()
    {
        state = STATE_AROW_MAP.DOWNLOAD_AROW_MAP;
        isStateEnd = new bool[Enum.GetNames(typeof(STATE_AROW_MAP)).Length];

        for (int i = 0; i < isStateEnd.Length; i++)
        {
            isStateEnd[i] = false;
        }

            game = gameObject.AddComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case STATE_AROW_MAP.DOWNLOAD_AROW_MAP:
                // stremingAssetの中のデータを使用しているので、サーバーからのダウンロードは行なっていない
                state = STATE_AROW_MAP.SETTING_MAP;
                break;

            case STATE_AROW_MAP.SETTING_MAP:
                break;

            case STATE_AROW_MAP.LOADING_AROW_MAP:
                //
                game.Initialize();
                state = STATE_AROW_MAP.CREATE_ROAD;
                break;

            case STATE_AROW_MAP.CREATE_ROAD:
                if (isCreate3DGround)
                {
                    //　３D地面生成
                    game.CreateGround(groundHeightScale, isCreateRoadObjOrTex, isToonLighting, isHeightColor);
                }

                if (isCreateRoadObjOrTex && !isCreate3DGround)
                {
                    // 平面の道生成（GameObjectの道生成）
                    game.CreateRoads();
                }

                state = STATE_AROW_MAP.CREATE_BUILDING;
                break;

            case STATE_AROW_MAP.CREATE_BUILDING:
                if (isCreateBuilding)
                {
                    // 建物の生成
                    if (useConfigAsset)
                    {
                        game.CreateBuildingFromConfigAsset(useLandmarkConfig, usePoiConfig);
                    }
                    else
                    {
                        game.CreateBuildingsAttachmentCollider();
                    }
                }

                state = STATE_AROW_MAP.SETTING_POI_OBSERVSER;
                break;

            case STATE_AROW_MAP.SETTING_POI_OBSERVSER:
                game.InitializePoiObserver();
                state = STATE_AROW_MAP.SETTING_PARKOUR_OBJECT;
                break;
            case STATE_AROW_MAP.SETTING_PARKOUR_OBJECT:
                GameLogic gameLogic = this.gameObject.AddComponent<GameLogic>();
                AddDebugObjects(gameLogic);
                state = STATE_AROW_MAP.PLAYING_GAME;
                break;
            case STATE_AROW_MAP.PLAYING_GAME:
                break;
            default:
                Debug.Assert(false);
                break;
        }

        StatusText.text = state.ToString();
    }

    [SerializeField] GameObject CheckPoint = null;
    [SerializeField] GameObject Goal = null;
    private void AddDebugObjects(GameLogic gameLogic)
    {
        Debug.Log("called ");
        Vector3 pos = GameObject.Find("unitychan").transform.position;
        if (CheckPoint)
        {
            GameObject checkpoint = Instantiate(CheckPoint, new Vector3(pos.x + 1, 75, pos.z), Quaternion.identity);
            checkpoint.GetComponent<CheckPointController>().gameLogic = gameLogic;
            checkpoint.SetActive(true);
        }
        if (Goal)
        {
            GameObject goal = Instantiate(Goal, new Vector3(pos.x - 1, 75, pos.z), Quaternion.identity);
            goal.GetComponent<GoalController>().gameLogic  = gameLogic;
            goal.SetActive(true);
        }
    }

    private void OnGUI()
    {
        const float ToggleLeftButtonX = 50;
        const float ToggleTopButtonY = 170f;
        const float BetweenX = 110f;
        const float BetweenY = 60f;
        float x = ToggleLeftButtonX;
        float y = ToggleTopButtonY;

        switch (state)
        {
            case STATE_AROW_MAP.SETTING_MAP:
                if (GUI.Button(new Rect(400, 400, 100, 50), "test button"))
                {
                    Timer instance = this.gameObject.GetComponent<Timer>();
                    if (instance != null)
                    {
                        Debug.Log("CurrentTime");
                        Debug.Log(instance.CurrentTime);
                        Debug.Log("Enabled ?");
                        Debug.Log(instance.countdownEnabled);
                        instance.countdownEnabled = true;
                    }
                    else
                    {
                        instance = this.gameObject.AddComponent<Timer>();
                        instance.SetDelegate(delegate ()
                        {
                            Debug.Log("callback called !");
                        });
                        Debug.Log("CurrentTime");
                        Debug.Log(instance.CurrentTime);
                        Debug.Log("SetCurrentTime");
                        instance.CurrentTime = 5.0f;
                        Debug.Log(instance.CurrentTime);
                        Debug.Log(instance.CurrentTime);
                    }

                }

                if (GUI.Button(new Rect(50, 50, 100, 50), isCreate3DGround ? "立体地面" : "平面地面"))
                {
                    isCreate3DGround = !isCreate3DGround;
                }

                if (isCreate3DGround)
                {
                    groundHeightScale = GUI.HorizontalSlider(new Rect(50, 110, 100, 50), groundHeightScale, 1f, 20f);
                    GUI.Label(new Rect(155, 110, 100, 50), "標高強調:" + groundHeightScale.ToString("00.0"));
                }

                if (GUI.Button(new Rect(x, y, 100, 50), isCreateRoadObjOrTex ? "道　作成" : "道　未作成"))
                {
                    isCreateRoadObjOrTex = !isCreateRoadObjOrTex;
                }

                y += BetweenY;

                if (isCreate3DGround)
                {
                    if (GUI.Button(new Rect(x, y, 100, 50), isHeightColor ? "高さ 色分け" : "一色"))
                    {
                        isHeightColor = !isHeightColor;
                    }

                    y += BetweenY;

                    if (GUI.Button(new Rect(x, y, 100, 50), !isToonLighting ? "影　ノーマル" : "影　トゥーン"))
                    {
                        isToonLighting = !isToonLighting;
                    }
                }

                x += BetweenX;
                y = ToggleTopButtonY;

                if (GUI.Button(new Rect(x, y, 100, 50), isCreateBuilding ?  "ビル作成" : "ビルなし"))
                {
                    isCreateBuilding = !isCreateBuilding;
                }

                y += BetweenY;

                if (isCreateBuilding)
                {
                    if (GUI.Button(new Rect(x, y, 100, 50), useConfigAsset ?  "asset から\nビル生成" : "asset なしで\nビル生成"))
                    {
                        useConfigAsset = !useConfigAsset;
                    }

                    y += BetweenY;

                    if (GUI.Button(new Rect(x, y, 100, 50), useLandmarkConfig ?  "landmark置換あり" : "landmark置換なし"))
                    {
                        useLandmarkConfig = !useLandmarkConfig;
                    }

                    y += BetweenY;

                    if (GUI.Button(new Rect(x, y, 100, 50), usePoiConfig ?  "POI置換あり" : "POI置換なし"))
                    {
                        usePoiConfig = !usePoiConfig;
                    }

                    y += BetweenY;
                }

                //
                if (GUI.Button(new Rect(BetweenX * 3 + 20, BetweenY * 6, 120, 50),  "地図作成"))
                {
                    if (isCreate3DGround)
                    {
                        // 3D地面の高さを強調すると地面の下にキャラクタが居ることになるので、高さだけ補正・・・
                        // FIXME: 雑参照
                        GameObject.Find("unitychan").transform.localPosition = new Vector3(0f, 20f * groundHeightScale, 0f);
                    }

                    y += BetweenY;
                    state = STATE_AROW_MAP.LOADING_AROW_MAP;
                }

                break;

            case STATE_AROW_MAP.PLAYING_GAME:
                if (GUI.Button(new Rect(50, 50, 140, 50), "「商業施設」登録\nカテゴリのみ"))
                {
                    GameObject ob = GameObject.Find("PoiObserver");
                    ArowPoiObserver poiObserver = ob.GetComponent<ArowPoiObserver>();
                    poiObserver.RegisterNoticeDistance(
                        ArowLibrary.ArowDefine.POIDefine.CategoryTbl[ArowLibrary.ArowDefine.POIDefine.CATEGORY.COMMERCIAL_FACILITY].name, 50);
                }

                if (GUI.Button(new Rect(50, 120, 140, 50), "「商業施設」\n「ファストフード」登録\nサブカテゴリまで"))
                {
                    GameObject ob = GameObject.Find("PoiObserver");
                    ArowPoiObserver poiObserver = ob.GetComponent<ArowPoiObserver>();
                    poiObserver.RegisterNoticeDistance(
                        ArowLibrary.ArowDefine.POIDefine.CategoryTbl[ArowLibrary.ArowDefine.POIDefine.CATEGORY.COMMERCIAL_FACILITY].name,
                        ArowLibrary.ArowDefine.POIDefine.SubcategoryTbl[ArowLibrary.ArowDefine.POIDefine.SUBCATEGORY.FAST_FOOD].name, 50);
                }

                if (GUI.Button(new Rect(50, 190, 140, 50),  "距離解除"))
                {
                    GameObject ob = GameObject.Find("PoiObserver");
                    ArowPoiObserver poiObserver = ob.GetComponent<ArowPoiObserver>();
                    poiObserver.RegisterNoticeDistance(ArowLibrary.ArowDefine.POIDefine.CategoryTbl[ArowLibrary.ArowDefine.POIDefine.CATEGORY.COMMERCIAL_FACILITY].name, 0);
                }

                break;
        }

        if (GUI.Button(new Rect(Screen.width - 100 - 50, 50, 100, 50),  "シーン再ロード"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_MoveControlCreatedMap", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        if (GUI.Button(new Rect(Screen.width - 100 - 50, Screen.height - 50 - 50, 100, 50), "スタートシーンへ"))
        {
            ArowSceneManager.ChangeScene(ArowSceneManager.StartSceneName);
        }
    }
}
