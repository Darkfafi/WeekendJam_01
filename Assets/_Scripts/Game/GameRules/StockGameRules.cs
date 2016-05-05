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

	// confactories
	private ConCoroutines conCoroutines;

	public StockGameRules(GameHandler handler, int stockAmount) : base(handler)
	{
		conCoroutines = Ramses.Confactory.ConfactoryFinder.Instance.Give<ConCoroutines>();
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
		object spearContext = new object();
		conCoroutines.StartCoroutine(WaitToSpawnSpear(spearContext), spearContext);
    }

	public override void OnCorpseSpawnedEvent(Corpse corpse)
	{
		base.OnCorpseSpawnedEvent(corpse);
		SetStockAmountPlayer(corpse.PlayerOwnedCorpse, playersAndStocks[corpse.PlayerOwnedCorpse] - 1);
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
				if(playersAndStocks[prePlayerCheck] > item.Value)
				{
					rank++;
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

			if (CheckAmountOfPlayersLeft() == 1)
			{
				// TODO CHECK FOR SUDDEN DEATH
				gameHandler.EndGame();
			}
		}
	}

	private int CheckAmountOfPlayersLeft()
	{
		int amount = 0;
		foreach(KeyValuePair<Player, int> pair in playersAndStocks)
		{
			if(pair.Value > 0)
			{
				amount++;
			}
		}
		return amount;
	}

	private void PrepairPlayerToSpawn(Player player)
	{
		object spawnContext = new object();
		conCoroutines.StartCoroutine(SpawnProcess(player, waitForSpawnInSeconds, spawnContext), spawnContext);
	}

	private IEnumerator SpawnProcess(Player p, float time, object context)
	{
		yield return new WaitForSeconds(time);
		gameHandler.SpawnPlayerCharacter(p, gameHandler.Spawnpoints[Random.Range(0, gameHandler.Spawnpoints.Length)]);
		conCoroutines.StopContext(context);
	}

	private IEnumerator WaitToSpawnSpear(object context)
	{
		yield return new WaitForSeconds(3);
		gameHandler.SpawnWeapon(WeaponFactory.AllWeapons.Spear);
		conCoroutines.StopContext(context);
	}
}
