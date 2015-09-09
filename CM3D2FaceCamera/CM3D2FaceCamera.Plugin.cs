using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;

namespace CM3D2FaceCamera
{
    [PluginFilter("CM3D2x64"),
    PluginFilter("CM3D2x86"),
    PluginName("Face Camera"),
    PluginVersion("1.0.0.2")]

    public class FaceCamera : PluginBase
    {

        private GameObject maid;
        private Transform maidHead;
        private Transform maidHair;
        private GameObject faceCameraObj;
        private Camera faceCamera;
        private bool allowUpdate = false;
        private int frameCount = 0;
        private String oldParamVisible;
        private bool occulusVR = false;
        private bool onScreen = true;
        private bool executing = false;

        /// <summary>
        /// trueにするとテンキーで画面の移動が出来るようになります。
        /// 2↓・4←・6→・8↑
        /// </summary>
        private bool movableMode = false;

        public void Awake()
        {
            string path = Application.dataPath;
            //Console.WriteLine(Application.dataPath);
            occulusVR = path.Contains("CM3D2VRx64");
            if (occulusVR)
            {
                Console.WriteLine("FaceCamera: Occuls Rift is not Support!");
            }
            else
            {
                GameObject.DontDestroyOnLoad(this);
            }
        }

        public void OnLevelWasLoaded(int level)
        {
            if (!occulusVR)
            {
                if (level == 14)
                {
                    oldParamVisible = GameMain.Instance.CMSystem.GetSystemVers("夜伽_HideScroll_HorizontalRigt");
                    faceCameraObj = new GameObject("faceCamera");
                    faceCamera = faceCameraObj.AddComponent<Camera>();
                    faceCamera.CopyFrom(Camera.main);

                    SetRect(oldParamVisible);

                    faceCameraObj.SetActive(true);

                    SearchMaid();

                    allowUpdate = true;
                }
                else
                {
                    allowUpdate = false;
                }
            }
        }

        private void SearchMaid()
        {
            GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();

            if (array.Length > 0)
            {

                foreach (var gameobject in array)
                {
                    if (gameobject.name.IndexOf("Maid[0]") > -1)
                    {
                        maid = gameobject;
                    }
                }
                if (maid)
                {
                    Transform[] objList = maid.transform.GetComponentsInChildren<Transform>();
                    if (objList.Count() == 0)
                    {
                        allowUpdate = false;
                    }
                    else
                    {
                        foreach (var gameobject in objList)
                        {
                            if (gameobject.name == "Bone_Face")
                            {
                                maidHead = gameobject;
                            }
                            else if (gameobject.name == "Hair_F")
                            {
                                maidHair = gameobject;
                            }
                        }
                    }
                }
                else
                {
                    allowUpdate = false;
                }
            }
            else
            {
                allowUpdate = false;
            }
        }

        private void SetRect(string s)
        {
            if (s == "1")
            {
                faceCamera.rect = new Rect(0.679f, 0.7f, 0.15f, 0.3f);
            }
            else
            {
                faceCamera.rect = new Rect(0.814f, 0.7f, 0.15f, 0.3f);
            }

        }

        public void Update()
        {
            if (!occulusVR)
            {

                if (movableMode)
                {
                    if (allowUpdate)
                    {
                        if (Input.GetKey(KeyCode.Keypad4))
                        {
                            faceCamera.rect = new Rect(faceCamera.rect.xMin - 0.001f, faceCamera.rect.yMin, faceCamera.rect.width, faceCamera.rect.height);
                        }
                        else if (Input.GetKey(KeyCode.Keypad6))
                        {
                            faceCamera.rect = new Rect(faceCamera.rect.xMin + 0.001f, faceCamera.rect.yMin, faceCamera.rect.width, faceCamera.rect.height);
                        }
                        if (Input.GetKey(KeyCode.Keypad2))
                        {
                            faceCamera.rect = new Rect(faceCamera.rect.xMin, faceCamera.rect.yMin - 0.001f, faceCamera.rect.width, faceCamera.rect.height);
                        }
                        if (Input.GetKey(KeyCode.Keypad8))
                        {
                            faceCamera.rect = new Rect(faceCamera.rect.xMin, faceCamera.rect.yMin + 0.001f, faceCamera.rect.width, faceCamera.rect.height);
                        }
                    }
                }

                if (Input.GetKey(KeyCode.M))
                {
                    if (!executing)
                    {
                        if (onScreen)
                        {
                            faceCamera.gameObject.SetActive(false);
                            onScreen = !onScreen;
                        }
                        else
                        {
                            faceCamera.gameObject.SetActive(true);
                            onScreen = !onScreen;
                        }
                        executing = true;
                    }
                }
                else
                {
                    executing = false;
                }
            }
        }

        public void LateUpdate()
        {
            if (!occulusVR)
            {
                if (allowUpdate)
                {
                    if (maid != null)
                    {
                        Vector3 targetCamPos = maidHead.transform.position + maidHead.transform.right * -0.5f;
                        faceCameraObj.transform.position = Vector3.Lerp(faceCameraObj.transform.position, targetCamPos, Time.deltaTime * 5f);
                        Vector3 targetFacePos = Vector3.Lerp(maidHair.position, maidHead.position, 0.6f);
                        faceCameraObj.transform.LookAt(targetFacePos, maidHead.transform.up);
                    }
                    else
                    {
                        SearchMaid();
                    }

                    string paramVisible = GameMain.Instance.CMSystem.GetSystemVers("夜伽_HideScroll_HorizontalRigt");

                    if (paramVisible != oldParamVisible)
                    {
                        SetRect(paramVisible);
                        oldParamVisible = paramVisible;
                    }

                    if (movableMode)
                    {
                        if (frameCount == 60)
                        {
                            Console.WriteLine("left:{0}, top:{1}, width:{2}, height{3}", faceCamera.rect.xMin, faceCamera.rect.yMin, faceCamera.rect.width, faceCamera.rect.height);

                            frameCount = 0;
                        }
                        else
                        {
                            ++frameCount;
                        }
                    }
                }
            }
        }
    }
}
