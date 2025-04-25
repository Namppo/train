using UnityEngine;
using UnityEngine.UI;

public class BOUPartController : MonoBehaviour
{
    public Image partImage;
    public Sprite openSprite;
    public Sprite closeSprite;

    public void ShowPart()
    {
        if (partImage.sprite == openSprite)
        {
            partImage.sprite = closeSprite;
        }
        else
        {
            partImage.sprite = openSprite;
        }
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
