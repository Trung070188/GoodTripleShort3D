using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClearVfx : MonoBehaviour
{
    private SpriteRenderer vfxSprite;

    public Sprite[] spriteArr;

    // Start is called before the first frame update
    void Start()
    {
        vfxSprite = GetComponent<SpriteRenderer>();
        ResetPara();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
          //  PlayAnim();
    }

    public void PlayAnim()
    {
        transform.rotation = Quaternion.Euler(0, 0, 5);
        vfxSprite.color = new Color(1, 1, 1, 1);

        transform.DOScale(new Vector3(-1.5f, 1.5f, 1.5f), 0.25f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DORotate(new Vector3(0, 0, -20.0f), 0.35f).SetDelay(0.25f).SetEase(Ease.Linear).OnComplete(() =>
            {


            });

            transform.DOScale(new Vector3(-2.5f, 2.5f, 2.5f), 0.35f).SetDelay(0.25f).SetEase(Ease.OutQuart);

            vfxSprite.DOFade(0.0f, 0.35f).SetDelay(0.25f).SetEase(Ease.Linear).OnComplete(() =>
            {

                ResetPara();

            });

        });

       
    }

    private void ResetPara()
    {
        transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        vfxSprite.color = new Color(1, 1, 1, 0);
        int randomIndex = Random.Range(0, spriteArr.Length);
        vfxSprite.sprite = spriteArr[randomIndex];
    }
}
