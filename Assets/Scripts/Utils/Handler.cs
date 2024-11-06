using System.Collections;
using Managers;
using TMPro;
using UnityEngine;

namespace Utils
{
    public class Handler : MonoBehaviour
    {
        [SerializeField] private GameObject[] itemsInfoPanels;
        [SerializeField] private GameObject submitButton;

        [SerializeField] private TMP_InputField[] nameInputFields;
        [SerializeField] private TMP_InputField[] priceInputFields;
        
        [SerializeField] private TextMeshProUGUI message;

        private int _panelEditIdx = -1;
        private string _originalName;
        private string _originalPrice;

        public void OnEditButtonPressed(int index)
        {
            if (_panelEditIdx != -1)
            {
                return;
            }

            _panelEditIdx = index;
            itemsInfoPanels[index].GetComponent<Animator>().SetBool("Active", true);
            StartCoroutine(DelaySubmitButton());
        }

        private IEnumerator DelaySubmitButton()
        {
            yield return new WaitForSeconds(0.35f);
            submitButton.SetActive(true);
            nameInputFields[_panelEditIdx].gameObject.SetActive(true);
            priceInputFields[_panelEditIdx].gameObject.SetActive(true);
            
            var product = ShopManager.Instance.GetProducts()[_panelEditIdx];
            _originalName = product.name;
            _originalPrice = product.price.ToString("F2");

            SetInputFieldText(_panelEditIdx, "name", _originalName);
            SetInputFieldText(_panelEditIdx, "price", _originalPrice);
        }

        public void OnSubmitButtonPressed()
        {
            bool isValid = true;

            string newName = nameInputFields[_panelEditIdx].text;
            if (string.IsNullOrEmpty(newName))
            {
                ShowMessage("Name cannot be empty");
                isValid = false;
                nameInputFields[_panelEditIdx].text = _originalName;
            }
            else
            {
                ShopManager.Instance.UpdateProductName(_panelEditIdx, newName);
            }

            if (!float.TryParse(priceInputFields[_panelEditIdx].text, out float newPrice))
            {
                ShowMessage("Invalid price format. Please enter a valid number");
                isValid = false;
                priceInputFields[_panelEditIdx].text = _originalPrice;
            }
            else if (isValid)
            {
                ShopManager.Instance.UpdateProductPrice(_panelEditIdx, newPrice);
            }

            if (!isValid)
            {
                return;
            }

            submitButton.SetActive(false);
            nameInputFields[_panelEditIdx].gameObject.SetActive(false);
            priceInputFields[_panelEditIdx].gameObject.SetActive(false);
            itemsInfoPanels[_panelEditIdx].GetComponent<Animator>().SetBool("Active", false);
            _panelEditIdx = -1;
            ShowMessage("Product updated successfully");
        }

        private void SetInputFieldText(int index, string fieldType, string placeholderText)
        {
            TMP_InputField targetInputField = fieldType == "name" ? nameInputFields[index] : priceInputFields[index];

            if (targetInputField != null)
            {
                targetInputField.text = placeholderText;
            }
            else
            {
                Debug.LogWarning($"Invalid field type '{fieldType}' provided.");
            }
        }

        private void ShowMessage(string message)
        {
            if (this.message != null)
            {
                this.message.text = message;
                Color color = this.message.color;
                color.a = 1f;
                this.message.color = color;
                
                this.message.gameObject.SetActive(true);
                StartCoroutine(FadeOutMessage(3f));
            }
        }

        private IEnumerator FadeOutMessage(float delay)
        {
            yield return new WaitForSeconds(delay);

            float fadeDuration = 1f;
            float elapsedTime = 0f;
            Color color = message.color;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                message.color = color;
                yield return null;
            }

            message.gameObject.SetActive(false);
        }
    }
}
