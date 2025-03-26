using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon.Map
{
    public class MapLevelBehavior : MapLevelAbstractBehavior
    {
        [SerializeField] Image innerCircle;

        [Space]
        [SerializeField] Color reachedText;
        [SerializeField] Color reachedCircle;
        [Space]
        [SerializeField] Color openedText;
        [SerializeField] Color openedCircle;
        [Space]
        [SerializeField] Color closedText;
        [SerializeField] Color closedCircle;

        public Sprite openedSprite;

        public Sprite reachedSprite; 
        public Sprite closedSprite; 
        public Image numberBG;


        protected override void InitOpen()
        {
            // levelNumber.color = openedText;
            // innerCircle.color = openedCircle;
            numberBG.sprite = openedSprite;    
            button.gameObject.SetActive(true);
        }

        protected override void InitClose() 
        {
            // levelNumber.color = closedText;
            // innerCircle.color = closedCircle;
            numberBG.sprite = closedSprite;   
            button.gameObject.SetActive(false);
        }

        protected override void InitCurrent()
        {
            // levelNumber.color = reachedText;
            // innerCircle.color = reachedCircle;
            numberBG.sprite = reachedSprite;   
            button.gameObject.SetActive(true);
        }
    }
}