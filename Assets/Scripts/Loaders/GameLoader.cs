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
        /// <summary>
        /// Private fields
        /// </summary>
        private float _timer;
        
        
        /// <summary>
        /// Serialized fields
        /// </summary>
        [SerializeField] private TextMeshProUGUI loadingText;
        
        
        // End Of Local Variables

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
            using (UnityWebRequest request = UnityWebRequest.Get(ProductFetcher.GetServerUrl()))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error fetching data: " + request.error);
                    yield break;
                }

                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Received JSON: " + jsonResponse);

                ProductFetcher.ProductData productData = JsonUtility.FromJson<ProductFetcher.ProductData>(jsonResponse);

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

        private void DisplayProducts(ProductFetcher.Product[] products)
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
