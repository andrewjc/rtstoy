using System;
using Game.Render;
using Game.Utils;
using Game.World;
using Game.World.Player;
using UnityEngine;

namespace Game
{

	public class GameMain : MonoBehaviour
	{

		public static GameMain _instance;

		private GameWorld gameWorld;
        private RtsCamera mainCamera;
        private DesktopUserInterface userInterface;

        public void Start()
		{
			_instance = this;
			Application.targetFrameRate = 30;
			Time.timeScale = 1;
			Debug.unityLogger.logEnabled = true;

            
            Camera.main.transform.position = gameWorld.getSpawnPointPosition() + (Vector3.forward * 5);
            Camera.main.transform.position = VectorUtil.sitOnTerrain(Camera.main.transform.position) + (Vector3.up * 5);

            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RtsCamera>();
            if (mainCamera != null)
            {
                mainCamera.LookAt = Camera.main.transform.position;
                mainCamera.Rotation = 180;
            }

        }

		public void Update()
		{
            userInterface.Update();
		}

		public void Awake()
        {
            gameWorld = new GameWorld();
            gameWorld.Run();

            userInterface = new DesktopUserInterface(this);
            userInterface.Awake();
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

        internal GameWorld GetGameWorld()
        {
            return gameWorld;
        }
    }
}