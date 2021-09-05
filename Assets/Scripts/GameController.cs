using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
    public FollowCamera FollowCamera;

    public GameObject PlayerPrefab;
    private Player player;
    public Player Player { get { return player; } }

    public enum eGameState
    {
        InTown,
        PlayerTurn,
        MonsterTurn
    }
    public eGameState GameState = eGameState.PlayerTurn;

    public void GoToState(eGameState _state)
    {
        GameState = _state;
        switch (GameState)
        {

            case eGameState.InTown:
                UIController.Instance.EnterTown();
                DungeonController.Instance.ExitDungeon();
                break;

            case eGameState.MonsterTurn:
                MonsterController.Instance.MoveMonsters();
                break;
        }
    }

    private void Start()
    {
        StartNewGame();
    }

    void StartNewGame()
    {
        CreatePlayer();
        DungeonController.Instance.CreateNewDungeon();
        GoToState(eGameState.PlayerTurn);
    }

    void CreatePlayer()
    {
        GameObject playerGO = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        player = playerGO.GetComponent<Player>();
        player.Reset();
        FollowCamera.Target = playerGO.transform;
    }

    public void ExitDungeon()
    {
        GoToState(eGameState.InTown);
    }
}
