using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelManagement
{
	public class GameOverMenu : Menu<GameOverMenu>
	{
		[SerializeField]
		private int mainMenuIndex = 0;

		public void OnRestartPressed()
		{
			Time.timeScale = 1;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			base.OnBackPressed();
		}

		public void OnMainMenuPressed()
		{
			Time.timeScale = 1;
			SceneManager.LoadScene(mainMenuIndex);

			MainMenu.Open();
		}

		public void OnQuitPressed()
		{
			Application.Quit();
		}
	}
}
