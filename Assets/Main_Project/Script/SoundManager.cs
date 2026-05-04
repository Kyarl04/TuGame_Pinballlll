using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;

    [Header("BGM Clips")]
    public AudioClip menuBGM; // 메뉴용 음악
    public AudioClip gameBGM; // 인게임용 음악

    void Awake()
    {
        // 싱글톤 유지: 씬이 바뀌어도 파괴되지 않음
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // 씬이 로드될 때마다 실행될 함수 등록
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
{
    // 스페이스바를 누르면 현재 소리가 재생 중인지 로그를 찍어줍니다.
    if (Input.GetKeyDown(KeyCode.T))
    {
        Debug.Log($"재생 중인가? : {bgmSource.isPlaying}");
        Debug.Log($"현재 볼륨 : {bgmSource.volume}");
        Debug.Log($"현재 클립 이름 : {bgmSource.clip?.name}");
        Debug.Log($"전역 볼륨 : {AudioListener.volume}");
    }
}

    // 씬이 로드될 때마다 호출되는 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    // 1. 현재 씬의 메인 카메라(Audio Listener가 있는 곳)를 확인
    // 씬 전환 후 리스너가 제대로 동작하도록 강제로 갱신해주는 효과가 있습니다.
    AudioListener currentListener = FindObjectOfType<AudioListener>();
    if (currentListener == null)
    {
        Debug.LogWarning(scene.name + " 씬에 Audio Listener가 없습니다!");
    }

    // 2. 씬 이름에 따른 배경음 교체 로직
    if (scene.name == "1_MainMenu") 
    {
        PlayBGM(menuBGM);
    }
    else if (scene.name == "2_GameScean") 
    {
        PlayBGM(gameBGM);
    }
}

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource.clip == clip) return; // 이미 재생 중이면 리턴

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void SetVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    // 오브젝트 삭제 시 이벤트 연결 해제
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}