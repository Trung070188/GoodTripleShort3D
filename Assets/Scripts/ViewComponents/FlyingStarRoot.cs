using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class FlyingStarRoot : MonoBehaviour
{
    public GameObject starPrefab;

    public RectTransform targetRoot;

    public Canvas uiCanvas;

    private RectTransform starObj1, starObj2, starObj3, starObj4, starObj5, starObj6;

    // Start is called before the first frame update
    void Start()
    {

        GameObject obj1 = Instantiate(starPrefab) as GameObject;
        obj1.SetActive(true);
        starObj1 = obj1.GetComponent<RectTransform>();
        starObj1.SetParent(transform);
        starObj1.localScale = Vector3.one;


        GameObject obj2 = Instantiate(starPrefab) as GameObject;
        obj2.SetActive(true);
        starObj2 = obj2.GetComponent<RectTransform>();
        starObj2.SetParent(transform);
        starObj2.localScale = Vector3.one;

        GameObject obj3 = Instantiate(starPrefab) as GameObject;
        obj3.SetActive(true);
        starObj3 = obj3.GetComponent<RectTransform>();
        starObj3.SetParent(transform);
        starObj3.localScale = Vector3.one;


        starObj1.gameObject.SetActive(false);
        starObj2.gameObject.SetActive(false);
        starObj3.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }


    List<Vector3> arcPoint = new List<Vector3>();

    Vector3 midlePoint;

    public void SpawnStar(Vector3 spawnPos, int combo)
    {
        if(!starObj3.gameObject.activeInHierarchy)
        {
            starObj1.gameObject.SetActive(true);
            starObj2.gameObject.SetActive(true);
            starObj3.gameObject.SetActive(true);
            spawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z) - Camera.main.transform.forward;
            starObj1.position = WorldToCanvasPosition(uiCanvas, starObj1, Camera.main, spawnPos);
            starObj2.position = WorldToCanvasPosition(uiCanvas, starObj2, Camera.main, spawnPos);
            starObj3.position = WorldToCanvasPosition(uiCanvas, starObj3, Camera.main, spawnPos);

            arcPoint.Clear();
            midlePoint = BetweenP(starObj1.localPosition, targetRoot.localPosition, 0.5f);

            for (int i = 0; i < 10; i++)
            {
                arcPoint.Add(SampleParabola(starObj1.localPosition, midlePoint, -100.0f, (float)i / 9.0f));
            }

            for (int i = 0; i < 10; i++)
            {
                arcPoint.Add(SampleParabola(midlePoint, targetRoot.localPosition, 100.0f, (float)i / 9.0f));
            }

            starObj1.DOLocalPath(arcPoint.ToArray(), 1.0f, PathType.Linear).SetLoops(1).SetEase(Ease.Linear).OnComplete(() =>
            {
                starObj1.gameObject.SetActive(false);
            });

            starObj2.DOLocalPath(arcPoint.ToArray(), 1.0f, PathType.Linear).SetLoops(1).SetDelay(0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                starObj2.gameObject.SetActive(false);
            });

            starObj3.DOLocalPath(arcPoint.ToArray(), 1.0f, PathType.Linear).SetLoops(1).SetDelay(0.2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                AudioManager.instance.fireworkSound.Play();
                starObj3.gameObject.SetActive(false);
                GameManager.Instance.uiManager.gameView.GetStarCombo(combo);
            });
        }
        else
        {
            GameManager.Instance.uiManager.gameView.GetStarCombo(combo);
        }
    }

    private Vector2 WorldToCanvasPosition(Canvas canvas, RectTransform canvasRect, Camera camera, Vector3 position)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, position);
        // Vector2 result;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : camera, out result);
        //return canvas.transform.TransformPoint(result);
        return screenPoint;
    }

    Vector3 BetweenP(Vector3 start, Vector3 end, float percent)
    {
        return (start + percent * (end - start));
    }

    Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
    {
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result.y += Mathf.Sin(t * Mathf.PI) * height;
            return result;
        }
        else
        {
            //start and end are not level, gets more complicated
            Vector3 travelDirection = end - start;
            Vector3 levelDirecteion = end - new Vector3(start.x, end.y, start.z);
            Vector3 right = Vector3.Cross(travelDirection, levelDirecteion);
            Vector3 up = Vector3.Cross(right, travelDirection);
            if (end.y > start.y) up = -up;
            Vector3 result = start + t * travelDirection;
            result += (Mathf.Sin(t * Mathf.PI) * height) * up.normalized;
            return result;
        }
    }


}
