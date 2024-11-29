using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScrollBGTest
{
    [System.Serializable]
    /// <summary>
    //This script is used for background scrolling demo play.
    //Used in the editor and android.
    //This is a sample script and stability and optimization are not guaranteed.
    /// </summary>

    public class ScrollBackgroundCtrl : MonoBehaviour
    {
        //Background Layers
        public Transform[] Background;

        //Scrolling Speeds
        public float[] ScrollSpeed;

        //Renderer
        public MeshRenderer[] Ren;
        public MeshRenderer SkyRen;

        //Movement speed according to keyboard input
        public float MoveValue;
        public float MoveSpeed;

        //Scroll of the sky
        float SkyMoveValue;
        public float SkyScrollSpeed;

        public float[] backgroundOffsets;

        void Start()
        {
            //Reset Values
            MoveValue = 0;
            SkyMoveValue = 0;

            //Get MeshRenderers
            for (int i = 0; i < Background.Length; i++)
            {
                Ren[i] = Background[i].GetComponent<MeshRenderer>();
                backgroundOffsets[i] = 0f;
            }


        }


        void Update()
        {
            //Input
            //if (Input.GetKey(KeyCode.LeftArrow))
            //    MoveValue -= MoveSpeed;

            //if (Input.GetKey(KeyCode.RightArrow))
            //    MoveValue += MoveSpeed;

            //Material OffSet
            for (int i = 0; i < Background.Length; i++)
            {
                backgroundOffsets[i] += Time.unscaledDeltaTime * ScrollSpeed[i];
                Ren[i].material.mainTextureOffset = new Vector2(backgroundOffsets[i], 0);
            }

            SkyMoveValue += Time.unscaledDeltaTime * -SkyScrollSpeed;
            SkyRen.material.mainTextureOffset = new Vector2(SkyMoveValue, 0);
        }
    }

}
