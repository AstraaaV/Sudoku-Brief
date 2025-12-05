using UnityEngine;
using UnityEditor;
using System.IO;
public class SudokuEditorWindow : EditorWindow
{
    private int[,] grid = new int[9, 9]; // Stores Sudoku numbers
    private string[] ruleOptions = { "Standard", "Diagonal" };
    private int chosenRule = 0;

    [MenuItem("Tools/Sudoku Editor")]
    public static void ShowWindow()
    {
        GetWindow<SudokuEditorWindow>("Sudoku Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Sudoku Puzzle Editor", EditorStyles.boldLabel);

        // Rule Selection
        GUILayout.Label("Select Puzzle Ruleset:");
        chosenRule = EditorGUILayout.Popup(chosenRule, ruleOptions);

        GUILayout.Space(10);

        DrawGrid(); // Draws 9x9 grid

        GUILayout.Space(15);

        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Import Puzzle"))
        {
            ImportPuzzle();
        }

        if(GUILayout.Button("Export Puzzle"))
        {
            ExportPuzzle();
        }

        if(GUILayout.Button("Solve Puzzle"))
        {
            SolvePuzzle();
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(10);
    }

    private void DrawGrid()
    {
        GUIStyle cellStyle = new GUIStyle();
        cellStyle.alignment = TextAnchor.MiddleCenter;
        cellStyle.fontSize = 14;

        for(int r = 0; r < 9; r++)
        {
            GUILayout.BeginHorizontal();
            for(int c = 0; c < 9; c++)
            {
                GUI.backgroundColor = IsCellValid(r, c) ? Color.white : new Color(1f, 0.7f, 0.7f);

                string input = GUILayout.TextField(
                    grid[r, c] == 0 ? "" : grid[r, c].ToString(),
                    cellStyle,
                    GUILayout.Width(25),
                    GUILayout.Height(25)
                    );

                if(int.TryParse(input, out int value))
                {
                    grid[r, c] = Mathf.Clamp(value, 1, 9);
                }
                else
                {
                    grid[r, c] = 0;
                }

                // Adds spacing between 3x3 boxes
                if (c == 2 || c == 5)
                    GUILayout.Space(5);
            }
            GUILayout.EndHorizontal();

            if (r == 2 || r == 5)
                GUILayout.Space(5);
        }
        GUI.backgroundColor = Color.white; // resets
    }

    private Color GetCellColour(int row, int col)
    {
        Color color = Color.white;

        // Light hightlight for diagonal
        if(chosenRule == 1) // diagonal
        {
            bool onMain = (row == col);
            bool onAnti = (row + col == 8);

            if(onMain && onAnti)
            {
                // Center cell
                color = new Color(0.8f, 0.7f, 1f); // purpley
            }
            else if(onMain)
            {
                color = new Color(0.7f, 0.9f, 1f); // light cyan
            }
            else if (onAnti)
            {
                color = new Color(1f, 0.8f, 1f); // light magenta
            }
        }

        if (!IsCellValid(row, col))
        {
            color = new Color(1f, 0.7f, 0.7f);
        }
        return color;
    }
    private void ImportPuzzle()
    {
        string path = EditorUtility.OpenFilePanel("Load Puzzle", "", "txt");

        if (string.IsNullOrEmpty(path)) return;

        string[] lines = File.ReadAllLines(path);

        if(lines.Length >= 10 && lines[9].StartsWith("#RULE:"))
        {
            string ruleLine = lines[9].Trim().ToUpper();
            if(ruleLine.Contains("DIAGONAL"))
            {
                chosenRule = 1;
            }
            else
            {
                chosenRule = 0;
            }
        }

        for (int r = 0; r < 9; r++)
        {
            if (r >= lines.Length) break;

            string[] items = lines[r].Split(' ', '\t');
            for (int c = 0; c < 9 && c < items.Length; c++)
            {
                grid[r, c] = items[c] == "." ? 0 : int.Parse(items[c]);
            }
        }

        EditorUtility.DisplayDialog("Loaded.", "Puzzle imported successfully.", "OK");
    }

    private bool IsCellValid(int row, int col)
    {
        int value = grid[row, col];
        if (value == 0) return true;

        // Checks row
        for(int c = 0; c < 9; c++)
        {
            if (c != col && grid[row, c] == value)
                return false;
        }

        // Checks column
        for(int r = 0; r < 9; r++)
        {
            if(r != row && grid[r, col] == value)
                return false;
        }

        // Checks 3x3 box
        int startR = row - (row % 3);
        int startC = col - (col % 3);

        for(int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                int rr = startR + r;
                int cc = startC + c;

                if ((rr != row || cc != col) && grid[rr, cc] == value)
                    return false;
            }
        }

        // Diagonal rules
        if(chosenRule == 1)
        {
            // Main diagonal (top-left to bottom-right)
            if(row == col)
            {
                for(int i = 0; i < 9; i++)
                {
                    if(i != row && grid[i, i] == value)
                        return false;
                }
            }

            // Anti-Diagonal (top-right to bottom-left)
            if(row + col == 8)
            {
                for(int i = 0; i < 9; i++)
                {
                    int aa = 8 - i;
                    if(i != row && grid[i, aa] == value)
                        return false;
                }
            }
        }
        return true;
    }
    private void ExportPuzzle()
    {
        string path = EditorUtility.SaveFilePanel("Save Puzzle", "", "puzzle.txt", "txt");

        if (string.IsNullOrEmpty(path)) return;

        using (StreamWriter writer = new StreamWriter(path))
        {
            for(int r = 0; r < 9; r++)
            {
                for(int c = 0; c < 9; c++)
                {
                    writer.Write(grid[r, c] == 0 ? "." : grid[r, c].ToString());
                    writer.Write(" ");
                }
                writer.WriteLine();
            }
            string ruleMeta = (chosenRule == 0) ? "#RULE: STANDARD" : "#RULE: DIAGONAL";
            writer.WriteLine(ruleMeta);
        }
        EditorUtility.DisplayDialog("Success!", "Puzzle exported succesfully.", "OK");
    }

    private void SolvePuzzle()
    {
        int[,] backup = (int[,])grid.Clone();

        if(SolveRecursive())
        {
            EditorUtility.DisplayDialog("Solved", "Puzzle solved successfully.", "OK");
        }
        else
        {
            grid = backup;
            EditorUtility.DisplayDialog("Unsolvable", "No solution found under current rules.", "OK");
        }
    }

    private bool SolveRecursive()
    {
        int row = -1, col = -1;

        if(!FindEmptyCell(ref row, ref col))
        {
            return true;
        }

        for(int num = 1; num <= 9; num++)
        {
            grid[row, col] = num;

            if(IsCellValid(row, col) && SolveRecursive())
            {
                return true;
            }

            // Backtrack
            grid[row, col] = 0;
        }
        return false;
    }

    private bool FindEmptyCell(ref int row, ref int col)
    {
        for(int r = 0; r < 9; r++)
        {
            for(int c = 0; c < 9; c++)
            {
                if (grid[r, c] == 0)
                {
                    row = r;
                    col = c;
                    return true;
                }
            }
        }
        return false;
    }
}
