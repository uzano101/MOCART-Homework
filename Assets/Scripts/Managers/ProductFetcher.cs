using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


namespace Managers
{
    public class ProductFetcher : MonoBehaviour
    {
        private readonly string _serverUrl = "https://homework.mocart.io/api/products";

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

        public void Start()
        {
            StartCoroutine(FetchAndDisplayProducts());
        }

        private IEnumerator FetchAndDisplayProducts()
        {
            UnityWebRequest request = UnityWebRequest.Get(_serverUrl);

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

    }
}