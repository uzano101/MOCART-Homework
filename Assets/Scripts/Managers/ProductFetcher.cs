using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


namespace Managers
{
    public class ProductFetcher : MonoBehaviour
    {
        /// <summary>
        /// Product fetcher server URL
        /// </summary>
        private const string ServerUrl = "https://homework.mocart.io/api/products";


        /// <summary>
        /// Serializable classes
        /// </summary>
        [Serializable]
        public class Product
        {
            public string name;
            public string description;
            public float price;
        }

        [Serializable]
        public class ProductData
        {
            public Product[] products;
        }

        
        // End Of Local Variables
        
        public void Start()
        {
            StartCoroutine(FetchAndDisplayProducts());
        }

        private IEnumerator FetchAndDisplayProducts()
        {
            UnityWebRequest request = UnityWebRequest.Get(ServerUrl);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching data: " + request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Received JSON: " + jsonResponse);
                ProductData productData = JsonUtility.FromJson<ProductData>(jsonResponse);

                if (productData.products is { Length: > 0 })
                {
                    DisplayProducts(productData.products);
                }
                else
                {
                    Debug.LogWarning("No products found in JSON response.");
                }
            }
        }

        private void DisplayProducts(Product[] products)
        {
            ShopManager.Instance.ClearProducts();
            foreach (var product in products)
            {
                ShopManager.Instance.AddProduct(product);
            }
        }

        public static string GetServerUrl()
        {
            return ServerUrl;
        }
    }
}