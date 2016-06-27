using UnityEngine;
using System.Collections;
using System.IO;
using NAudio;
using NAudio.Wave;

public class AudioLoader : MonoBehaviour {
    private float[] AudioData;
    private float[] ReadBuffer;
    private AudioFileReader AFR;
    private string[] MusicPath;
    private AudioClip clip;

    public AudioSource SoundSystem;

    public static AudioLoader instance = null;

    public bool Loaded { get; private set; }
    public string SongName { get; private set; }

    void Awake() {
        if (instance == null) { instance = this; }
        else if (instance != null) { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);
        Loaded = false;
    }

    void OnDestroy() {
        UnloadAudio();
    }

    private bool LoadAudioBytestream(string path) {
        try {
            WWW www = new WWW(path);

            if (www.error == null) {
                AFR = new AudioFileReader(path);
                AudioData = new float[(int)AFR.Length];

                AFR.Read(AudioData, 0, (int)AFR.Length);

                clip = AudioClip.Create(Path.GetFileNameWithoutExtension(path), (int)AFR.Length, AFR.WaveFormat.Channels, AFR.WaveFormat.SampleRate, false);
                clip.SetData(AudioData, 0);

                SoundSystem.clip = clip;
            
                return true;
            }
        }
        catch(System.Exception ex) {
            Debug.LogWarning("ERROR: " + ex.Message);
        }

        return false;
    }

    public void LoadAudio() {
        UnloadAudio();

        System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
        dialog.Title = "Open Audio File";
        dialog.Filter = "MP3 audio (*.mp3) | *.mp3";

        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
            if (!LoadAudioBytestream(dialog.FileName)) {
                System.Windows.Forms.MessageBox.Show("Cannot open mp3 file!");
                return;
            }

            Loaded = true;
            SongName = dialog.FileName;
            Resources.UnloadUnusedAssets();
        }
    }

    public void PlayAudio() {
        if (!SoundSystem.isPlaying) {
            SoundSystem.Play();
        }
    }

    public void ChangeVolume(float volume) {
        SoundSystem.volume = volume;
    }

    public void StopAudio() {
        if (SoundSystem != null) {
            if (SoundSystem.isPlaying) {
                SoundSystem.Stop();
            }
        }
    }

    public void UnloadAudio() {
        Loaded = false;
        SongName = "";

        StopAudio();

        clip = null;

        if (SoundSystem != null) {
            SoundSystem.clip = null;
        }
    }
}
