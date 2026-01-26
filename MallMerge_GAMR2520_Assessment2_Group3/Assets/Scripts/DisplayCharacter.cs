using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCharacter : MonoBehaviour
{
    //Image on the screen that be accessed in unity inspector
    [SerializeField] private Image Character_Image;
   private void Awake()
    {
        //Character image is hiden at the start of the game
        Character_Image.gameObject.SetActive(false);
    }

    //Show one of the character sprite images
    public void DisplayImage(Sprite sprite)
    {
        //Change the image to the character sprite
        Character_Image.sprite= sprite;

        //Display the character sprite
        Character_Image.gameObject.SetActive(true);
    }

    //Hide the character sprite
    public void Hide()
    {
        Character_Image.gameObject.SetActive(true);

    }
}
