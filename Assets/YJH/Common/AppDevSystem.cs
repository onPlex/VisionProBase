using UnityEngine;
public class AppDevSystem : MonoBehaviour
{
    private static AppDevSystem instance;
    public static AppDevSystem Instance
    {
        get
        {
            // 만약 싱글톤 인스턴스가 null이라면, 
            // 씬 내에 있는 객체를 찾아서 할당하거나 
            // 새로 생성할 수도 있습니다.
            if (instance == null)
            {
                // 혹시 에디터에서 미리 존재할 수도 있으니 먼저 찾음
                instance = FindFirstObjectByType<AppDevSystem>();

                // 만약 없으면 직접 오브젝트를 생성
                if (instance == null)
                {
                    GameObject go = new GameObject("AppDevSystem");
                    instance = go.AddComponent<AppDevSystem>();
                }
            }

            return instance;
        }
    }

    // 다른 스크립트가 Singleton을 사용해 호출하더라도
    // 자기 자신을 참조하게끔 Awake()에서 할당
    private void Awake()
    {
        // 기존에 인스턴스가 이미 존재한다면 중복 제거
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        // 씬 전환 시에도 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);
    }

public void OnQuitApp()
{
    Application.Quit();
}
}