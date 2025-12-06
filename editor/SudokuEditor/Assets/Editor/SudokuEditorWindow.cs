using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using Codice.Client.Common.GameUI;
public class SudokuEditorWindow : EditorWindow
{
    private int[,] grid = new int[9, 9]; // Stores Sudoku numbers
    private string[] ruleOptions = { "Standard", "Diagonal", "Anti-Knight"};
    private int chosenRule = 0;

    private int steps = 0;
    private int backtracks = 0;
    private System.Diagnostics.Stopwatch stopwatch;

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

        if(GUILayout.Button("Generate Puzzle"))
        {
            GeneratePuzzle();
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

        int cellSize = 25;
        int thin = 1;
        int thick = 3;

        Rect gridRect = GUILayoutUtility.GetRect(
            cellSize * 9,
            cellSize * 9
            );

        // Draw background
        EditorGUI.DrawRect(new Rect(gridRect.x, gridRect.y, gridRect.width, gridRect.height), Color.white);

        // Draw grid lines
        // Vertical lines
        for (int x = 0; x <= 9; x++)
        {
            int thickness = (x % 3 == 0) ? thick : thin;
            float xpos = gridRect.x + x * cellSize - (thickness * 0.5f);

            EditorGUI.DrawRect(
                new Rect(xpos, gridRect.y, thickness, cellSize * 9),
                Color.black
                );
        }

        // Horizontal lines
        for (int y = 0; y <= 9; y++)
        {
            int thickness = (y % 3 == 0) ? thick : thin;
            float ypos = gridRect.y + y * cellSize - (thickness * 0.5f);

            EditorGUI.DrawRect(
                new Rect(gridRect.x, ypos, cellSize * 9, thickness),
                Color.black
                );
        }

        // Now draw each cell on top
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                float x = gridRect.x + c * cellSize + 1;
                float y = gridRect.y + r * cellSize + 1;

                Rect cellRect = new Rect(x, y, cellSize - 2, cellSize - 2);

                Color bg = GetCellColour(r, c);
                EditorGUI.DrawRect(cellRect, bg);

                string input = GUI.TextField(
                    cellRect,
                    grid[r, c] == 0 ? "" : grid[r, c].ToString(),
                    cellStyle
                    );

                if (int.TryParse(input, out int val))
                    grid[r, c] = Mathf.Clamp(val, 1, 9);
                else
                    grid[r, c] = 0;
            }
        }

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

        // Anti-knight rules
        if (chosenRule == 2)
        {
            int[,] knightMoves =
            {
                { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 },
                { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 }
            };

            for (int i = 0; i < knightMoves.GetLength(0); i++)
            {
                int rr = row + knightMoves[i, 0];
                int cc = col + knightMoves[i, 1];

                if (rr >= 0 && rr < 9 && cc >= 0 && cc < 9)
                {
                    if (grid[rr, cc] == value)
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

    private bool GenerateFullBoard()
    {
        // Clear grid first
        for(int r = 0; r < 9; r++)
        {
            for(int c = 0; c < 9; c++)
            {
                grid[r, c] = 0;
            }
        }
        return GenerateRecursive();
    }

    private bool GenerateRecursive()
    {
        int row = -1, col = -1;

        if (!FindEmptyCell(ref row, ref col))
            return true;

        // Randomised order
        System.Random random = new System.Random();
        int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        nums = nums.OrderBy(x => random.Next()).ToArray();

        foreach (int num in nums)
        {
            grid[row, col] = num;

            if(IsCellValid(row, col) && GenerateRecursive())
                return true;

            grid[row, col] = 0;
        }
        return false;
    }

    private void RemoveClues(int removals = 40)
    {
        System.Random rand = new System.Random();

        int removed = 0;
        while (removed < removals)
        {
            int r = rand.Next(0, 9);
            int c = rand.Next(0, 9);

            if (grid[r, c] != 0)
            {
                grid[r, c] = 0;
                removed++;
            }
        }
    }

    private void GeneratePuzzle()
    {
        if (GenerateFullBoard())
        {
            RemoveClues(40);
            EditorUtility.DisplayDialog("Generated!", "Puzzle generated!", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Generation failed.", "OK");
        }
    }
    private void SolvePuzzle()
    {
        int[,] backup = (int[,])grid.Clone();

        steps = 0;
        backtracks = 0;
        stopwatch = System.Diagnostics.Stopwatch.StartNew();

        if(SolveRecursive())
        {
            stopwatch.Stop();
            EditorUtility.DisplayDialog("Solved",
                $"Puzzle solved successfully.\n\n" + 
                $"Steps Tried: {steps}\n" +
                $"Backtracks: {backtracks}\n" +
                $"Time: {stopwatch.ElapsedMilliseconds} ms",
                "OK");
        }
        else
        {
            stopwatch.Stop();
            grid = backup;
            EditorUtility.DisplayDialog("Unsolvable",
                "No solution found under current rules.",
                "OK");
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
            steps++; // metric

            grid[row, col] = num;

            if(IsCellValid(row, col) && SolveRecursive())
            {
                return true;
            }

            // Backtrack
            grid[row, col] = 0;
            backtracks++; // metric
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
