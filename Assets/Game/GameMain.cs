using Game.World;
using UnityEngine;

namespace Game
{

	public class GameMain : MonoBehaviour
	{

		public static GameMain _instance;

		private GameWorld _gameWorld;

		public void Start()
		{
			_instance = this;
			Application.targetFrameRate = 30;
			Time.timeScale = 1;
			Debug.unityLogger.logEnabled = true;

			_gameWorld = new GameWorld();
			_gameWorld.Run();

		}

		public void Update()
		{
		}

		public void Awake()
		{
		}

		public void OnRequestMouseCapture()
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = false;
		}

		public static GameMain GetInstance()
		{
			return _instance;
		}

	}
}