using UnityEngine;

namespace LevelManagement
{
	public class MainMenu : Menu<MainMenu>
	{
		public void OnPlayPressed()
		{
			LevelLoader.LoadNextLevel();
		}

		public void OnSettingPressed()
		{
			SettingMenu.Open();
		}

		public void OnCreditsPressed()
		{
			CreditsMenu.Open();
		}

		public void OnLeaderboardPressed()
		{
			LeaderboardMenu.Open();
		}

		public override void OnBackPressed()
		{
			Application.Quit();
		}

	}
}
