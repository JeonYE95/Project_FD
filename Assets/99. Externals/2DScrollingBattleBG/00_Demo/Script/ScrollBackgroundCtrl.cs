using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
            // Reset Values
            MoveValue = 0;
            SkyMoveValue = 0;

            // Initialize arrays
            Ren = new MeshRenderer[Background.Length];
            backgroundOffsets = new float[Background.Length];

            // Get MeshRenderers
            for (int i = 0; i < Background.Length; i++)
            {
                Ren[i] = Background[i].GetComponent<MeshRenderer>();
                backgroundOffsets[i] = 0f;
            }
        }



        void Update()
        {
            // Input
            if (Input.GetKey(KeyCode.LeftArrow))
                MoveValue -= MoveSpeed * Time.unscaledDeltaTime;

            else if (Input.GetKey(KeyCode.RightArrow))
                MoveValue += MoveSpeed * Time.unscaledDeltaTime;
            
            else
                MoveValue = 0f;

            // Material Offset
            for (int i = 0; i < Background.Length; i++)
            {
                backgroundOffsets[i] += Time.unscaledDeltaTime * ScrollSpeed[i];
                float offsetX = MoveValue * ScrollSpeed[i]; // MoveValue를 X축 오프셋에 적용
                Ren[i].material.mainTextureOffset = new Vector2(offsetX + backgroundOffsets[i], 0);
            }

            // Sky Background
            SkyMoveValue += Time.unscaledDeltaTime * -SkyScrollSpeed;
            SkyRen.material.mainTextureOffset = new Vector2(SkyMoveValue, 0);
        }

    }

}
