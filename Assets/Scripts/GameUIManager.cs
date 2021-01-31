using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameUIManager : MonoBehaviour, DogActions.IPlayerActions {
    private DogActions _actions;

    // Miki's Edit
    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unpaused;

    public AudioMixer audioMixer;

    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider ambienceSlider;
    public Slider masterSlider;

    // Miki's Edit

    //Olivia's edit for BoneToggles
    public GameObject skeletonPanel;

    [SerializeField] public List<Toggle> boneToggles;

    private int animatedBoneID;

    //Olivia's edit for BoneToggles

    [SerializeField] private GameObject pawseMenu;
    [SerializeField] private GameObject winMenu;

    private void Start() {
        _actions = new DogActions();
        _actions.Player.SetCallbacks(this);
        _actions.Player.Pawse.Enable();
    }

    public void Unpawse() {
        StartCoroutine(ClosePawse());
    }

    public void Pawse() {
        pawseMenu.SetActive(true);
        Time.timeScale = 0;
        // For Audio Mixer to switch Snapshot - Miki
        paused.TransitionTo(0.5f);
        //set selected button for keyboard navigation
        masterSlider.Select();
    }

    public void Quit() {
        // We may want to do this asynchronously, with some kind of loading indicator
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    [ContextMenu("Force win")]
    public void WinGame() {
        winMenu.SetActive(true);
        Time.timeScale = 0;
    }


    public void OnMove(InputAction.CallbackContext context) { }

    public void OnLook(InputAction.CallbackContext context) { }

    public void OnSniff(InputAction.CallbackContext context) { }

    public void OnDig(InputAction.CallbackContext context) { }

    public void OnPawse(InputAction.CallbackContext context) {
        if (context.performed) {
            if (pawseMenu.activeSelf) {
                Unpawse();
            }
            else {
                Pawse();
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context) { }

    public void OnBark(InputAction.CallbackContext context) { }

    IEnumerator ClosePawse() {
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;
        pawseMenu.SetActive(false);
        // For Audio Mixer to switch snapshot - Miki
        unpaused.TransitionTo(0.5f);
    }


    // Setting up sliders for audio

    public void SetMasterVolume()
    {
        audioMixer.SetFloat("MasterExpo", Mathf.Log(masterSlider.value) * 20);
    }


    public void SetMusicVolume ()
    {
        audioMixer.SetFloat("MusicExpo", Mathf.Log(musicSlider.value)*20);
    }

    public void SetSFXVolume()
    {
        audioMixer.SetFloat("SFXExpo", Mathf.Log(sfxSlider.value)*20);
    }

    public void SetAmbienceVolume()
    {
        audioMixer.SetFloat("AmbiExpo", Mathf.Log(ambienceSlider.value)*20);
    }

    //bone Toggle handling
    public void ToggleBone(int boneID)
    {
        animatedBoneID = boneID;
        skeletonPanel.GetComponent<Animator>().SetBool("Show", true);
        StartCoroutine(animateBonePanel());
    }

    IEnumerator animateBonePanel()
    {
        yield return new WaitForSecondsRealtime(1.1f);
        boneToggles[animatedBoneID].GetComponent<Toggle>().isOn = true;
        yield return new WaitForSecondsRealtime(1f);
        skeletonPanel.GetComponent<Animator>().SetBool("Show", false);
    }
}
