using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class ShopManager
    {
        private static ShopManager instance;
        private List<ProductFetcher.Product> _products = new List<ProductFetcher.Product>();
        
        public event Action OnProductsUpdated;
        
        public static ShopManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ShopManager();
                }
                return instance;
            }
        }
        

        public void ClearProducts()
        {
            _products.Clear();
        }
        
        public void AddProduct(ProductFetcher.Product product)
        {
            _products.Add(product);
        }

        public ProductFetcher.Product[] GetProducts()
        {
            return _products.ToArray();
        }

        public void UpdateProductPrice(int productId, float newPrice)
        {
            if (productId >= 0 && productId < _products.Count)
            {
                _products[productId].price = newPrice;
                OnProductsUpdated?.Invoke();
            }
            else
            {
                Debug.LogError("Invalid product ID");
            }
        }

        public void UpdateProductName(int index, string text)
        {
            if (index >= 0 && index < _products.Count)
            {
                _products[index].name = text;
                OnProductsUpdated?.Invoke();
            }
            else
            {
                Debug.LogError("Invalid product ID");
            }
        }

    }
}
