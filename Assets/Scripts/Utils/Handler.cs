using System;
using System.Collections;
using Managers;
using TMPro;
using UnityEngine;

namespace Utils
{
    public class Handler : MonoBehaviour
    {
        /// <summary>
        /// Serialized fields
        /// </summary>
        [SerializeField] private GameObject[] itemsInfoPanels;

        [SerializeField] private GameObject submitButton;
        [SerializeField] private GameObject enterNewNameText;
        [SerializeField] private GameObject enterNewPriceText;

        [SerializeField] private TMP_InputField[] nameInputFields;
        [SerializeField] private TMP_InputField[] priceInputFields;

        [SerializeField] private TextMeshProUGUI message;


        /// <summary>
        /// Private fields
        /// </summary>
        private int _panelEditIdx = -1;

        private string _originalName;
        private string _originalPrice;

        private TouchScreenKeyboard _keyboard;

        private TMP_InputField _activeInputField;


        // End Of Local Variables

        private void Update()
        {
            HandleTouchKeyboard();
        }

        public void OnEditButtonPressed(int index)
        {
            if (_panelEditIdx != -1)
                return;

            _panelEditIdx = index;
            itemsInfoPanels[index].GetComponent<Animator>().SetBool("Active", true);
            StartCoroutine(DelayExtraUIComponents());
        }

        private IEnumerator DelayExtraUIComponents()
        {
            yield return new WaitForSeconds(0.4f);
            submitButton.SetActive(true);
            nameInputFields[_panelEditIdx].gameObject.SetActive(true);
            priceInputFields[_panelEditIdx].gameObject.SetActive(true);
            enterNewNameText.SetActive(true);
            enterNewPriceText.SetActive(true);

            var product = ShopManager.Instance.GetProducts()[_panelEditIdx];
            _originalName = product.name;
            _originalPrice = product.price.ToString("F2");

            SetInputFieldText(_panelEditIdx, "name", _originalName);
            SetInputFieldText(_panelEditIdx, "price", _originalPrice);
        }

        private void HandleTouchKeyboard()
        {
            foreach (var inputField in nameInputFields)
            {
                if (inputField.isFocused)
                {
                    OpenTouchKeyboard(inputField);
                    return;
                }
            }

            foreach (var inputField in priceInputFields)
            {
                if (inputField.isFocused)
                {
                    OpenTouchKeyboard(inputField);
                    return;
                }
            }

            if (_keyboard != null && !_keyboard.active)
            {
                _keyboard = null;
            }
        }

        private void OpenTouchKeyboard(TMP_InputField inputField)
        {
            if (_keyboard == null || !_keyboard.active)
            {
                _keyboard = TouchScreenKeyboard.Open(inputField.text, TouchScreenKeyboardType.Default);
                _activeInputField = inputField;
            }

            if (_keyboard != null && _keyboard.active && _activeInputField != null)
            {
                _activeInputField.text = _keyboard.text;
            }
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
            else if (newName != _originalName && isValid)
            {
                ShopManager.Instance.UpdateProductName(_panelEditIdx, newName);
                ShowMessage("Product name updated successfully");
            }

            if (!float.TryParse(priceInputFields[_panelEditIdx].text, out float newPrice))
            {
                ShowMessage("Invalid price format. Please enter a valid number");
                isValid = false;
                priceInputFields[_panelEditIdx].text = _originalPrice;
            }
            else if (_originalPrice != newPrice.ToString("F2") && isValid)
            {
                newPrice = (float)Math.Round(newPrice, 2);
                ShopManager.Instance.UpdateProductPrice(_panelEditIdx, newPrice);
                ShowMessage("Product price updated successfully");
            }

            if (!isValid)
                return;

            DeactivateExtraUIComponents();
            itemsInfoPanels[_panelEditIdx].GetComponent<Animator>().SetBool("Active", false);
            _panelEditIdx = -1;
        }

        private void DeactivateExtraUIComponents()
        {
            submitButton.SetActive(false);
            nameInputFields[_panelEditIdx].gameObject.SetActive(false);
            priceInputFields[_panelEditIdx].gameObject.SetActive(false);
            enterNewNameText.SetActive(false);
            enterNewPriceText.SetActive(false);
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

        private void ShowMessage(string messageToShow)
        {
            if (message != null)
            {
                message.text = messageToShow;
                Color color = message.color;
                color.a = 1f;
                message.color = color;

                message.gameObject.SetActive(true);
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