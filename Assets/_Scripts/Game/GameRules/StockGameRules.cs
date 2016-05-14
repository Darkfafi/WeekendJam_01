using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StockGameRules : BaseGameRules {

	public delegate void PlayerHandler(Player player);
	public delegate void PlayerStockHandler(Player player, int stock);

	public event PlayerStockHandler PlayerStockChangedEvent;
	public event PlayerHandler PlayerOutOfStocksEvent;

	public int StartingStockAmount { get; private set; }

	private Dictionary<Player, int> playersAndStocks = new Dictionary<Player, int>();
	private float waitForSpawnInSeconds = 2f;

	public StockGameRules(GameHandler handler, int stockAmount) : base(handler)
	{
		StartingStockAmount = stockAmount;
    }

	public override void Start()
	{
		base.Start();

		foreach(Player p in gameHandler.ActivePlayers.GetAllPlayers())
		{
			playersAndStocks.Add(p, StartingStockAmount);
        }
		gameHandler.SpawnAllPlayers();
		gameHandler.SpawnWeapon(WeaponFactory.AllWeapons.Spear, 3);
    }

	public override void OnCorpseSpawnedEvent(Corpse corpse)
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
			// TODO CHECK FOR SUDDEN DEATH
			EndGame();
        }
	}

	protected virtual void EndGame()
	{
		gameHandler.EndGame();
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
