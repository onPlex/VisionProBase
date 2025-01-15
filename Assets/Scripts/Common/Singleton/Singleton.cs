using UnityEngine;
using System.Collections;

namespace Novaflo.Common
{
    // abstract 추상 클래스고
    // MonoBehaviour를 상속 받았고
    // T는 MonoBehaviour를 상속받은 클래스여야 한다.
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance = null;
        private static object _lock = new object();
        private static bool _applicationQuit = false;

        public static T Instance
        {
            get
            {
                // 고스트 객체 생성 방지용
                // 위에서 설명한 leak을 방지한다.
                if (_applicationQuit)
                {
                    // null 리턴
                    return null;
                }

                // thread-safe
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        // 현재 씬에 싱글톤이 있나 찾아본다.
                        _instance = FindObjectOfType<T>();

                        // 없으면
                        if (_instance == null)
                        {
                            // 해당 컴포넌트 이름을 가져온다.
                            string componentName = typeof(T).ToString();

                            // 해당 컴포넌트 이름으로 게임 오브젝트 찾기
                            GameObject findObject = GameObject.Find(componentName);

                            // 없으면
                            if (findObject == null)
                            {
                                // 생성
                                findObject = new GameObject(componentName);
                            }

                            // 생성된 오브젝트에, 컴포넌트 추가
                            _instance = findObject.AddComponent<T>();

                            // 씬이 변경되어도 객체가 유지되도록 설정
                            DontDestroyOnLoad(_instance);
                        }
                    }

                    // 객체 리턴
                    return _instance;
                }
            }
        }

        // 원칙적으로 싱글톤은 응용 프로그램이 종료될때, 소멸되어야 한다.
        // 유니티에서 응용 프로그램이 종료되면 임의 순서대로 오브젝트가 파괴된다.
        // 만약 싱글톤 오브젝트가 파괴된 이후, 싱글톤 오브젝트가 호출된다면
        // 앱의 재생이 정지된 이후에도, 에디터 씬에서 고스트 객체가 생성된다.
        // 고스트 객체의 생성을 방지하기 위해서 상태를 관리한다.

        // 앱이 종료될때 호출
        protected virtual void OnApplicationQuit()
        {
            _applicationQuit = true;
        }

        // 객체가 파괴될때 호출
        public virtual void OnDestroy()
        {
            _applicationQuit = true;
        }
    }
}