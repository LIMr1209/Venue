using System;
using UnityEngine;
using UnityEngine.Video;

namespace DefaultNamespace
{
    public class ArtInit : MonoBehaviour
    {
        
        private ArtUpdateTrans _artUpdateTrans;
        public void Start()
        {
            _artUpdateTrans = FindObjectOfType<ArtUpdateTrans>();
        }

        public void UpdateArt(JsonData.ArtData artData, bool init = false)
        {
            GameObject baseArt = null;
            GameObject art = GameObject.Find(artData.name);
            if (art && artData.isDel) // 删除了的 模型里面的画框
            {
                Debug.Log("删除画框");
                DeleteArt(art.name);
                return;
            }

            if (art == null && !artData.isDel) // 复制的 且没有被标记删除的画框 需要复制 （找到基体）
            {
                Debug.Log("复制画框");
                baseArt = GameObject.Find(artData.cloneBase);
                if (baseArt == null)
                {
                    return;
                }

                art = CopyArt(baseArt);
                art.name = artData.name;
            }

            if (!art) return;
            CustomAttr customAttr = CustomAttr.GetCustomAttr(art);
            customAttr.SetArtData(artData);
            customAttr.cloneBase = baseArt != null ? baseArt.name : artData.cloneBase;
            SetTransForm(art, customAttr);
            if (!string.IsNullOrEmpty(artData.imageUrl))
            {
                if (!init && artData.imageUrl == customAttr.imageUrl) return;
                customAttr.imageUrl = artData.imageUrl;
                GameObject paining = art.transform.GetChild(1).gameObject;
                AbInit.instances.ReplaceMaterialContent(paining, artData.imageUrl, artData.nKind, init);
            }
        }

        public GameObject CopyArt(GameObject art, bool select = false)
        {
            GameObject clone = Instantiate(art, art.transform.parent);
            Debug.Log("复制克隆");
            CustomAttr customAttr = CustomAttr.GetCustomAttr(clone);
            if (String.IsNullOrEmpty(customAttr.cloneBase))
            {
                Debug.Log("使用父类");
                clone.name = art.name + "-" + Guid.NewGuid();
            }
            else
            {
                Debug.Log("使用祖先");
                clone.name = customAttr.cloneBase + "-" + Guid.NewGuid();
            }

            Debug.Log("克隆重命名");
            GameObject paining = clone.transform.GetChild(1).gameObject;
            MeshRenderer painingRender = paining.GetComponent<MeshRenderer>();
            Material material = Instantiate(painingRender.material);
            painingRender.material = material;
            Debug.Log("获取videoplayer组件");
            if (paining.TryGetComponent<VideoPlayer>(out VideoPlayer videoPlayer))
            {
                videoPlayer.Play();
            }

            // 编辑模式 前端调用复制 需要 选中并且传数据给前端
            if (select && _artUpdateTrans.enabled)
            {
                Debug.Log("复制画框 设置自定义属性");
                Debug.Log("cloneBase " + customAttr.cloneBase);
                if (String.IsNullOrEmpty(customAttr.cloneBase))
                {
                    Debug.Log("设置 cloneBase ");
                    customAttr.cloneBase = art.name;
                }

                _artUpdateTrans.ClearAndAddTarget(clone.transform);
            }

            return clone;
        }

        // 复制art
        public GameObject CopyArt(string artName, bool select = false)
        {
            GameObject art = GameObject.Find(artName);
            if (art == null)
            {
                throw (new Exception("画框不存在"));
            }

            return CopyArt(art, select);
        }

        // 删除art
        public void DeleteArt(string artName)
        {
            GameObject art = GameObject.Find(artName);
            if (art)
            {
                DeleteArt(art);
            }
        }

        // 删除art
        public void DeleteArt(GameObject art)
        {
            GameObject paining = art.transform.GetChild(1).gameObject;
            Material material = paining.GetComponent<MeshRenderer>().material;
            Destroy(art);
            Destroy(material);
        }

        // 锁定解锁 art
        public void LockArt(string artName, bool locking)
        {
            GameObject art = GameObject.Find(artName);
            if (art == null)
            {
                throw (new Exception("画框不存在"));
            }

            GameObject paining = art.transform.GetChild(1).gameObject;
            if (locking) paining.layer = LayerHelp.lockArtLayerNum;
            else paining.layer = LayerHelp.focusArtLayerNum;
        }

        // 初始化 加载 art
        public void InitArtImage(JsonData.ArtData[] artDataList)
        {
            foreach (JsonData.ArtData i in artDataList)
            {
                UpdateArt(i, true);
            }
        }

        // 初始加载 删除 被标记删除的art
        public void InitDeleteArt(JsonData.ArtData[] artDataList)
        {
            foreach (JsonData.ArtData i in artDataList)
            {
                DeleteArt(i.name);
            }
        }

        public static void SetTransForm(GameObject art, CustomAttr customAttr)
        {
            art.transform.localPosition = customAttr.oldLocation +
                                          new Vector3(customAttr.location[0], customAttr.location[1],
                                              customAttr.location[2]);
            Vector3 oldScale = customAttr.oldScale;
            art.transform.localScale = new Vector3(customAttr.scaleS * oldScale.x, customAttr.scaleS * oldScale.y,
                customAttr.scaleS * oldScale.z);
            art.transform.localRotation = customAttr.oldRotate * Quaternion.Euler(0, 0, customAttr.rotateS);
            // art.transform.localPosition = new Vector3(i.location[0], i.location[1], i.location[2]);
            // art.transform.localScale = new Vector3(i.scaleS,i.scaleS, i.scaleS);
            // art.transform.localScale = new Vector3(i.scale[0], i.scale[1], i.scale[2]);
            // Quaternion rotate = new Quaternion();
            // rotate.eulerAngles = new Vector3(i.scale[0], i.scale[1], i.scale[2]);
            // Quaternion oldRotate = art.transform.localRotation;
            // rotate.eulerAngles = new Vector3(oldRotate.eulerAngles.x, i.rotateS, oldRotate.eulerAngles.z);
            // art.transform.localRotation = rotate;
        }
    }
}