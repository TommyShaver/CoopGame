using UnityEngine;
using UnityEngine.UI;
public class PlayerHelperItem : MonoBehaviour
{
    public PlayerManager playerManager;
    public Sprite[] helperItemsSprites;
    public Image helpItemImage;

    private int currentHelperItem;

   

    public void SelectHelperItem()
    {
        ItemSelected();
        if (currentHelperItem == 2)
        {
            currentHelperItem = 0;
            
            return;
        }
        currentHelperItem++;
    }

    private void ItemSelected()
    {
        playerManager.WhichHelperItemSelected(currentHelperItem);
        helpItemImage.sprite = helperItemsSprites[currentHelperItem];
    }
}
