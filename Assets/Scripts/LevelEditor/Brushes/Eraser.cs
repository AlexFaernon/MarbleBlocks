using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Eraser : Brush
{
    [SerializeField] private GameObject feesh;
    [SerializeField] private GameObject jumper;
    [SerializeField] private GameObject sonic;
    [SerializeField] private Grid grid;

	private void Awake()
	{
		Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
	}

	public override void Draw(Tile tile)
	{
		EraseObject(tile);
	}
	
	private void EraseObject(Tile tile)
        {
            var oldTile = tile.GetSave();
            tile.ClearTile();
            GameObject character = null;
            if (tile.isSonicOnTile)
            {
                Destroy(GameObject.FindWithTag("Sonic"));
                character = sonic;
            }
            if (tile.isJumperOnTile)
            {
                Destroy(GameObject.FindWithTag("Jumper"));
                character = jumper;
            }
            if (tile.isFeeshOnTile)
            {
                Destroy(GameObject.FindWithTag("Feesh"));
                character = feesh;
            }
            
            Drawer.Undo.Push(() => RevertErase(tile, oldTile, character));
        }
    
        private void RevertErase(Tile tile, TileClass oldTile, GameObject selectedCharacter)
        {
            tile.TileClass = oldTile;
            if (selectedCharacter is not null)
            {
                var pos = grid.GetCellCenterWorld((Vector3Int)tile.gridPosition);
                var character = Instantiate(selectedCharacter, pos, quaternion.identity);
                if (selectedCharacter == sonic)
                {
                    character.GetComponent<Sonic>().enabled = false;
                }
                else if (selectedCharacter == feesh)
                {
                    character.GetComponent<Feesh>().enabled = false;
                }
                else if (selectedCharacter == jumper)
                {
                    character.GetComponent<Jumper>().enabled = false;
                }
            }
            Drawer.Redo.Push(() => EraseObject(tile));
        }
}