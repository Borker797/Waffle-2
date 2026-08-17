using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class Board : MonoBehaviour
{
    // Keys able to be pressed 
    private static readonly Key[] SUPPORTED_KEYS = new Key[]
    {
        Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H,
        Key.I, Key.J, Key.K, Key.L, Key.M, Key.N, Key.O, Key.P,
        Key.Q, Key.R, Key.S, Key.T, Key.U, Key.V, Key.W, Key.X,
        Key.Y, Key.Z
    };

    private Row[] rows;
    
    private int rowIndex;
    private int columnIndex;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
    }

   private void Update()
   {    

        Row currentRow = rows[rowIndex]; 
    
    if (Keyboard.current[Key.Backspace].wasPressedThisFrame)
    {
        columnIndex = Mathf.Max(columnIndex - 1, 0);
        currentRow.tiles[columnIndex].SetLetter('\0');
    }

    
    else if (columnIndex >= currentRow.tiles.Length)
    {
        if (Keyboard.current[Key.Enter].wasPressedThisFrame)
        {
            SubmitRow(currentRow);
        }
    } 
    else
    {
        for ( int i = 0; i < SUPPORTED_KEYS.Length; i++ )
         {
            if (Keyboard.current[SUPPORTED_KEYS[i]].wasPressedThisFrame)
            {
                currentRow.tiles[columnIndex].SetLetter((char)('A' + i));
                columnIndex++;
                break;
            }
        }
    }

     
   }

   private void SubmitRow(Row row)
   {
        ///..
   }
}
  