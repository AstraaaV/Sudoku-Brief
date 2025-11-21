#include "Solver.h"
#include "Rules.h"

bool Solver::solve(Grid& grid)
{
    int row, col;

    if (!findEmpty(grid, row, col))
    {
        return true;
    }

    for (int num = 1; num <= 9; num++)
    {
        if (Rules::isValid(grid, row, col, num))
        {
            grid.set(row, col, num);

            if (solve(grid))
            {
                return true;
            }

            grid.set(row, col, 0);
        }
    }
    return false;
}

bool Solver::findEmpty(const Grid& grid, int& row, int& col)
{
    for (row = 0; row < 9; row++)
    {
        for (col = 0; col < 9; col++)
        {
            if (grid.get(row, col) == 0)
            {
                return true;
            }
        }
    }
    return false;
}

