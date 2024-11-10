using UnityEngine;
using TMPro;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// Serialized fields
        /// </summary>
        [SerializeField] private GameObject[] itemsInfoPanels;

        [SerializeField] private TextMeshProUGUI[] namesText;
        [SerializeField] private TextMeshProUGUI[] descriptionText;
        [SerializeField] private TextMeshProUGUI[] priceText;

        [SerializeField] private TextMeshProUGUI totalPriceText;


        // End Of Local Variables
        
        private void Start()
        {
            ShopManager.Instance.OnProductsUpdated += UpdateProducts;
            DisplayProducts();
        }

        private void UpdateProducts()
        {
            DisplayProducts();
        }

        private void DisplayProducts()
        {
            ProductFetcher.Product[] products = ShopManager.Instance.GetProducts();
            float totalPrice = 0;
            for (int i = 0; i < products.Length; i++)
            {
                itemsInfoPanels[i].SetActive(true);
                namesText[i].text = products[i].name;
                descriptionText[i].text = products[i].description;
                priceText[i].text = products[i].price.ToString();
                totalPrice += products[i].price;
            }

            totalPriceText.text = totalPrice.ToString();
        }
    }
}