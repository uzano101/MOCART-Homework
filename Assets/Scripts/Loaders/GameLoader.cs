using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Managers;
using TMPro;
using UnityEngine.Networking;

namespace Loaders
{
    public class GameLoader : MonoBehaviour
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

        private float _timer;
        
        [SerializeField] private TextMeshProUGUI loadingText;

        private void Start()
        {
            StartCoroutine(LoadGameSequence());
        }
        
        private void Update()
        {
            _timer += Time.deltaTime * 2;
            loadingText.text = "Loading" + new string('.', (int)(_timer % 3) + 1);
        }

        private IEnumerator LoadGameSequence()
        {
            yield return FetchAndDisplayProducts();

            DontDestroyOnLoad(gameObject);

            LoadManagers();

            yield return LoadMainScene();
        }

        private IEnumerator FetchAndDisplayProducts()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(_serverUrl))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error fetching data: " + request.error);
                    yield break;
                }

                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Received JSON: " + jsonResponse);

                ProductData productData = JsonUtility.FromJson<ProductData>(jsonResponse);

                if (productData.products != null && productData.products.Length > 0)
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
                var newProduct = new ProductFetcher.Product
                {
                    name = product.name,
                    description = product.description,
                    price = product.price
                };
                ShopManager.Instance.AddProduct(newProduct);
            }
        }

        private void LoadManagers()
        {
            Debug.Log("Initializing Game Managers...");
            var shopManager = ShopManager.Instance;
        }

        private IEnumerator LoadMainScene()
        {
            Debug.Log("Loading Main Scene...");
            SceneManager.sceneLoaded += OnMainSceneLoaded;
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("Main");

            yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Main");
        }

        private void OnMainSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnMainSceneLoaded;
            Debug.Log("Main Scene Loaded, destroying GameLoader...");
            Destroy(gameObject);
        }
    }
}
