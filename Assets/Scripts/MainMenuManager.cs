using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    public static MainMenuManager instance = null;
    public Slider VolumeSlider;
    public Text TextPrompt;
    public Text StartButtonText;

    void Awake() {
        if (instance == null) { instance = this; }
        else if (instance != null) { Destroy(gameObject); }

        TextPrompt.text = "Press Load Music to Select Song";
    }

    public void OnLoadButtonPressed() {
        AudioLoader.instance.LoadAudio();

        if (AudioLoader.instance.Loaded) {
            StartButtonText.text = "Start";
            OnVolSliderChanged();
            TextPrompt.text = "Loaded File: " + AudioLoader.instance.SongName;
        }
    }

    public void OnPlayButtonPressed() {
        if (!(AudioLoader.instance.SoundSystem.clip == null)) {
            if (StartButtonText.text == "Start") {
                TextPrompt.text = "Playing File:\n" + AudioLoader.instance.SongName;
                StartButtonText.text = "Stop";
                AudioLoader.instance.PlayAudio();
            }
            else {
                TextPrompt.text = "Stopped:\n" + AudioLoader.instance.SongName;
                StartButtonText.text = "Start";
                AudioLoader.instance.StopAudio();
            }
        }
    }

    public void OnVolSliderChanged() {
        if (AudioLoader.instance.Loaded) {
            TextPrompt.text = "Volume: " + VolumeSlider.value * 100 + "%";
            AudioLoader.instance.ChangeVolume(VolumeSlider.value);
        }
    }

    public void OnQuitButtonPressed() {
        if (!Application.isEditor) {
            AudioLoader.instance.UnloadAudio();
            Application.Quit();
        }
    }
}
