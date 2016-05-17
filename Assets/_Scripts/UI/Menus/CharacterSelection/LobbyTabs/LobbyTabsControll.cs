using UnityEngine;
using System.Collections;
using Ramses.SectionButtons;

public class LobbyTabsControll : MonoBehaviour {

	[SerializeField]
	private CharacterGameLobby lobby;

	[SerializeField]
	private LobbyTab lobbySettingsTab;

	[SerializeField]
	private LobbyTab gameRulesTab;

	private MultiInputUser inputUsers;

	private LobbyTab currentlySelectedLobbyTab = null;

	void Awake ()
	{
		inputUsers = gameObject.GetComponent<MultiInputUser>();
    }

	void OnEnable()
	{
		inputUsers.InputBindingUsedEvent -= OnInputUsedEvent;
		inputUsers.InputBindingUsedEvent += OnInputUsedEvent;
	}

	void OnDisable()
	{
		inputUsers.InputBindingUsedEvent -= OnInputUsedEvent;
	}

	private void OnInputUsedEvent(ConGameInputBindings.BindingTypes type, InputAction action)
	{
		if (action.Type == InputItem.InputType.KeyCode && action.KeyActionValue == InputAction.KeyAction.OnKeyDown)
		{
			if (action.Name == InputNames.USE)
			{
				if (currentlySelectedLobbyTab == null)
				{
					SelectLobbyTab(lobbySettingsTab);
                }
				else if (currentlySelectedLobbyTab == lobbySettingsTab)
				{
					CloseCurrentTab();
				}
			}
			if (action.Name == InputNames.GRAB_THROW)
			{
				if (currentlySelectedLobbyTab == null)
				{
					SelectLobbyTab(gameRulesTab);
                }
				else if(currentlySelectedLobbyTab == gameRulesTab)
				{
					CloseCurrentTab();
				}
			}
		}
		if (currentlySelectedLobbyTab != null)
		{
			currentlySelectedLobbyTab.GetInput(type, action);
		}
	}
	private void SelectLobbyTab(LobbyTab lobbyTab)
	{
		currentlySelectedLobbyTab = lobbyTab;
		lobbyTab.Open();
		lobby.SetListeningToInput(false);
		lobbyTab.SetActiveState(true);
    }
	private void CloseCurrentTab()
	{
		if (currentlySelectedLobbyTab != null)
		{
			currentlySelectedLobbyTab.Close();
			currentlySelectedLobbyTab.SetActiveState(false);
			currentlySelectedLobbyTab = null;
			lobby.SetListeningToInput(true);
		}
	}
}
