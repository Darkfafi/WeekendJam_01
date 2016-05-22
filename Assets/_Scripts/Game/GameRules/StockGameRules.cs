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

	public StockGameRules(int stockAmount) : base()
	{
		StartingStockAmount = stockAmount;
    }

	public override void Start(GameHandler handler)
	{
		base.Start(handler); 
        audioManager.PlaySoloAudio("TrompetMusic1", ConAudioManager.MUSIC_STATION, 0.5f);
		audioManager.PlayAudio("VoiceFight");
		foreach (Player p in GameHandler.ActivePlayers.GetAllPlayers())
		{
			playersAndStocks.Add(p, StartingStockAmount);
        }
		GameHandler.SpawnAllPlayers();
		GameHandler.SpawnWeapon(WeaponFactory.AllWeapons.Spear, 3);
    }

	public override void OnCorpseSpawnedEvent(Corpse corpse)
	{
		if (listeningToDeathEvents)
		{
			base.OnCorpseSpawnedEvent(corpse);
			if (StartingStockAmount != 0 || SuddenDeathActivated)
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
		Dictionary<int, List<Player>> playersSortedOnLifesRank = new Dictionary<int, List<Player>>();
		Player prePlayerCheck = null;
		int rank = 0;
		foreach (KeyValuePair<Player, int> item in playersAndStocks.OrderByDescending(key => key.Value))
		{
			if(prePlayerCheck != null)
			{
				// If the player has died more times, or has died the same amount of times but has less kills then his rank is lower then the other player
				if (playersAndStocks[prePlayerCheck] > item.Value)
				{
					rank++;
				}
            }
			if (!playersSortedOnLifesRank.ContainsKey(rank))
			{
				playersSortedOnLifesRank.Add(rank, new List<Player>());
			}
			playersSortedOnLifesRank[rank].Add(item.Key);
			prePlayerCheck = item.Key;
        }
		int realRank = 0;
		foreach(KeyValuePair<int, List<Player>> rankPair in playersSortedOnLifesRank)
		{
			prePlayerCheck = null;
			rank = 0;
			if(rankPair.Value.Count > 1)
			{
				rankPair.Value.Sort(SortOnKillsMethod);
			}

			foreach(Player p in rankPair.Value)
			{
				if (prePlayerCheck != null)
				{
					if (GameHandler.BattleHistoryLog.GetAllKillsOfPlayer(prePlayerCheck).Length > GameHandler.BattleHistoryLog.GetAllKillsOfPlayer(p).Length)
					{
						rank++;
					}
				}
				prePlayerCheck = p;
				realRank = rank + rankPair.Key;
				returnDic.Add(p, realRank);
			}
		}
		return returnDic;
	}
	
	private int SortOnKillsMethod(Player pOne, Player pTwo)
	{
		if(GameHandler.BattleHistoryLog.GetAllKillsOfPlayer(pOne).Length > GameHandler.BattleHistoryLog.GetAllKillsOfPlayer(pTwo).Length)
		{
			return -1;
		}
		else
		{
			return 1;
		}
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
			int oldAmount = playersAndStocks[player];
			playersAndStocks[player] = amount;

			if (PlayerStockChangedEvent != null)
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
			if (oldAmount > playersAndStocks[player]) { 
				EndGameCheck();
			}
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
			GameHandler.EndGame();
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
		audioManager.StopAudio(ConAudioManager.MUSIC_STATION);
		audioManager.PlayAudio("VoiceSuddenDeath");
		audioManager.PlaySoloAudio("HolyMusic1", ConAudioManager.MUSIC_STATION, 0.3f);
		foreach (Player p in GameHandler.ActivePlayers.GetAllPlayers())
		{
			GameHandler.DestroyPlayerCharacter(p);
			if (playersForSuddenDeath.Contains(p))
			{
				SetStockAmountPlayer(p, 1);
			}
		}
		for(int i = 0; i < 20; i++)
		{
			GameHandler.SpawnWeapon(WeaponFactory.AllWeapons.Spear, i * 0.25f);
		}
    }

	private int CheckAmountOfPlayersLeft()
	{
		int amount = 0;
		foreach(KeyValuePair<Player, int> pair in playersAndStocks)
		{
			if(pair.Value > 0 || (StartingStockAmount == 0 && !SuddenDeathActivated))
			{
				amount++;
			}
		}
		return amount;
	}

	private void PrepairPlayerToSpawn(Player player)
	{
		GameHandler.SpawnPlayerCharacter(player, GameHandler.Spawnpoints[Random.Range(0, GameHandler.Spawnpoints.Length)], waitForSpawnInSeconds);
	}
}
