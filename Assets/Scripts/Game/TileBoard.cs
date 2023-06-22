using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private TileState[] _tileStates;

    private TileGrid _grid;
    private List<Tile> _tiles;
    private bool _waiting;
    
    public bool CheckForGameOver()
    {
        if (_tiles.Count != _grid.size) {
            return false;
        }

        foreach (var tile in _tiles)
        {
            TileCell up = _grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = _grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = _grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = _grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.tile)) {
                return false;
            }

            if (down != null && CanMerge(tile, down.tile)) {
                return false;
            }

            if (left != null && CanMerge(tile, left.tile)) {
                return false;
            }

            if (right != null && CanMerge(tile, right.tile)) {
                return false;
            }
        }

        return true;
    }

    private void Awake()
    {
        _grid = GetComponentInChildren<TileGrid>();
        _tiles = new List<Tile>(16);
        
        EventBus.GameEvents.OnGameStarted += OnStartGame;
        EventBus.InputEvents.OnInputMoveChange += OnInputMoveChange;
    }

    private void OnDestroy()
    {
        EventBus.GameEvents.OnGameStarted -= OnStartGame;
        EventBus.InputEvents.OnInputMoveChange -= OnInputMoveChange;
    }

    private void OnStartGame()
    {
        ClearBoard();
        CreateTile(2);
    }

    private void ClearBoard()
    {
        foreach (var cell in _grid.cells) {
            cell.tile = null;
        }

        foreach (var tile in _tiles) {
            Destroy(tile.gameObject);
        }

        _tiles.Clear();
    }

    private void CreateTile(int count = 1)
    {
        for (int i = 0; i < count; i++) {
            Tile tile = Instantiate(_tilePrefab, _grid.transform);
            tile.SetState(_tileStates[0], 2);
            tile.Spawn(_grid.GetRandomEmptyCell());
            _tiles.Add(tile);
            EventBus.GameEvents.OnTileSpawned?.Invoke();
        }
    }
    
    private void OnInputMoveChange(Vector2 direction)
    {
        if (_waiting == false)
        {
            if (direction == Vector2.up) {
                Move(Vector2Int.up, 0, 1, 1, 1);
            } else if (direction == Vector2.left) {
                Move(Vector2Int.left, 1, 1, 0, 1);
            } else if (direction == Vector2.down) {
                Move(Vector2Int.down, 0, 1, _grid.height - 2, -1);
            } else if (direction == Vector2.right) {
                Move(Vector2Int.right, _grid.width - 2, -1, 0, 1);
            }
        }
    }

    private void Move(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;

        for (int x = startX; x >= 0 && x < _grid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < _grid.height; y += incrementY)
            {
                TileCell cell = _grid.GetCell(x, y);

                if (cell.occupied) {
                    changed |= MoveTile(cell.tile, direction);
                                        
                }
            }
        }

        if (changed) {
            EventBus.GameEvents.OnTileMoved?.Invoke();
            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = _grid.GetAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        {
            if (adjacent.occupied)
            {
                if (CanMerge(tile, adjacent.tile))
                {
                    MergeTiles(tile, adjacent.tile);
                    return true;
                }

                break;
            }

            newCell = adjacent;
            adjacent = _grid.GetAdjacentCell(adjacent, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }

    private bool CanMerge(Tile a, Tile b)
    {
        return a.number == b.number && !b.locked;
    }

    private void MergeTiles(Tile a, Tile b)
    {
        _tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, _tileStates.Length - 1);
        int number = b.number * 2;

        b.SetState(_tileStates[index], number);

        EventBus.GameEvents.OnTileMerged?.Invoke(number);
        
        if(number == 2048)
            EventBus.GameEvents.OnGameEnded?.Invoke(true);
    }

    private int IndexOf(TileState state)
    {
        for (int i = 0; i < _tileStates.Length; i++)
        {
            if (state == _tileStates[i]) {
                return i;
            }
        }

        return -1;
    }

    private IEnumerator WaitForChanges()
    {
        _waiting = true;

        yield return new WaitForSeconds(0.1f);

        _waiting = false;

        foreach (var tile in _tiles) {
            tile.locked = false;
        }

        if (_tiles.Count != _grid.size) {
            CreateTile();
        }

        if (CheckForGameOver()) {
            EventBus.GameEvents.OnGameEnded?.Invoke(false);
        }
    }
}
