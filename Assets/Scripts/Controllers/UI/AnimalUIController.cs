using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AnimalUIController : MonoBehaviour
{
    public Text NameText;

    public Text StateText;

    public Text AdaptiveText;

    public Text AgeText;

    public Text AgeStageText;

    public Text GenderText;

    public Text HPText;

    public Slider HungerSlider;

    public Slider ThirstSlider;

    public Slider StaminaSlider;

    private MouseController mouseController;

    public Animal currentlySelected { get; protected set; }
    
    // Start is called before the first frame update
    void Start()
    {
        mouseController = FindObjectOfType<MouseController>();
        currentlySelected = null;
        if (mouseController == null || NameText == null || StateText == null)
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                currentlySelected = mouseController.GetMouseoverAnimal();
            }
        }
        
        if (currentlySelected != null)
        {
            NameText.text = "Name: " + currentlySelected.ToString();
            HungerSlider.value = currentlySelected.Hunger;
            ThirstSlider.value = currentlySelected.Thirst;
            StaminaSlider.value = currentlySelected.CurrentStamina / currentlySelected.MaxStamina;

            if (currentlySelected.CurrentState == AnimalState.Wandering ||
                currentlySelected.CurrentState == AnimalState.Idle)
            {
                StateText.text = "Current State: N/A";
            }
            else
            {
                StateText.text = $"Current State: {currentlySelected.CurrentState}";
            }
            AdaptiveText.text = $"Adaptive: {currentlySelected.CurrentAdaptiveState} : {currentlySelected.Genome.tempResist:F1}";
            AgeText.text = $"Age: {currentlySelected.Age}";
            AgeStageText.text = $"Age Stage: {currentlySelected.lifeStage}";
            GenderText.text = $"Gender: {currentlySelected.AnimalSex}";
            HPText.text = $"HP: {currentlySelected.HP:F0} / 100";
        }
        else
        {
            NameText.text = "Name: N/A";
            StateText.text = "Current State: N/A";
            AdaptiveText.text = $"Adaptive: N/A";
            AgeText.text = "Age: N/A";
            AgeStageText.text = "Age Stage: N/A";
            GenderText.text = "Gender: N/A";
            HPText.text = $"HP: N/A";

            HungerSlider.value = 0f;
            ThirstSlider.value = 0f;
            StaminaSlider.value = 0f;
        }
    }

    public void SetCurrentlySelectedToHungry()
    {
        if (currentlySelected != null)
        {
            currentlySelected.Hunger = 0.31f;
        }
    }

    public void SetCurrentlySelectedToThirsty()
    {
        if (currentlySelected != null)
        {
            currentlySelected.Thirst = 0.31f;
        }
    }

    public void CameraLockToSelectedAnimal()
    {
        if (currentlySelected != null)
        {
            mouseController.LockToAnimal(currentlySelected);
        }
    }

    public void QuitToMainMenu() 
    {
        SceneManager.LoadScene("Menu");
    }
}
