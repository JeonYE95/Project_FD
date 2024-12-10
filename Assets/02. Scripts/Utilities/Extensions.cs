using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extensions : MonoBehaviour
{
    // UI 요소의 월드 중심점 구하기
    public static Vector3 GetUIWorldPosition(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        Vector3 centerPosition = (corners[0] + corners[2]) / 2f;
        centerPosition.z = 0;
        return centerPosition;
    }

    // 마우스 위치를 월드 좌표로 변환
    public static Vector3 GetMouseWorldPosition(Canvas canvas, Vector3 screenPosition)
    {
        Vector3 worldPos = canvas.worldCamera.ScreenToWorldPoint(screenPosition);
        worldPos.z = 0;
        return worldPos;
    }
}
