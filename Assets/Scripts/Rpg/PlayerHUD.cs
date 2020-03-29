using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace rpg
{
    public class PlayerHUD : MonoBehaviour
    {
        private Sprite heartFull, heartEmpty;
        private GameObject heartTemplate;
        private Transform heartsContainer;
        private List<Image> heartsList;

        private void Awake()
        {
            heartsContainer = transform.Find("Hearts").transform;
            heartTemplate = heartsContainer.Find("Full").gameObject;

            Transform heartFullTransform = heartsContainer.Find("Full");
            heartFull = heartFullTransform.GetComponent<Image>().sprite;
            Transform heartEmptyTransform = heartsContainer.Find("Empty");
            heartEmpty = heartEmptyTransform.GetComponent<Image>().sprite;

            heartFullTransform.gameObject.SetActive(false);
            heartEmptyTransform.gameObject.SetActive(false);

            heartsList = new List<Image>();
        }

        public void UpdateHearts(int currentLife, int maxLife)
        {
            while (maxLife > heartsList.Count)
            {
                GameObject newHeart = Instantiate(heartTemplate, heartsContainer);
                newHeart.SetActive(true);
                heartsList.Add(newHeart.GetComponent<Image>());
            }
            while (maxLife < heartsList.Count)
            {
                Image lastHeart = heartsList[heartsList.Count - 1];
                heartsList.Remove(lastHeart);
                Destroy(lastHeart.gameObject);
            }

            int i = 0;
            foreach (var heart in heartsList)
            {
                heart.sprite = i < currentLife ? heartFull : heartEmpty;
                i++;
            }
        }
    }
}