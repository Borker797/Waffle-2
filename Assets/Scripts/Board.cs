using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

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

    public IReadOnlyList<string> Wordles => _wordles;
    public IReadOnlyCollection<string> NonWordles => _nonWordles;
    public IReadOnlyCollection<string> AllAcceptedGuesses => _allAccepted;

    private List<string> _wordles;
    private HashSet<string> _nonWordles;
    private HashSet<string> _allAccepted;


    private string word;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
    }


    private void Start()
    {
        LoadData();
        SetRandomWord();
    }

    private void LoadData()
    {
        var wordlesText = Resources.Load<TextAsset>("wordles");
        _wordles = ParseJsonStringArray(wordlesText.text);

        var nonWordlesText = Resources.Load<TextAsset>("nonwordles");
        _nonWordles = new HashSet<string>(ParseJsonStringArray(nonWordlesText.text));

        _allAccepted = new HashSet<string>(_wordles);
        _allAccepted.UnionWith(_nonWordles);
    }

    // For JSON like: ["cigar","rebut", etc..]
    private static List<string> ParseJsonStringArray(string json)
    {
        var result = new List<string>();
        if (string.IsNullOrWhiteSpace(json)) return result;

        // make sure all words have no spaces
        json = json.ToLower().Trim();
        if (!json.StartsWith("[") || !json.EndsWith("]")) return result;

        // Extract quoted strings
        int i = 0;
        while (i < json.Length)
        {
            int quoteStart = json.IndexOf('"', i);
            if (quoteStart < 0) break;

            int quoteEnd = json.IndexOf('"', quoteStart + 1);
            if (quoteEnd < 0) break;

            string value = json.Substring(quoteStart + 1, quoteEnd - quoteStart - 1);
            if (!string.IsNullOrWhiteSpace(value))
                result.Add(value.Trim());

            i = quoteEnd + 1;
        }
        return result;
    }

    private void SetRandomWord()
    {
        word = _wordles[Random.Range(0, _wordles.Count)];
    }

    private void Update()
    {
        Row currentRow = rows[rowIndex];

        // handle backspacing
        if (Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            columnIndex = Mathf.Max(columnIndex - 1, 0);
            currentRow.tiles[columnIndex].SetLetter('\0');
        }
        else if (columnIndex >= currentRow.tiles.Length)
        {
            // submit row
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                SubmitRow(currentRow);
            }
        }
        else
        {
            for (int i = 0; i < SUPPORTED_KEYS.Length; i++)
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
        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile tile = row.tiles[i];
        }
    }
}