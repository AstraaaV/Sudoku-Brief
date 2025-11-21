#include "Solver.h"
#include "Rules.h"

/*
    So this is the full recursive backtracking.

    So it:
    - Finds empty cell
    - Then tries numbers 1-9
    - Checks if rules allow the number
    - If yes then place it and recurse
    - If recursion fails then undo move (backtrack)
    - If nothing works then puzzle is impossible under these rules
*/
bool Solver::solve(Grid& grid)
{
    int row, col;

    // If empty cell is not found then puzzle is solved
    if (!findEmpty(grid, row, col))
    {
        return true;
    }

    // Tries every number from 1 to 9
    for (int num = 1; num <= 9; num++)
    {
        // Asks current ruleset if number is allowed
        if (rules->isValid(grid, row, col, num))
        {
            // Temp places the number
            grid.set(row, col, num);

            // Recurses and tries to finish puzzle
            if (solve(grid))
            {
                return true; // puzzle solved
            }

            // Otherwise, backtrack
            grid.set(row, col, 0);
        }
    }
    // If nothing worked then dead end
    return false;
}

// Just loops through entire grid searching for empty cell
bool Solver::findEmpty(const Grid& grid, int& row, int& col)
{
    for (row = 0; row < 9; row++)
    {
        for (col = 0; col < 9; col++)
        {
            if (grid.get(row, col) == 0)
            {
                return true; // found empty cell and filled row/col
            }
        }
    }
    return false; // no empty cell found -> solved puzzle
}

