using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StockGameRules : BaseGameRules {

	public delegate void PlayerHandler(Player player);
	public delegate void PlayerStockHandler(Player player, int stock);

	public event PlayerStockHandler PlayerStockChangedEvent;
	public event PlayerHandler PlayerOutOfStocksEvent;
	public bool SuddenDeathActivated { get; private set; }
	public int StartingStockAmount { get; private set; }

	private Dictionary<Player, int> playersAndStocks = new Dictionary<Player, int>();
	private float waitForSpawnInSeconds = 2f;
	private bool listeningToDeathEvents = true;

	public StockGameRules(GameHandler handler, int stockAmount) : base(handler)
	{
		StartingStockAmount = stockAmount;
    }

	public override void Start()
	{
		base.Start(); 
        audioManager.PlayAudio("TrompetMusic1", ConAudioManager.MUSIC_STATION, 0.5f);
		audioManager.PlayAudio("VoiceFight");
		foreach (Player p in gameHandler.ActivePlayers.GetAllPlayers())
		{
			playersAndStocks.Add(p, StartingStockAmount);
        }
		gameHandler.SpawnAllPlayers();
		gameHandler.SpawnWeapon(WeaponFactory.AllWeapons.Spear, 3);
    }

	public override void OnCorpseSpawnedEvent(Corpse corpse)
	{
		if (listeningToDeathEvents)
		{
			base.OnCorpseSpawnedEvent(corpse);
			if (StartingStockAmount != 0)
			{
				SetStockAmountPlayer(corpse.PlayerOwnedCorpse, playersAndStocks[corpse.PlayerOwnedCorpse] - 1);
			}
			else
			{
				PrepairPlayerToSpawn(corpse.PlayerOwnedCorpse);
			}
		}
	}

	public override Dictionary<Player, int> GetPlayersSortedOnRank()
	{
		Dictionary<Player, int> returnDic = new Dictionary<Player, int>();

		Player prePlayerCheck = null;
		int rank = 0;
		foreach (KeyValuePair<Player, int> item in playersAndStocks.OrderByDescending(key => key.Value))
		{
			if(prePlayerCheck != null)
			{
				// If the player has died more times, or has died the same amount of times but has less kills then his rank is lower then the other player
				if (playersAndStocks[prePlayerCheck] > item.Value
					|| (playersAndStocks[prePlayerCheck] == item.Value && 
						gameHandler.BattleHistoryLog.GetAllKillsOfPlayer(prePlayerCheck).Length > gameHandler.BattleHistoryLog.GetAllKillsOfPlayer(item.Key).Length))
				{
					rank++;
				}else if(playersAndStocks[prePlayerCheck] == item.Value &&
						gameHandler.BattleHistoryLog.GetAllKillsOfPlayer(prePlayerCheck).Length < gameHandler.BattleHistoryLog.GetAllKillsOfPlayer(item.Key).Length)
				{
					returnDic[prePlayerCheck] += 1;
                }
			}
			returnDic.Add(item.Key, rank);
			prePlayerCheck = item.Key;
        }
		return returnDic;
	}

	public int GetStockAmountOfPlayer(Player player)
	{
		if(playersAndStocks.ContainsKey(player))
		{
			return playersAndStocks[player];
        }
		else
		{
			Debug.LogError("Player: " + player + " is not in the stock list");
			return -1;
		}
	}

	private void SetStockAmountPlayer(Player player, int amount)
	{
		if (playersAndStocks.ContainsKey(player))
		{
			playersAndStocks[player] = amount;

			if(PlayerStockChangedEvent != null)
			{
				PlayerStockChangedEvent(player, amount);
            }

			if (amount <= 0)
			{
				if (PlayerOutOfStocksEvent != null)
				{
					PlayerOutOfStocksEvent(player);
				}
			}
			else
			{
				PrepairPlayerToSpawn(player);
			}
			EndGameCheck();
		}
	}

	protected virtual void EndGameCheck()
	{
		if (CheckAmountOfPlayersLeft() == 1)
		{
			EndGame();
        }
	}

	protected virtual void EndGame()
	{
		List<Player> playersOnFirstPlace = new List<Player>();
		listeningToDeathEvents = false;
        foreach (KeyValuePair<Player,int> pWithRank in GetPlayersSortedOnRank())
		{
			if(pWithRank.Value == 0)
			{
				playersOnFirstPlace.Add(pWithRank.Key);
            }
		}

		if (!SuddenDeathActivated)
		{
			audioManager.PlayAudio("VoiceRound");
		}

		if (playersOnFirstPlace.Count > 1)
		{
			Timer t = new Timer(3, 0);
			t.TimerEndedEvent += () => { SuddenDeathTimerEnded(playersOnFirstPlace.ToArray(), t); };
			t.Start();
        }
		else
		{
			gameHandler.EndGame();
		}
	}

	private void SuddenDeathTimerEnded(Player[] playersForSuddenDeath, Timer timer)
	{
        timer.TimerEndedEvent -= () => { SuddenDeathTimerEnded(playersForSuddenDeath.ToArray(), timer); };
		timer.Stop();
		timer = null;
		SuddenDeath(playersForSuddenDeath);
    }


	protected virtual void SuddenDeath(Player[] playersForSuddenDeath)
	{
		SuddenDeathActivated = true;
		listeningToDeathEvents = true;
       // Ramses.Confactory.ConfactoryFinder.Instance.Give<ConSceneSwitcher>().FakeSwitchScreen();
		audioManager.StopAudio(ConAudioManager.MUSIC_STATION);
		audioManager.PlayAudio("VoiceSuddenDeath");
		audioManager.PlayAudio("HolyMusic1", ConAudioManager.MUSIC_STATION, 0.3f);
		foreach (Player p in playersForSuddenDeath)
		{
			gameHandler.DestroyPlayerCharacter(p);
			SetStockAmountPlayer(p, 1);
		}
		for(int i = 0; i < 20; i++)
		{
			gameHandler.SpawnWeapon(WeaponFactory.AllWeapons.Spear, i * 0.25f);
		}
    }

	private int CheckAmountOfPlayersLeft()
	{
		int amount = 0;
		foreach(KeyValuePair<Player, int> pair in playersAndStocks)
		{
			if(pair.Value > 0 || StartingStockAmount == 0)
			{
				amount++;
			}
		}
		return amount;
	}

	private void PrepairPlayerToSpawn(Player player)
	{
		gameHandler.SpawnPlayerCharacter(player, gameHandler.Spawnpoints[Random.Range(0, gameHandler.Spawnpoints.Length)], waitForSpawnInSeconds);
	}
}
